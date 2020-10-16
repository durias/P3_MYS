using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _MYS1_Practica3_P13
{
    class Coordenada
    {
        private int x;
        private int y;

        public Coordenada(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int get_x() {
            return this.x;

        }

        public int get_y()
        {
            return this.y;

        }
    }
}
