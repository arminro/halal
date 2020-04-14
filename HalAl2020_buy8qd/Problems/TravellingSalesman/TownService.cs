using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TravellingSalesman
{
    public class TownService
    {

        const string CONFIG_NAME = "towns.txt";

        #region ProblemFunctions


        //public static IList<Route> InitilizePopulation(IList<Town> towns, int numberOfPopulation)
        //{
        //    // generating numberOfPopulation pieces of unique routes
        //    Town startPoint = towns[random.Next(0, towns.Count)];
        //    List<List<Town>> townsByPosition = new List<List<Town>>(numberOfPopulation);
        //    IList<Route> routes = new List<Route>(numberOfPopulation);

        //    int numberOfTowns = towns.Count;

        //    for (int i = 0; i < numberOfPopulation; i++)
        //    {
        //        townsByPosition.Add(new List<Town>(numberOfTowns+1));
        //        routes.Add(new Route()
        //        {
        //            RoutePath = new List<Town>(numberOfTowns+1)
        //            {
        //                startPoint
        //            }
        //        });
        //    }

        //    var townsWithoutStartPoint = towns.Where(t => !t.Equals(startPoint));
        //    for (int i = 0; i < numberOfPopulation; i++)
        //    {
        //        // deep copy
        //        var routePool = townsWithoutStartPoint.ToList();
        //        for (int j = 1; j < numberOfTowns; j++)
        //        {
        //            // choosing a town that is unique for this position as well within the route
        //            var uniqeForThisPosition = routePool
        //                .Where(t => !townsByPosition[i].Contains(t))
        //                .ToList();

        //            Town routeStop = uniqeForThisPosition[random.Next(0, uniqeForThisPosition.Count())];
        //            routes[i].RoutePath.Add(routeStop);

        //            routePool.Remove(routeStop);
        //            townsByPosition[i].Add(routeStop);
        //        }

        //    }

        //    foreach (Route route in routes)
        //    {
        //        route.RoutePath.Add(startPoint);
        //    }

        //    return routes;
        //}

        public static Route GetRandomPermuation(IList<Town> basePool)
        {
            IList<Town> result = new List<Town>(basePool.Count + 2); // the start and stop is not part of the path now

            Town origin = basePool[random.Next(0, basePool.Count)];

            // start
            result.Add(origin);

            var pool = basePool.Where(t => t != origin).ToList();

            for (int i = 0; i < basePool.Count - 1; i++)
            {
                Town town = pool.ElementAt(random.Next(0, pool.Count()));
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



        public static float CalculateDistanceForRoute(IList<Town> route)
        {
            float routeLength = 0;
            for (int i = 0; i < route.Count -1; i++)
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

        //static void EvaluateFitness(IList<Route> routes, Func<IList<Town>, float> calculateFitness)
        //{
        //    foreach (var route in routes)
        //    {
        //        route.Fitness = calculateFitness(route.RoutePath);
        //    }
        //}

        

        public static bool StopCondition(int currentIter)
        {
            return currentIter > 30;
        }

        public static Route GetRouteFromEpsilonDistanceRoutesWithMinimalFitness(Route p, int epsilon, int numberOfEpsilons)
        {
            // we use a limit on epsilons so that the program wont crash
            List<Route> routes = new List<Route>();
            for (int i = 0; i < numberOfEpsilons; i++)
            {
                routes.Add(GetRandomElementWithEpsDifference(p, epsilon));
            }
            return routes.Min();
        }



        static Route GetRandomElementWithEpsDifference(Route original, int epsilon)
        {
            Town[] elements = new Town[original.RoutePath.Count];
            Queue<Town> unusedElements = new Queue<Town>();
            if(epsilon > original.RoutePath.Count)
            {
                throw new ArgumentException("Letöröm a kezed"); // the  "" marks has duble meaning (I <3 prog2)
            }

            int numberOfIdenticalStops = original.RoutePath.Count - epsilon;

            // an eleigible element is a splice of the original list with the length of numberOfIdenticalStops
            // the order of towns is also has to be the same in the spliced segment
            int splice = random.Next(0, epsilon);

            for (int i = 0; i < original.RoutePath.Count; i++)
            {
                if(i >= splice && i < splice + numberOfIdenticalStops)
                {
                    elements[i] = original.RoutePath[i];
                }
                else
                {
                    unusedElements.Enqueue(original.RoutePath[i]);
                }
            }

            //var unusedElements = original.RoutePath.Where(t => !elements.Contains(t));
            // the origin s fixed
            // due to hwo rand.Next works, we cannot roll the last element
            if (splice == 0)
            {
                elements[elements.Length - 1] = elements[0];

                // but to preserve eps we have to remove 1 element that is not at the beginning/end
                int removeRandIdx = random.Next(1, splice + numberOfIdenticalStops);
                unusedElements.Enqueue(elements[removeRandIdx]);
                elements[removeRandIdx] = null;
            }

            //else if (splice + numberOfIdenticalStops == original.RoutePath.Count)
            //{
            //    elements[0] = elements[elements.Length - 1];

            //    // but to preserve eps we have to remove 1 element that is not at the beginning/end
            //    int removeRandIdx = random.Next(1, elements.Length - 1);
            //    unusedElements.Append(elements[removeRandIdx]);
            //    elements[removeRandIdx] = null;
            //}

            else
            {
                Town town = unusedElements.Dequeue();
                elements[0] = town;
                elements[elements.Length - 1] = town;
                //unusedElements = unusedElements.Where(u => u != town);
            }

            // shuffle:https://forgetcode.com/appium/2593-extension-method-to-shuffle-an-ienumerable-in-c
        
            var unused = new Queue<Town>(unusedElements.OrderBy(x => Guid.NewGuid()));
            for (int i = 0; i < elements.Length; i++)
            {
                if(elements[i] == null)
                {
                    elements[i] = unused.Dequeue();
                }
            }

            return new Route()
            {
                RoutePath = elements,
                Fitness = CalculateDistanceForRoute(elements)
            };


        }



        //public static void GeneticAlgorithm(IList<Town> towns,
        //    Func<IList<Town>, float> calculateFitness,
        //    Func<int, bool> StopCondition,
        //    int numberOfTowns,
        //    int populationNumber)
        //{
        //    var population = InitilizePopulation(towns, numberOfTowns, populationNumber);
        //    EvaluateFitness(population, CalculateFitnessForRoute);
        //    var best = 

        //}

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
