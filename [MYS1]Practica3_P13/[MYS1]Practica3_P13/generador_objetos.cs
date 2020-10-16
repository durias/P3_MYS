using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimioAPI;
using SimioAPI.Extensions;
using SimioAPI.Graphics;
using Simio;
using Simio.SimioEnums;
using QlmLicenseLib;
using System.Drawing.Printing;

namespace _MYS1_Practica3_P13
{
    class generador_objetos
    {
        private ISimioProject proyectoApi;
        private string rutabase = "[MYS1]ModeloBase_P13.spfx";
        private string rutafinal = "[MYS1]ModeloFinal_P13.spfx";
        private string[] warnings;
        private IModel model;
        private IIntelligentObjects intelligentObjects;

        public generador_objetos() {
            proyectoApi = SimioProjectFactory.LoadProject(rutabase, out warnings);
            model = proyectoApi.Models[1];
            intelligentObjects = model.Facility.IntelligentObjects;
        }

        public void crearModelo()
        {
           
            crear_modelo();
            SimioProjectFactory.SaveProject(proyectoApi, rutafinal, out warnings);
        }
        //Inicio de la creación del modelo 
        public void crear_modelo() {
            crear_mapa();
            crear_fronteras();
            crear_base_naval();
            crear_regiones();
            enlazar_regiones();
            crear_aeropuertos();
        }

        public void crear_mapa() {
            int[,] array2D = new int[,] {   { 50,-100},{-40,-100},{-40,-80},{-60,-80 },{-40,-60},{-33,-55},
                                            {-20,-40}, {-10,-30},{-10,-20},{-70,-20},{-100,25},{-90,35},{-95,50},
                                            {-100,60},{-70,80},{-50,90},{-20,90},{0,100},{20,90},{35,75},{30,65},
                                            {35,65},{50,50}, {50,35},{70,20},{90,10},{100,0},{75,5},{50,-10}};
            //Creación de transfernodes y enlaces
            for (int i = 0; i < array2D.GetLength(0); i++)
            {
                createTransferNode("TN" + i, (array2D[i, 0] / 2) + 10, (array2D[i, 1] / 2) - 25);
                if (i > 0 && i <= 28)
                {
                    createConveyor(get_nodo("TN" + (i - 1)), get_nodo("TN" + i));
                }
            }
            createConveyor(get_nodo("TN28"), get_nodo("TN0"));
        }

        public void crear_base_naval() {
            //Source de aviones
            createSource("Base_Naves", 22, -65);
            set_propiedad("Base_Naves", "InterarrivalTime", "Random.Exponential(15)");
            set_propiedad("Base_Naves", "MaximumArrivals", "15");
            createPath(get_multi_Nodo("base_Naves", 0), get_nodo("TN0"));
        }

        // Crea los conveyors que representan las fronteras, con sus tamaños y velocidad de los aviones
        public void crear_fronteras() { 
            //Frontera con Mexico
            for ( int i = 1; i < 14;i++){
                set_tamano_conveyor("conveyor" + i, "74000");
                set_velocidad_conveyor("conveyor" + i, "16.67");
            }
            //Pacifico
            for (int i = 14; i < 18; i++)
            {
                set_tamano_conveyor("conveyor" + i, "63500");
                set_velocidad_conveyor("conveyor" + i, "16.67");
            }
            //El Salvador
            for (int i = 18; i < 22; i++)
            {
                set_tamano_conveyor("conveyor" + i, "50750");
                set_velocidad_conveyor("conveyor" + i, "16.67");
            }
            //Honduras
            for (int i = 22; i < 27; i++)
            {
                set_tamano_conveyor("conveyor" + i, "51200");
                set_velocidad_conveyor("conveyor" + i, "16.67");
            }
            //El caribe
            set_tamano_conveyor("conveyor27", "74000");
            set_velocidad_conveyor("conveyor27", "16.67");
            set_tamano_conveyor("conveyor28", "74000");
            set_velocidad_conveyor("conveyor28", "16.67");
            //Frontera con Belice
            set_tamano_conveyor("conveyor29", "266000");
           set_velocidad_conveyor("conveyor29", "16.67");
        }

        public void crear_regiones() {
            
            //-------------------- Region metropolitana--------------------------------------------------
            createSource("So_Metropolitana", 0, 0);
            set_tasa_source("So_Metropolitana", "Random.Poisson(2)");
            createServer("Se_Metropolitana", 0+5, 0);
            set_tasa_server("Se_Metropolitana", "Random.Exponential(4)");
            createPath(get_multi_Nodo("So_Metropolitana", 0), get_multi_Nodo("Se_Metropolitana", 0));
            createTransferNode("TN_Metropolitana", 0 + 5, -5);
            set_propiedad("TN_Metropolitana", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_Metropolitana", 1),get_nodo("TN_Metropolitana") );
            //Salida desde metropolitana
            createTransferNode("Reingreso_Metropolitana", 3, 4);
            set_propiedad("Reingreso_Metropolitana", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_nodo("Reingreso_Metropolitana"), get_multi_Nodo("Se_Metropolitana", 0));
            set_propiedad("Path4", "SelectionWeight", "0.5");
            createSink("Salida_metropolitana",4,7);
            createPath(get_nodo("Reingreso_Metropolitana"), get_multi_Nodo("Salida_metropolitana", 0));
            set_propiedad("Path5", "SelectionWeight", "0.5");

            //-----------------------Region norte--------------------------------------------------------
            createSource("So_Norte", 5, -20);
            set_tasa_source("So_Norte", "Random.Poisson(8)");
            createServer("Se_Norte",5+5, -20);
            set_tasa_server("Se_Norte", "Random.Exponential(5)");
            createPath(get_multi_Nodo("So_Norte", 0), get_multi_Nodo("Se_Norte", 0));
            createTransferNode("TN_Norte", 5 + 5, -25);
            set_propiedad("TN_Norte", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_Norte", 1), get_nodo("TN_Norte"));
            //---------------------- Region nor oriente----------------------------------------------------
            createSource("So_Nor_oriente", 30 , -10);
            set_tasa_source("So_Nor_oriente", "Random.Poisson(6)");
            createServer("Se_Nor_oriente", 30+5 , -14);
            set_tasa_server("Se_Nor_oriente", "Random.Exponential(3)");
            createPath(get_multi_Nodo("So_Nor_oriente", 0), get_multi_Nodo("Se_Nor_oriente", 0));
            createTransferNode("TN_Nor_oriente", 30 + 10, -19);
            set_propiedad("TN_Nor_oriente", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_Nor_oriente", 1), get_nodo("TN_Nor_oriente"));
            //----------------------- Region sur oriente ---------------------------------------------------
            createSource("So_Sur_oriente", 15-5, 20-5);
            set_tasa_source("So_Sur_oriente", "Random.Poisson(10)");
            createServer("Se_Sur_oriente", 15, 20-5);
            set_tasa_server("Se_Sur_oriente", "Random.Exponential(4)");
            createPath(get_multi_Nodo("So_Sur_oriente", 0), get_multi_Nodo("Se_Sur_oriente", 0));
            createTransferNode("TN_Sur_oriente", 15, 20-10);
            set_propiedad("TN_Sur_oriente", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_Sur_oriente", 1), get_nodo("TN_Sur_oriente"));
            //------------------------ Region central -------------------------------------------------------
            createSource("So_central", -15, 20-5);
            set_tasa_source("So_central", "Random.Poisson(3)");
            createServer("Se_central", -15+5, 20-5);
            set_tasa_server("Se_central", "Random.Exponential(5)");
            createPath(get_multi_Nodo("So_central", 0), get_multi_Nodo("Se_central", 0));
            createTransferNode("TN_central", -15+5, 20-10);
            set_propiedad("TN_central", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_central", 1), get_nodo("TN_central"));
            //-------------------------- Region Sur Occidente ------------------------------------------------
            createSource("So_Sur_occidente", -40+10, 10-5);
            set_tasa_source("So_Sur_occidente", "Random.Poisson(4)");
            createServer("Se_Sur_occidente", -40+10 + 5, 10-5);
            set_tasa_server("Se_Sur_occidente", "Random.Exponential(3)");
            createPath(get_multi_Nodo("So_Sur_occidente", 0), get_multi_Nodo("Se_Sur_occidente", 0));
            createTransferNode("TN_Sur_occidente", -40 + 10+5, 10-10);
            set_propiedad("TN_Sur_occidente", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_Sur_occidente", 1), get_nodo("TN_Sur_occidente"));
            //Salida desde sur occidente
            createTransferNode("Reingreso_Sur_occidente", -26, 9);
            set_propiedad("Reingreso_Sur_occidente", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_nodo("Reingreso_Sur_occidente"), get_multi_Nodo("Se_Sur_occidente", 0));
            set_propiedad("Path16", "SelectionWeight", "0.6");
            createSink("Salida_Sur_occidente", -23, 11);
            createPath(get_nodo("Reingreso_Sur_occidente"), get_multi_Nodo("Salida_Sur_occidente", 0));
            set_propiedad("Path17", "SelectionWeight", "0.4");
            //-------------------------- Region Nor Occidente -------------------------- 
            createSource("So_Nor_occidente", -40+10, -20);
            set_tasa_source("So_Nor_occidente", "Random.Poisson(12)");
            createServer("Se_Nor_occidente", -40+15, -20);
            set_tasa_server("Se_Nor_occidente", "Random.Exponential(6)");
            createPath(get_multi_Nodo("So_Nor_occidente", 0), get_multi_Nodo("Se_Nor_occidente", 0));
            createTransferNode("TN_Nor_occidente", -40 + 15, -25);
            set_propiedad("TN_Nor_occidente", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_Nor_occidente", 1), get_nodo("TN_Nor_occidente"));
            //-------------------------- Region Peten -------------------------- 
            createSource("So_Peten",  10, -50);
            set_tasa_source("So_Peten", "Random.Poisson(4)");
            createServer("Se_Peten",  10+5, -50);
            set_tasa_server("Se_Peten", "Random.Exponential(4)");
            createPath(get_multi_Nodo("So_Peten", 0), get_multi_Nodo("Se_Peten", 0));
            createTransferNode("TN_Peten", 10+5, -45);
            set_propiedad("TN_Peten", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_multi_Nodo("Se_Peten", 1), get_nodo("TN_Peten"));
            // Salida desde peten
            createTransferNode("Reingreso_Peten", 10, -45);
            set_propiedad("Reingreso_Peten", "OutboundLinkRule", "ByLinkWeight");
            createPath(get_nodo("Reingreso_Peten"), get_multi_Nodo("Se_Peten", 0));
            set_propiedad("Path22", "SelectionWeight", "0.7");
            createSink("Salida_Peten", 7,-41);
            createPath(get_nodo("Reingreso_Peten"), get_multi_Nodo("Salida_Peten", 0));
            set_propiedad("Path23", "SelectionWeight", "0.3");

        }

        /// Metodos para crear los conveyors que representan las rutas entre regiones
        /// Hay dos formas una para objetos con dos nodos y con un nodo
        public void crear_conexion_regiones(string nombre,String actual, String destino,String probabilidad,String distancia) {
            createConveyor(get_nodo(actual), get_multi_Nodo(destino, 0));
            set_tamano_conveyor(nombre, distancia);
            set_velocidad_conveyor(nombre, "19.44");
            set_prob_conveyor(nombre, probabilidad);
        }
        public void crear_conexion_regiones_nodo_nodo(string nombre, String actual, String destino, String probabilidad, String distancia)
        {
            createConveyor(get_nodo(actual), get_nodo(destino));
            set_tamano_conveyor(nombre, distancia);
            set_velocidad_conveyor(nombre, "19.44");
            set_prob_conveyor(nombre, probabilidad);
        }

        //Enlaza las regiones haciendo llamadas a los métodos crear_conexion_regiones y crear_conexion_regiones_nodo_nodo 
        public void enlazar_regiones() {
            crear_conexion_regiones_nodo_nodo("Conveyor30", "TN_Metropolitana", "Reingreso_Metropolitana", "0.35", "0");
            crear_conexion_regiones("Conveyor31", "TN_Metropolitana", "Se_central", "0.30", "63000");
            crear_conexion_regiones("Conveyor32", "TN_Metropolitana", "Se_Sur_oriente", "0.15", "124000");
            crear_conexion_regiones("Conveyor33", "TN_Metropolitana", "Se_Nor_oriente", "0.20", "241000");

            crear_conexion_regiones("Conveyor34", "TN_Norte", "Se_Norte", "0.40", "0");
            crear_conexion_regiones_nodo_nodo("Conveyor35", "TN_Norte", "Reingreso_Peten", "0.40", "147000");
            crear_conexion_regiones("Conveyor36", "TN_Norte", "Se_Nor_oriente", "0.10", "138000");
            crear_conexion_regiones("Conveyor37", "TN_Norte", "Se_Nor_occidente", "0.10", "145000");

            crear_conexion_regiones("Conveyor38", "TN_Nor_oriente", "Se_Nor_oriente", "0.20", "0");
            crear_conexion_regiones_nodo_nodo("Conveyor39", "TN_Nor_oriente", "Reingreso_Metropolitana", "0.30", "241000");
            crear_conexion_regiones("Conveyor40", "TN_Nor_oriente", "Se_Norte", "0.15", "138000");
            crear_conexion_regiones("Conveyor41", "TN_Nor_oriente", "Se_Sur_oriente", "0.05", "231000");
            crear_conexion_regiones_nodo_nodo("Conveyor42", "TN_Nor_oriente", "Reingreso_Peten", "0.30", "282000");

            crear_conexion_regiones("Conveyor43", "TN_Sur_oriente", "Se_Sur_oriente", "0.40", "0");
            crear_conexion_regiones("Conveyor44", "TN_Sur_oriente", "Se_Nor_oriente", "0.20", "231000");
            crear_conexion_regiones_nodo_nodo("Conveyor45", "TN_Sur_oriente", "Reingreso_Metropolitana", "0.25", "124000");
            crear_conexion_regiones("Conveyor46", "TN_Sur_oriente", "Se_central", "0.15", "154000");

            crear_conexion_regiones("Conveyor47", "TN_central", "Se_central", "0.35", "0");
            crear_conexion_regiones_nodo_nodo("Conveyor48", "TN_central", "Reingreso_Metropolitana", "0.35", "63000");
            crear_conexion_regiones("Conveyor49", "TN_central", "Se_Sur_oriente", "0.05", "154000");
            crear_conexion_regiones_nodo_nodo("Conveyor50", "TN_central", "Reingreso_Sur_occidente", "0.15", "155000");
            crear_conexion_regiones("Conveyor51", "TN_central", "Se_Nor_occidente", "0.10", "269000");

            crear_conexion_regiones_nodo_nodo("Conveyor52", "TN_Sur_occidente", "Reingreso_Sur_occidente", "0.35", "0");
            crear_conexion_regiones("Conveyor53", "TN_Sur_occidente", "Se_Nor_occidente", "0.30", "87000");
            crear_conexion_regiones("Conveyor54", "TN_Sur_occidente", "Se_central", "0.35", "155000");

            crear_conexion_regiones("Conveyor55", "TN_Nor_Occidente", "Se_Nor_Occidente", "0.40", "0");
            crear_conexion_regiones_nodo_nodo("Conveyor56", "TN_Nor_Occidente", "Reingreso_Sur_occidente", "0.30", "87000");
            crear_conexion_regiones("Conveyor57", "TN_Nor_Occidente", "Se_central", "0.10", "269000");
            crear_conexion_regiones("Conveyor58", "TN_Nor_Occidente", "Se_Norte", "0.20", "145000");

            crear_conexion_regiones_nodo_nodo("Conveyor59", "TN_Peten", "Reingreso_Peten", "0.5", "0");
            crear_conexion_regiones("Conveyor60", "TN_Peten", "Se_Norte", "0.25", "147000");
            crear_conexion_regiones("Conveyor61", "TN_Peten", "Se_Nor_oriente", "0.25", "282000");

        }

        //Creación de los elementos de los 3 aeropuertos
        public void crear_aeropuertos() {
            createSource("La_Aurora", 0, -5);
            set_tasa_source("La_Aurora", "Random.Exponential(35)");
            set_entidades_arribo("La_Aurora","70");
            createPath(get_multi_Nodo("La_Aurora", 0), get_multi_Nodo("Se_Metropolitana", 0));

            createSource("Mundo_Maya", 10, -55);
            set_tasa_source("Mundo_Maya", "Random.Exponential(50)");
            set_entidades_arribo("Mundo_Maya", "40");
            createPath(get_multi_Nodo("Mundo_Maya", 0), get_multi_Nodo("Se_Peten", 0));

            createSource("Inter_Quetzaltenago", -30, 0);
            set_tasa_source("Inter_Quetzaltenago", "Random.Exponential(70)");
            set_entidades_arribo("Inter_Quetzaltenago", "30");
            createPath(get_multi_Nodo("Inter_Quetzaltenago", 0), get_multi_Nodo("Se_Sur_occidente", 0));

        }

        //--------------- Métodos para creación de obejtos diversos --------------------------------

        public void createTransferNode(string nombre, int x, int y)
        {
            intelligentObjects.CreateObject("TransferNode", new FacilityLocation(x, 0, y));
            model.Facility.IntelligentObjects["TransferNode1"].ObjectName = nombre;
        }

        public void createSource(string nombre, int x, int y)
        {
            intelligentObjects.CreateObject("Source", new FacilityLocation(x, 0, y));
            model.Facility.IntelligentObjects["Source1"].ObjectName = nombre;
        }

        public void set_tasa_source(string nombre, string tasa) {
            set_propiedad(nombre,"InterarrivalTime",tasa);
        }

        public void set_entidades_arribo(string nombre, string entidades)
        {
            set_propiedad(nombre, "EntitiesPerArrival", entidades);
        }

        public void createServer(string nombre, int x, int y)
        {
            intelligentObjects.CreateObject("Server", new FacilityLocation(x, 0, y));
            model.Facility.IntelligentObjects["Server1"].ObjectName = nombre;
        }

        public void set_tasa_server(string nombre, string tasa)
        {
            set_propiedad(nombre, "ProcessingTime", tasa);
        }

        public void createSink(string nombre, int x, int y)
        {
            intelligentObjects.CreateObject("Sink", new FacilityLocation(x, 0, y));
            model.Facility.IntelligentObjects["Sink1"].ObjectName = nombre;
        }

        public void set_propiedad(String name, String property, String value)
        {
            model.Facility.IntelligentObjects[name].Properties[property].Value = value;
        }

        public void set_tamano_conveyor(string nombre, string distancia) {
            set_propiedad(nombre, "DrawnToScale", "False");
            set_propiedad(nombre, "LogicalLength", distancia);
        }

        public void set_velocidad_conveyor(string nombre, string velocidad) {
            set_propiedad(nombre,"InitialDesiredSpeed",velocidad);
        }

        public void set_prob_conveyor(string nombre, string probabilidad)
        {
            set_propiedad(nombre, "SelectionWeight", probabilidad);
        }

        public void createPath(INodeObject nodo1, INodeObject nodo2)
        {
            this.createLink("Path", nodo1, nodo2);
        }
        public void createTimePath(INodeObject nodo1, INodeObject nodo2)
        {
            this.createLink("TimePath", nodo1, nodo2);
        }

        public void createConveyor(INodeObject nodo1, INodeObject nodo2)
        {
            this.createLink("Conveyor", nodo1, nodo2);
        }
        //Devuelve uno de los nodos que se espeficican con el entero nodo
        public INodeObject get_multi_Nodo(String name, int nodo)
        {
            return ((IFixedObject)model.Facility.IntelligentObjects[name]).Nodes[nodo];
        }
        //Devuelve el nodo de un objeto que solamente tiene un enlace
        public INodeObject get_nodo(String name)
        {
            return (INodeObject)model.Facility.IntelligentObjects[name];
        }

        public void createLink(String type, INodeObject nodo1, INodeObject nodo2)
        {
            intelligentObjects.CreateLink(type, nodo1, nodo2, null);
        }

        
    }
}
