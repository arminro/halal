using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HalAl2020_buy8qd.Problems.GameArmyOptimizer
{
    public class GAOProvider
    {
        const string CONFIG = "Salary.txt";
        public static int Gold { get; set; }

        public static IList<Unit> Units { get; set; }

        static GAOProvider()
        {
            LoadArmyUnits();
        }     

        public static float CalculateMassValorAsFitness(IList<Unit> units)
        {
            if(units.Sum(u => u.Cost) > 100)
            {
                return 0;
            }

            return units.Sum(u => u.Valor);
        }

        public static IList<Army> InitializePopulation(IList<Unit> basePool, int maxPopulation)
        {
            IList<Army> pop = new List<Army>();
            decimal smallestCost = basePool.Min(u => u.Cost);
            for (int i = 0; i < maxPopulation; i++)
            {
                pop.Add(GetRandomPermutation(basePool.OrderByDescending(u => u.Cost), Gold, smallestCost));
            }

            return pop;
        }

        static Army GetRandomPermutation(IOrderedEnumerable<Unit> basePool, int goldLimit, decimal smallestCost)
        {
            IList<Unit> army = new List<Unit>();
            decimal currentGold = goldLimit;

            
            while (currentGold > smallestCost)
            {
                var recruit = GetRandomUnit(basePool, currentGold, basePool.First().Cost);
                army.Add(recruit);
                currentGold -= recruit.Cost;
            }

            return new Army()
            {
                SolutionFragments = army
            };
        }

        static Unit GetRandomUnit(IEnumerable<Unit> basePool, decimal currentGold, decimal largestCost)
        {
            // we need filtering if we cannot afford one or more of the units
            var newPool = basePool;
            if (currentGold < largestCost)
            {
                newPool = basePool.SkipWhile(u => u.Cost > currentGold);
            }

            return newPool.ElementAt(Utils.random.Next(0, newPool.Count()));
        }

        static void LoadArmyUnits()
        {
            var parse = File.ReadAllLines(CONFIG)
                .Select(s => s.Split('\t'));

            Gold = int.Parse(parse.ElementAt(0)[0]);

            Units = parse.Skip(1)
                .Select(crd => new Unit() { Valor = int.Parse(crd[0]), Cost = decimal.Parse(crd[1]) })
                .ToList();
        }
    }
}
