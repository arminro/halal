using HalAl2020_buy8qd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HalAl2020_buy8qd.Problems.TravellingSalesman
{
    public class Route :  ISolution<Town>
    {
        public float Fitness { get; set; }
        public IList<Town> SolutionFragments { get; set; }

        public object Clone()
        {
            Route route = new Route();
            route.Fitness = this.Fitness;

            route.SolutionFragments = new List<Town>();
            foreach (var item in this.SolutionFragments)
            {
                route.SolutionFragments.Add(item);
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

            if(this.Fitness == other.Fitness)
            {
                return 0;
            }

            else if(this.Fitness < other.Fitness)
            {
                return -1;
            }

            return 1;
        }

        public override string ToString()
        {
            return Fitness.ToString();
        }

        public string VictoryRide()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in this.SolutionFragments)
            {
                builder.Append($" {item.X},{item.Y} ");
            }

            return builder.ToString();
        }
    }
}
