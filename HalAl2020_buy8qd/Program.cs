using HalAl2020_buy8qd.Problems.FunctionApproximation;
using HalAl2020_buy8qd.Problems.TravellingSalesman;
using HalAl2020_buy8qd.Solvers;
using System;


namespace HalAl2020_buy8qd
{
    class Program
    {
        static void Main(string[] args)
        {
            //var population = TSProvider.LoadTownsConfig();

            //var opt = HillClimbingSteepestAscend<Route, Town>.Solve(population,
            //    TSProvider.CalculateRouteLengthAsFitness,
            //    30, 30);
            //var now = DateTime.Now.GetHashCode();
            //var opt = SimulatedAnnealing<Route, Town>.Solve(population, TSProvider.CalculateRouteLengthAsFitness, 30, now, 500, 4);

            var opt = GeneticAlgorithm<Function, float>.Solve(null,
                FAProvider.CalculateFitnessAsDifferenceToPredifinedValues,
                5,
                5,
                FAProvider.InitializePopulation,
                140, 3, 10000);

            Console.WriteLine(opt.VictoryRide());
            Console.WriteLine("Press ENTER to end");

            Console.ReadLine();
        }
    }
}
