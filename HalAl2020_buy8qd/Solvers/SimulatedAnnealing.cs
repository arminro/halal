using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalAl2020_buy8qd.Utils;

namespace HalAl2020_buy8qd
{
    public class SimulatedAnnealing
    {
        public static Random random = new Random();
        public static Route Solve(IList<Town> towns,
            int eps,
            int constant,
            int maxTemperature,
            int alpha)
        {
            var p = GetRandomPermuation(towns);
            p.Fitness = CalculateDistanceForRoute(p.RoutePath);
            var p_opt = p;

            int t = 0;
            float deltaE = 0;
            while(!StopCondition(t, maxTemperature))
            {
                var q = GetRouteFromEpsilonDistanceRoutesWithRandomFitness(p, eps, 5000);
                
                deltaE += (q.Fitness - p.Fitness);

                if(deltaE < 0)
                {
                    p = q;
                    if(p.Fitness < p_opt.Fitness)
                    {
                        p_opt = p;
                    }                    
                }
                else
                {
                    var temp = GetNextTemperate(t, alpha, maxTemperature);
                    var probability = Math.Pow(Math.E, -1 * (deltaE / constant * temp));
                    
                    if(Utils.Utils.random.NextDouble() < probability)
                    {
                        p = q;
                    }
                }
                t++;
                //Console.Clear();
                Console.WriteLine($"T{t}: {p_opt.Fitness}");
            }
            return p;
        }
        
        public static float GetNextTemperate(int t, int alpha, int tMax)
        {
            var temp = tMax * (float)Math.Pow((1 - (t / tMax)), alpha);
            return temp;
        }

        public static bool StopCondition(int idx, int tMax)
        {
            return idx > tMax;
        }

        public static Route GetRouteFromEpsilonDistanceRoutesWithRandomFitness(Route p, int epsilon, int numberOfEpsilons)
        {
            // we use a limit on epsilons so that the program wont crash
            List<Route> routes = new List<Route>();
            for (int i = 0; i < numberOfEpsilons; i++)
            {
                routes.Add(GetRandomElementWithEpsDifference(p, epsilon));
            }

            return routes[Utils.Utils.random.Next(0, routes.Count)];
        }

        static Route GetRandomElementWithEpsDifference(Route original, int epsilon)
        {
            Town[] elements = new Town[original.RoutePath.Count];
            Queue<Town> unusedElements = new Queue<Town>();
            if (epsilon > original.RoutePath.Count)
            {
                throw new ArgumentException("Letöröm a kezed"); // (I <3 prog2)
            }

            int numberOfIdenticalStops = original.RoutePath.Count - epsilon;

            // an eleigible element is a splice of the original list with the length of numberOfIdenticalStops
            // the order of towns is also has to be the same in the spliced segment
            int splice = Utils.Utils.random.Next(0, epsilon);

            for (int i = 0; i < original.RoutePath.Count; i++)
            {
                if (i >= splice && i < splice + numberOfIdenticalStops)
                {
                    elements[i] = original.RoutePath[i];
                }
                else
                {
                    unusedElements.Enqueue(original.RoutePath[i]);
                }
            }

            //var unusedElements = original.RoutePath.Where(t => !elements.Contains(t));
            // the origin is fixed
            // due to hwo rand.Next works, we cannot roll the last element
            if (splice == 0)
            {
                elements[elements.Length - 1] = elements[0];

                // but to preserve eps we have to remove 1 element that is not at the beginning/end
                int removeRandIdx = random.Next(1, splice + numberOfIdenticalStops);
                unusedElements.Enqueue(elements[removeRandIdx]);
                elements[removeRandIdx] = null;
            }

            else
            {
                Town town = unusedElements.Dequeue();
                elements[0] = town;
                elements[elements.Length - 1] = town;
            }

            // shuffle:https://forgetcode.com/appium/2593-extension-method-to-shuffle-an-ienumerable-in-c
            var unused = new Queue<Town>(unusedElements.OrderBy(t => Guid.NewGuid()));
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null)
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

        public static Route GetRandomPermuation(IList<Town> basePool)
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

        static float CalculateDistanceBetweenTowns(Town town1, Town town2)
        {
            return (float)Math.Sqrt(Math.Pow((town2.X - town1.X), 2) + Math.Pow((town2.Y - town1.Y), 2));
        }
    }
}
