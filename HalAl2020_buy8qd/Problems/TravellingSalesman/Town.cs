using HalAl2020_buy8qd.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HalAl2020_buy8qd.Problems.TravellingSalesman
{
    public class Town : IComparable
    {
        public int X { get; set; }
        public int Y { get; set; }

    
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }


            Town other = obj as Town;

            if (this.X == other.X && this.Y == other.Y)
            {
                return 0;
            }

            else if (this.X < other.X && this.Y < other.Y)
            {
                return -1;
            }

            return 1;
        }

        public override bool Equals(object obj)
        {
            Town other = obj as Town;
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
