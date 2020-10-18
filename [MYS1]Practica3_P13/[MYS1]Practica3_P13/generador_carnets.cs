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
    class generador_carnets
    {
        private ISimioProject proyectoApi;
        private string rutabase = "[MYS1]ModeloBase_P13.spfx";
        private string rutafinal = "[MYS1]ModeloFinal_ID_P13.spfx";
        private string[] warnings;
        private IModel model;
        private IIntelligentObjects intelligentObjects;

        public generador_carnets() {
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
            display_7_segmentos(true,true,false,true,true,false,true,"d1", 0, 0);            //2
            display_7_segmentos(true, true, true, true, true, true, false, "d2", 15, 0);     //0
            display_7_segmentos(false, true, true, false, false, false, false, "d3", 25, 0); //1
            display_7_segmentos(true, true, true, true, false, false, true, "d4", 45, 0);    //3
            display_7_segmentos(false, true, true, false, false, false, false, "d5", 55, 0); //1
            display_7_segmentos(false, true, true, false, false, true, true, "d6", 75, 0);   //4
            display_7_segmentos(true, false, true, true, false, true, true, "d7", 90, 0);    //5
            display_7_segmentos(true, false, true, true, true, true, true, "d8", 105, 0);    //6
            display_7_segmentos(true, false, true, true, false, true, true, "d9", 120, 0);   //5

            display_7_segmentos(true, true, false, true, true, false, true, "r1", 0, 30);     //2
            display_7_segmentos(true, true, true, true, true, true, false, "r2", 15, 30);     //0
            display_7_segmentos(false, true, true, false, false, false, false, "r3", 25, 30); //1
            display_7_segmentos(true, true, true, true, false, false, true, "r4", 45, 30);    //3
            display_7_segmentos(false, true, true, false, false, false, false, "r5", 60, 30); //1
            display_7_segmentos(false, true, true, false, false, true, true, "r6", 75, 30);   //4
            display_7_segmentos(false, true, true, false, false, true, true, "r7", 90, 30);   //4
            display_7_segmentos(true, true, true, false, false, false, false, "r8", 105, 30);  //7
            display_7_segmentos(false, true, true, false, false, true, true, "r9", 120, 30);   //4
        }

        public void display_7_segmentos(Boolean a, Boolean b, Boolean c, Boolean d, Boolean e, Boolean f, Boolean g,string id_unico,int x, int y) {
            Boolean v1 = false;
            Boolean v2 = false;
            Boolean v3 = false;
            Boolean v4 = false;
            Boolean v5 = false;
            Boolean v6 = false;

            if (a) {
                if (!v1) {
                    createTransferNode("V1_" + id_unico, x, y);
                    v1 = true;
                }
                if (!v2)
                {
                    createTransferNode("V2_" + id_unico, x+10, y);
                    v2 = true;
                }
                createPath(get_nodo("V1_" + id_unico), get_nodo("V2_" + id_unico));
            }
            if (b) {
                if (!v2)
                {
                    createTransferNode("V2_" + id_unico, x+10, y);
                    v2 = true;
                }
                if (!v3)
                {
                    createTransferNode("V3_" + id_unico, x+10, y+10);
                    v3 = true;
                }
                createPath(get_nodo("V2_" + id_unico), get_nodo("V3_" + id_unico));
            }
            if (c) {
                if (!v3)
                {
                    createTransferNode("V3_" + id_unico, x+10, y+10);
                    v3 = true;
                }
                if (!v4)
                {
                    createTransferNode("V4_" + id_unico, x+10, y+20);
                    v4 = true;
                }
                createPath(get_nodo("V3_" + id_unico), get_nodo("V4_" + id_unico));
            }
            if (d) {
                if (!v4)
                {
                    createTransferNode("V4_" + id_unico, x+10, y+20);
                    v4 = true;
                }
                if (!v5)
                {
                    createTransferNode("V5_" + id_unico, x, y+20);
                    v5 = true;
                }
                createPath(get_nodo("V4_" + id_unico), get_nodo("V5_" + id_unico));
            }
            if (e) {
                if (!v5)
                {
                    createTransferNode("V5_" + id_unico, x, y+20);
                    v5 = true;
                }
                if (!v6)
                {
                    createTransferNode("V6_" + id_unico, x, y+10);
                    v6 = true;
                }
                createPath(get_nodo("V5_" + id_unico), get_nodo("V6_" + id_unico));
            }
            if (f) {
                if (!v6)
                {
                    createTransferNode("V6_" + id_unico, x, y+10);
                    v6 = true;
                }
                if (!v1)
                {
                    createTransferNode("V1_" + id_unico, x, y);
                    v1 = true;
                }
                createPath(get_nodo("V6_" + id_unico), get_nodo("V1_" + id_unico));
            }
            if (g) {
                if (!v6)
                {
                    createTransferNode("V6_" + id_unico, x, y+10);
                    v6 = true;
                }
                if (!v3)
                {
                    createTransferNode("V3_" + id_unico, x+10, y+10);
                    v3 = true;
                }
                createPath(get_nodo("V6_" + id_unico), get_nodo("V3_" + id_unico));
            }



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
