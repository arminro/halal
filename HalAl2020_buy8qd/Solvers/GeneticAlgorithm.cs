using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalAl2020_buy8qd.Common;
using HalAl2020_buy8qd.Utils;

namespace HalAl2020_buy8qd.Solvers
{
    public class GeneticAlgorithm<TSol, TSolFragment>
        where TSol: ISolution<TSolFragment>, new()
        where TSolFragment:ISolutionFragment
    {
        // couples with joined fitness above this will 100% not mate
        const int MATING_BARRIER = 10000;
        public static TSol Solve(IList<TSolFragment> basepool,
            Func<IList<TSolFragment>, float> calculateFitness,
            int matingPercent,
            int maxGenerationCount,
            int simultaneousMatingCount,
            int populationCount)
        {
            var population = InitializePopulation(basepool, populationCount);
            Evaluate(population, calculateFitness);
            var p_best = GetElementWithMinimalFitness(population);
            
            int generation = 0;
            
            while (!Utils.Utils.LoopStopCondition(generation, maxGenerationCount) && population.Count > 0)
            {

                // best matingPoolPercent will be breeding
                int matingGroundSize = (int)(population.Count * ((float)matingPercent / 100));
                var matingPool = new List<TSol>(SelectNPercentBestParent(population, matingGroundSize)); 
                IList<TSol> nextPopulation = new List<TSol>(matingGroundSize);
                while (nextPopulation.Count < populationCount)
                {
                    // selecting p1, p2....pk
                    List<TSol> grouppen = new List<TSol>(simultaneousMatingCount);
                    for (int i = 0; i < simultaneousMatingCount; i++)
                    {
                        var swinger = matingPool[Utils.Utils.random.Next(0, matingPool.Count)];
                        grouppen.Add(swinger);
                    }

                    var offspring = CrossOverFitness(basepool.Count, grouppen);
                    Mutate(offspring);

                    nextPopulation.Add(offspring);
                }
                population = nextPopulation;
                Evaluate(population, calculateFitness);
                p_best = GetElementWithMinimalFitness(population);
                Console.WriteLine($"Genetaion{generation}: {p_best.Fitness}");

                generation++;
            }

            return p_best;
        }

        public static bool MatingPoolSmallerThanPressure(int matingPoolCount, int pressure)
        {
            return matingPoolCount < pressure;
        }

        private static void Mutate(TSol offspring)
        {          
           int mutationWindow = Utils.Utils.random.Next(3, 20);

           // mutate with switching elements over an area of mutationWindow
           var values = offspring.SolutionFragments;
           int startingPoint = Utils.Utils.random.Next(0, offspring.SolutionFragments.Count - mutationWindow); // window should fit
           int endpoint = startingPoint + mutationWindow;

           for (int i = startingPoint; i < endpoint; i++)
           {
               TSolFragment temp = (TSolFragment)values[i % endpoint];
               values[i % endpoint] = values[(i + 1) % endpoint];
               values[(i + 1) % endpoint] = temp;
           }            
        }

        private static TSol CrossOverFitness(int basesPopCount, IList<TSol> parents)
        {
            // generating crossing points between cromosomes
            int[] crossingPoints = new int[Utils.Utils.random.Next(3, 31)];
            for (int i = 0; i < crossingPoints.Length; i++)
            {
                crossingPoints[i] = Utils.Utils.random.Next(0, basesPopCount);
            }

            return SwitchCromosomes(parents, crossingPoints);
        }

        static TSol SwitchCromosomes(IList<TSol> parents, int[] crossingPoints)
        {
            var basis = (TSol)parents[Utils.Utils.random.Next(0, parents.Count)].Clone();
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

        static IEnumerable<TSol> SelectNPercentBestParent(IList<TSol> population, int matingGroundVolume)
        {
            return population.OrderBy(pop => pop.Fitness)
                .Take(matingGroundVolume);
        }

        static TSol GetElementWithMinimalFitness(IList<TSol> population)
        {
            return population.Min();
        }

        static IList<TSol> InitializePopulation(IList<TSolFragment> basePool, int initialPopulationCount)
        {
            IList<TSol> pop = new List<TSol>(initialPopulationCount);
            for (int i = 0; i < initialPopulationCount; i++)
            {
                pop.Add(GetRandomPermuation(basePool));
            }

            return pop;
        }

        static void Evaluate(IList<TSol> TSols, Func<IList<TSolFragment>, float> calculateFitness)
        {
            foreach (var populationElement in TSols)
            {
                populationElement.Fitness = calculateFitness(populationElement.SolutionFragments);
            }
        }


        static TSol GetRandomPermuation(IList<TSolFragment> basePool)
        {
            IList<TSolFragment> result = new List<TSolFragment>(basePool.Count + 2); // the start and stop is not part of the path now

            TSolFragment origin = basePool[Utils.Utils.random.Next(0, basePool.Count)];

            // start
            result.Add(origin);

            var pool = basePool.Where(t => !t.Equals(origin)).ToList();

            for (int i = 0; i < basePool.Count - 1; i++)
            {
                TSolFragment TSolFragment = pool.ElementAt(Utils.Utils.random.Next(0, pool.Count()));
                result.Add(TSolFragment);
                pool.Remove(TSolFragment);
            }

            // back to origin
            result.Add(origin);
            return new TSol()
            {
                SolutionFragments = result
            };
        }
    }
}
