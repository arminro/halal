using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalAl2020_buy8qd.Common;


namespace HalAl2020_buy8qd.Solvers
{
    public class GeneticAlgorithm<TSol, TSolFragment>
        where TSol: ISolution<TSolFragment>, new()
    {
        // couples with joined fitness above this will 100% not mate
        const int MATING_BARRIER = 10000;
        public static TSol Solve(IList<TSolFragment> basepool,
            Func<IList<TSolFragment>, float> calculateFitness,
            int matingPercent,
            int basePopulationCount,
            Func<IList<TSolFragment>, int, IList<TSol>> initPopulation,
            Func<IList<TSol>, TSol> selector,
            Func<IList<TSol>, int,  IEnumerable<TSol>> selectParents,
            Action<TSol, IList<TSol>, int, int> mutateGeneSequence,
            int maxGenerationCount,
            int simultaneousMatingCount,
            int populationCount)
        {
            var population = initPopulation(basepool, populationCount);
            Evaluate(population, calculateFitness);
            var p_best = selector(population);
            
            int generation = 0;
            
            while (!Utils.LoopStopCondition(generation, maxGenerationCount) && population.Count > 0)
            {

                // best matingPoolPercent will be breeding
                int matingGroundSize = (int)(population.Count * ((float)matingPercent / 100));
                var matingPool = new List<TSol>(selectParents(population, matingGroundSize)); 
                IList<TSol> nextPopulation = new List<TSol>(matingGroundSize);
                while (nextPopulation.Count < populationCount)
                {
                    // selecting p1, p2....pk
                    List<TSol> grouppen = new List<TSol>(simultaneousMatingCount);
                    for (int i = 0; i < simultaneousMatingCount; i++)
                    {
                        var swinger = matingPool[Utils.random.Next(0, matingPool.Count)];
                        grouppen.Add(swinger);
                    }

                    var offspring = CrossOverFitness(basePopulationCount, grouppen);
                    Mutate(offspring, population, mutateGeneSequence);

                    nextPopulation.Add(offspring);
                }
                population = nextPopulation;
                Evaluate(population, calculateFitness);
                p_best = selector(population);
                Console.WriteLine($"Genetaion{generation}: {p_best}");

                generation++;
            }

            return p_best;
        }

        public static bool MatingPoolSmallerThanPressure(int matingPoolCount, int pressure)
        {
            return matingPoolCount < pressure;
        }

        private static void Mutate(TSol offspring, IList<TSol> population, Action<TSol, IList<TSol>,int,int> mutateGeneSequence)
        {          
           int mutationWindow = Utils.random.Next(1, offspring.SolutionFragments.Count/3);

           // mutate with switching elements over an area of mutationWindow
           var values = offspring.SolutionFragments;
           int startingPoint = Utils.random.Next(1, offspring.SolutionFragments.Count - mutationWindow); // window should fit
           int endpoint = startingPoint + mutationWindow;

           mutateGeneSequence(offspring, population, startingPoint, endpoint);           
        }

        private static TSol CrossOverFitness(int basesPopCount, IList<TSol> parents)
        {
            // generating crossing points between cromosomes
            int[] crossingPoints = new int[Utils.random.Next(1, basesPopCount/2)];
            for (int i = 0; i < crossingPoints.Length; i++)
            {
                crossingPoints[i] = Utils.random.Next(1, basesPopCount-1);
            }

            return SwitchCromosomes(parents, crossingPoints);
        }

        static TSol SwitchCromosomes(IList<TSol> parents, int[] crossingPoints)
        {
            var basis = (TSol)parents[Utils.random.Next(0, parents.Count)].Clone();
            for (int i = 0; i < parents.Count; i++)
            {
                foreach (var point in crossingPoints)
                {
                    if (!parents[i].Equals(basis))
                    {
                        basis.SolutionFragments[point] = parents[i].SolutionFragments[point];
                    }
                }
            }
            return basis;
        }

        static void Evaluate(IList<TSol> TSols, Func<IList<TSolFragment>, float> calculateFitness)
        {
            foreach (var populationElement in TSols)
            {
                populationElement.Fitness = calculateFitness(populationElement.SolutionFragments);
            }
        }

        public static T GetElementWithMinimalFitness<T>(IList<T> population)
        {
            return population.Min();
        }

        public static T GetElementWithMaximalFitness<T>(IList<T> population)
        {
            return population.Max();
        }

        public static IEnumerable<TSol> SelectNPercentBestParentByMin(IList<TSol> population, int matingGroundVolume)
        {
            return population.OrderBy(pop => pop.Fitness)
                .Take(matingGroundVolume);
        }

        public static IEnumerable<TSol> SelectNPercentBestParentByMax(IList<TSol> population, int matingGroundVolume)
        {
            return population.OrderByDescending(pop => pop.Fitness)
                .Take(matingGroundVolume);
        }

        public static void MutateGeneSequenceForList(TSol host, IList<TSol> pop, int editStart, int editEnd)
        {
            var values = host.SolutionFragments;
            for (int i = editStart; i < editEnd; i++)
            {
                TSolFragment temp = values[i];
                values[i] = values[i + 1];
                values[i + 1] = temp;
            }

            // make sure that the last always stays the last, but we could have lethal mutations as well
            values[values.Count -1] = values.First();
        }

        public static void MutateGeneSequenceForNumber(TSol host, IList<TSol> pop, int editStart, int editEnd)
        {
            var values = host.SolutionFragments;
            for (int i = editStart; i < editEnd; i++)
            {
                // only those elements can play that already conform to certain preconditions
                int randomPersonIdx = Utils.random.Next(0, pop.Count);
                values[i] = pop[randomPersonIdx]
                            .SolutionFragments[Utils.random.Next(0, pop[randomPersonIdx].SolutionFragments.Count)];
            }
        }
    }
}
