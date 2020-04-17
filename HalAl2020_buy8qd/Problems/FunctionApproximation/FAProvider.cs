using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HalAl2020_buy8qd.Problems.FunctionApproximation
{
    public class FAProvider
    {
        const string CONFIG = "FuncAppr1.txt";

        public static readonly IDictionary<float, float> InputsByFunctionOutputs = new Dictionary<float, float>();

        static FAProvider()
        {
            LoadFunctionConfig();
        }

        public static float CalculateEquation(IList<float> coefficients, float x)
        {
            // f(x) = a*(x-b)^3 + c*(x-d)^2 +e*x
            var result = coefficients[0] * Math.Pow(x - coefficients[1], 3) + coefficients[2] * Math.Pow(x - coefficients[3], 2) + coefficients[4] * x;
            return (float)result;
        }

        public static float CalculateFitnessAsDifferenceToPredifinedValues(IList<float> coefficients)
        {
            float sum_diff = 0;

            foreach (var valuepair in InputsByFunctionOutputs)
            {
                float x = valuepair.Key;
                float y = CalculateEquation(coefficients, x);

                float diff = (float)Math.Pow(y - valuepair.Value, 2);

                sum_diff += diff;
            }

            return sum_diff;
        }

        public static void LoadFunctionConfig()
        {
            var parse = File.ReadAllLines(CONFIG)
                .Select(s => s.Split('\t'));

            foreach (string[] coord in parse)
            {
                if (coord.Length > 1)
                {
                    InputsByFunctionOutputs.Add(float.Parse(coord[0]), float.Parse(coord[1])); 
                }
            }
        }

        public static IList<Function> InitializePopulation(IList<float> basePool, int initialPopulationCount)
        {
            IList<Function> pop = new List<Function>(initialPopulationCount);
            for (int i = 0; i < initialPopulationCount; i++)
            {
                pop.Add(GetRandomPermuation());
            }

            return pop;
        }

        static Function GetRandomPermuation()
        {
            IList<float> elements = new List<float>(5); // had to change last minute :(
            for (int i = 0; i < 5; i++)
            {
                elements.Add((float)Utils.random.NextDouble() * 50);
            }

            return new Function()
            {
                SolutionFragments = elements
            };

        }
    }
}
