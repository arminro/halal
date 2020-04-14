﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TravellingSalesman
{
    public class Town
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