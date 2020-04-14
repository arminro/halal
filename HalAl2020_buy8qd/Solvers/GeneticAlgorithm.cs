using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TravellingSalesman;

namespace HalAl2020_buy8qd
{
    public class GeneticAlgorithm
    {
        // couples with joined fitness above this will 100% not mate
        const int MATING_BARRIER = 10000;
        public static Route Solve(IList<Town> basepool,
            int matingPercent,
            int maxGenerationCount,
            int selectionPressure,
            int simultaneousMatingCount,
            int populationCount)
        {
            var population = InitializePopulation(basepool, populationCount);
            Evaluate(population, CalculateDistanceForRoute);
            var p_best = GetElementWithMinimalFitness(population);
            
            int generation = 0;
            
            while (!Utils.Utils.LoopStopCondition(generation, maxGenerationCount) && population.Count > 0)
            {

                // best matingPoolPercent will be breeding
                int matingGroundSize = (int)(population.Count * ((float)matingPercent / 100));
                var matingPool = new List<Route>(SelectNPercentBestParent(population, CalculateDistanceForRoute, matingGroundSize, selectionPressure)); 
                IList <Route> nextPopulation = new List<Route>(matingGroundSize);
                while (nextPopulation.Count < populationCount)
                {
                    // selecting p1, p2....pk
                    List<Route> grouppen = new List<Route>(simultaneousMatingCount);
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
                Evaluate(population, CalculateDistanceForRoute);
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

        private static void Mutate(Route offspring)
        {          
           int mutationWindow = Utils.Utils.random.Next(3, 20);

           // mutate with switching elements over an area of mutationWindow
           var values = offspring.RoutePath;
           int startingPoint = Utils.Utils.random.Next(0, offspring.RoutePath.Count - mutationWindow); // window should fit
           int endpoint = startingPoint + mutationWindow;

           for (int i = startingPoint; i < endpoint; i++)
           {
               Town temp = values[i % endpoint];
               values[i % endpoint] = values[(i + 1) % endpoint];
               values[(i + 1) % endpoint] = temp;
           }            
        }

        private static Route CrossOverFitness(int basesPopCount, IList<Route> parents)
        {
            // generating crossing points between cromosomes
            int[] crossingPoints = new int[Utils.Utils.random.Next(3, 31)];
            for (int i = 0; i < crossingPoints.Length; i++)
            {
                crossingPoints[i] = Utils.Utils.random.Next(0, basesPopCount);
            }

            return SwitchCromosomes(parents, crossingPoints);
        }

        static Route SwitchCromosomes(IList<Route> parents, int[] crossingPoints)
        {
            var basis = (Route)parents[Utils.Utils.random.Next(0, parents.Count)].Clone();
            for (int i = 0; i < parents.Count; i++)
            {
                foreach (var point in crossingPoints)
                {
                    if (!parents[i].Equals(basis))
                    {
                        basis.RoutePath[point] = parents[i].RoutePath[point];
                    }
                }
            }
            return basis;
        }

        static IEnumerable<Route> SelectNPercentBestParent(IList<Route> population, Func<IList<Town>, float> calculateFitness, int matingGroundVolume, int k)
        {
            return population.OrderBy(pop => pop.Fitness)
                .Take(matingGroundVolume);
        }

        static Route GetElementWithMinimalFitness(IList<Route> population)
        {
            return population.Min();
        }

        static IList<Route> InitializePopulation(IList<Town> basePool, int initialPopulationCount)
        {
            IList<Route> pop = new List<Route>(initialPopulationCount);
            for (int i = 0; i < initialPopulationCount; i++)
            {
                pop.Add(GetRandomPermuation(basePool));
            }

            return pop;
        }

        static void Evaluate(IList<Route> routes, Func<IList<Town>, float> calculateFitness)
        {
            foreach (var populationElement in routes)
            {
                populationElement.Fitness = calculateFitness(populationElement.RoutePath);
            }
        }

        

        public static float CalculateDistanceForRoute(IList<Town> route)
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

        static float CalculateDistanceBetweenTowns(Town town1, Town town2)
        {
            return (float)Math.Sqrt(Math.Pow((town2.X - town1.X), 2) + Math.Pow((town2.Y - town1.Y), 2));
        }

        static Route GetRandomPermuation(IList<Town> basePool)
        {
            IList<Town> result = new List<Town>(basePool.Count + 2); // the start and stop is not part of the path now

            Town origin = basePool[Utils.Utils.random.Next(0, basePool.Count)];

            // start
            result.Add(origin);

            var pool = basePool.Where(t => t != origin).ToList();

            for (int i = 0; i < basePool.Count - 1; i++)
            {
                Town town = pool.ElementAt(Utils.Utils.random.Next(0, pool.Count()));
                result.Add(town);
                pool.Remove(town);
            }

            // back to origin
            result.Add(origin);
            return new Route()
            {
                RoutePath = result
            };
        }
    }
}
