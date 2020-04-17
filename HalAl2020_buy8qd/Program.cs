using HalAl2020_buy8qd.Problems.FunctionApproximation;
using HalAl2020_buy8qd.Problems.GameArmyOptimizer;
using HalAl2020_buy8qd.Problems.TravellingSalesman;
using HalAl2020_buy8qd.Solvers;
using System;


namespace HalAl2020_buy8qd
{
    class Program
    {
        static void Main(string[] args)
        {

            #region TravelingSalesman_HillClimbingSteepestAscend

            //var population = TSProvider.LoadTownsConfig();
            //var opt = HillClimbingSteepestAscend<Route, Town>.Solve(population,
            //    TSProvider.CalculateRouteLengthAsFitness,
            //    5, 30);

            #endregion

            #region TravelingSalesmen_GeneticAlgorithm

            //var population = TSProvider.LoadTownsConfig();
            //var opt = GeneticAlgorithm<Route, Town>.Solve(population,
            //    TSProvider.CalculateRouteLengthAsFitness,
            //    5,
            //    population.Count,
            //    TSProvider.InitializePopulation,
            //    GeneticAlgorithm<Route, Town>.GetElementWithMinimalFitness,
            //    GeneticAlgorithm<Route, Town>.SelectNPercentBestParentByMin,
            //    GeneticAlgorithm<Route, Town>.MutateGeneSequenceForList,
            //    50, 100, 10000);

            #endregion


            #region FunctionApproximation_GeneticAlgorithm

            //var opt = GeneticAlgorithm<Function, float>.Solve(null,
            //    FAProvider.CalculateFitnessAsDifferenceToPredifinedValues,
            //    5,
            //    5,
            //    FAProvider.InitializePopulation,
            //    GeneticAlgorithm<Function, float>.GetElementWithMinimalFitness,
            //    GeneticAlgorithm<Function, float>.SelectNPercentBestParentByMin,
            //    GeneticAlgorithm<Function, float>.MutateGeneSequenceForNumber,
            //    50, 1, 10000);

            #endregion

            #region GameArmyOptimizer_GeneticAlgorithm

            var opt = GeneticAlgorithm<Army, Unit>.Solve(GAOProvider.Units,
                GAOProvider.CalculateMassValorAsFitness,
                5,
                GAOProvider.Units.Count,
                GAOProvider.InitializePopulation,
                GeneticAlgorithm<Army, Unit>.GetElementWithMaximalFitness,
                GeneticAlgorithm<Army, Unit>.SelectNPercentBestParentByMax,
                GeneticAlgorithm<Army, Unit>.MutateGeneSequenceForNumber,
                50, 10, 10000);

            #endregion


            Console.WriteLine(opt.VictoryRide());
            Console.WriteLine("Press ENTER to end");

            Console.ReadLine();
        }
    }
}
