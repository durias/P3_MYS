using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace _MYS1_Practica3_P13
{
    public partial class Form1 : Form
    {
       

        public Form1()
        {
            InitializeComponent();
        }

        private void crear_modelo_Click(object sender, EventArgs e)
        {

            generador_objetos gen_ob = new generador_objetos();
            gen_ob.crearModelo();
            MessageBox.Show("Modelo creado con éxito.");

        }

        private void crear_carnets_Click(object sender, EventArgs e)
        {

        }
    }
}
