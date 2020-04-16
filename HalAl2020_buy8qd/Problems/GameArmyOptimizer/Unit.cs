using System;
using System.Collections.Generic;
using System.Text;

namespace HalAl2020_buy8qd.Problems.GameArmyOptimizer
{
    public class Unit : IComparable
    {
        public decimal Cost { get; set; }

        public int Valor { get; set; }

     
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }


            Unit other = obj as Unit;

            if (this.Cost == other.Cost && this.Valor == other.Valor)
            {
                return 0;
            }

            else if (this.Cost < other.Cost && this.Valor < other.Valor)
            {
                return -1;
            }

            return 1;
        }
    }
}
