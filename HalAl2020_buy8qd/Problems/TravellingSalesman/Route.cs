using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravellingSalesman
{
    public class Route :  IComparable, ICloneable
    {
        public float Fitness { get; set; }

        public IList<Town> RoutePath { get; set; }

        public object Clone()
        {
            Route route = new Route();
            route.Fitness = this.Fitness;

            route.RoutePath = new List<Town>();
            foreach (var item in this.RoutePath)
            {
                route.RoutePath.Add(item);
            }

            return route;
        }

        public int CompareTo(object obj)
        {
            if(obj == null)
            {
                return 1;
            }

            Route other = obj as Route;
            if(this.Fitness < other.Fitness)
            {
                return -1;
            }
            return 1;
        }

        public override string ToString()
        {
            return $"Fitness: {Fitness}";
        }
    }
}
