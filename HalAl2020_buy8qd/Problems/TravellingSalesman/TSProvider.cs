using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HalAl2020_buy8qd.Utils;

namespace HalAl2020_buy8qd.Problems.TravellingSalesman
{
    public class TSProvider
    {
        const string CONFIG_NAME = "towns.txt";

        public static float CalculateDistanceBetweenTowns(Town town1, Town town2)
        {
            return (float)Math.Sqrt(Math.Pow((town2.X - town1.X), 2) + Math.Pow((town2.Y - town1.Y), 2));
        }

        public static float CalculateRouteLengthAsFitness(IList<Town> route)
        {
            float routeLength = 0;
            for (int i = 0; i < route.Count - 1; i++)
            {
                Town town1 = route[i];
                Town town2 = route[i + 1];

                routeLength += CalculateDistanceBetweenTowns(town1, town2);
            }

            return routeLength;
        }


        public static IList<Town> LoadTownsConfig()
        {
            IList<Town> towns = new List<Town>();
            var parse = File.ReadAllLines(CONFIG_NAME)
                .Select(s => s.Split('\t'))
                .Select(crd => new Town() { X = int.Parse(crd[0]), Y = int.Parse(crd[1])});
                          
            foreach (Town t in parse)
            {
                towns.Add(t);
            }

            return towns;
        }


        #region GA_ONLY

        public static IList<Route> InitializePopulation(IList<Town> basePool, int initialPopulationCount)
        {
            IList<Route> pop = new List<Route>(initialPopulationCount);
            for (int i = 0; i < initialPopulationCount; i++)
            {
                pop.Add(GetRandomPermuation(basePool));
            }

            return pop;
        }

        static Route GetRandomPermuation(IList<Town> basePool)
        {
            IList<Town> result = new List<Town>(basePool.Count + 2); // the start and stop is not part of the path now

            Town origin = basePool[Utils.Utils.random.Next(0, basePool.Count)];

            // start
            result.Add(origin);

            var pool = basePool.Where(t => !t.Equals(origin)).ToList();

            for (int i = 0; i < basePool.Count - 1; i++)
            {
                Town TSolFragment = pool.ElementAt(Utils.Utils.random.Next(0, pool.Count()));
                result.Add(TSolFragment);
                pool.Remove(TSolFragment);
            }

            // back to origin
            result.Add(origin);
            return new Route()
            {
                SolutionFragments = result
            };
        }

        #endregion
    }
}
