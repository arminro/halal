using HalAl2020_buy8qd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HalAl2020_buy8qd.Problems.GameArmyOptimizer
{
    public class Army : ISolution<Unit>
    {
        public float Fitness { get; set; }
        public IList<Unit> SolutionFragments { get; set; }


        public object Clone()
        {
            Army route = new Army();
            route.Fitness = this.Fitness;

            route.SolutionFragments = new List<Unit>();
            foreach (var item in this.SolutionFragments)
            {
                route.SolutionFragments.Add(item);
            }

            return route;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }


            Army other = obj as Army;

            if (this.Fitness == other.Fitness)
            {
                return 0;
            }

            else if (this.Fitness < other.Fitness)
            {
                return -1;
            }

            return 1;
        }

        public override string ToString()
        {
            return this.Fitness.ToString();
        }

        public string VictoryRide()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var unit in GAOProvider.Units)
            {
                builder.Append($"{unit.Cost}: {SolutionFragments.Count(u => u.Valor == unit.Valor)} units\n");
            }
            builder.Append($"All cost: {SolutionFragments.Sum(u => u.Cost)}");

            return builder.ToString();
        }
    }
}
