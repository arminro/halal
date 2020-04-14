using System;
using TravellingSalesman;

namespace HalAl2020_buy8qd
{
    class Program
    {
        static void Main(string[] args)
        {
            var population = TownService.LoadTownsConfig();

            //var opt = TownService.HillClimbingSteepestAscend(population,
            //    TownService.GetRouteFromEpsilonDistanceRoutesWithMinimalFitness,
            //    60,
            //    TownService.CalculateDistanceForRoute,
            //    TownService.StopCondition);
            //var now = DateTime.Now.GetHashCode();
            //var opt = SimulatedAnnealing.Solve(population, 30, now, 500, 4);

            var opt = GeneticAlgorithm.Solve(population, 5, 300, 4, 3, 10000);

            Console.WriteLine("Press ENTER to end");

            Console.ReadLine();
        }
    }
}
