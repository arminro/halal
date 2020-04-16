using HalAl2020_buy8qd.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HalAl2020_buy8qd.Problems.FunctionApproximation
{
    public class Function : ISolution<float>
    {
        public float Fitness { get; set; }
        public IList<float> SolutionFragments { get; set; }

        public object Clone()
        {
            Function route = new Function();
            route.Fitness = this.Fitness;

            route.SolutionFragments = new List<float>();
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

            Function other = obj as Function;
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
            StringBuilder builder = new StringBuilder(SolutionFragments.Count);
            foreach (var item in SolutionFragments)
            {
                builder.Append(item.ToString());
                builder.Append(" ");
            }

            return builder.ToString();
        }

        public string VictoryRide()
        {
            StringBuilder builder = new StringBuilder(FAProvider.InputsByFunctionOutputs.Count);

            foreach (var valuepair in FAProvider.InputsByFunctionOutputs)
            {
                float x = valuepair.Key;
                float y = FAProvider.CalculateEquation(SolutionFragments, x);

                builder.Append($"{x}: {y} ({valuepair.Value})\n");
            }

            return builder.ToString();
        }
    }
}
