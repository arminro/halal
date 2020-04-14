using HalAl2020_buy8qd.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HalAl2020_buy8qd.Problems.TravellingSalesman
{
    public class Town : ISolutionFragment
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            Town other = obj as Town;
            return this.X == other.X && this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
