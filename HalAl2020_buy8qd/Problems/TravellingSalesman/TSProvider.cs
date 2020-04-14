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

        #region ProblemFunctions

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

        #endregion



        #region I/O
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
        #endregion

       
    }
}
