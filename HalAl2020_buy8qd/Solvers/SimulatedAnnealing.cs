using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalAl2020_buy8qd.Common;
using HalAl2020_buy8qd.Utils;

namespace HalAl2020_buy8qd
{
    public class SimulatedAnnealing<TSol, TSolFragment>
        where TSol : ISolution<TSolFragment>, new()
        where TSolFragment : class, ISolutionFragment
    {
        public static TSol Solve(IList<TSolFragment> towns,
            Func<IList<TSolFragment>, float> calculateFitness,
            int eps,
            int constant,
            int maxTemperature,
            int alpha)
        {
            var p = GetRandomPermuation(towns);
            p.Fitness = calculateFitness(p.SolutionFragments);
            var p_opt = p;

            int t = 0;
            float deltaE = 0;
            while(!StopCondition(t, maxTemperature))
            {
                var q = GetSolutionFromEpsilonDistanceWithRandomFitness(p, eps, 5000, calculateFitness);
                
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

        public static TSol GetSolutionFromEpsilonDistanceWithRandomFitness(TSol p, int epsilon, int numberOfEpsilons, Func<IList<TSolFragment>, float> calculateFitness)
        {
            // we use a limit on epsilons so that the program wont crash
            List<TSol> routes = new List<TSol>();
            for (int i = 0; i < numberOfEpsilons; i++)
            {
                routes.Add(GetRandomElementWithEpsDifference(p, epsilon, calculateFitness));
            }

            return routes[Utils.Utils.random.Next(0, routes.Count)];
        }

        static TSol GetRandomElementWithEpsDifference(TSol original, int epsilon, Func<IList<TSolFragment>, float> calculateFitness)
        {
            TSolFragment[] elements = new TSolFragment[original.SolutionFragments.Count];
            Queue<TSolFragment> unusedElements = new Queue<TSolFragment>();
            if (epsilon > original.SolutionFragments.Count)
            {
                throw new ArgumentException("Letöröm a kezed"); // (I <3 prog2)
            }

            int numberOfIdenticalStops = original.SolutionFragments.Count - epsilon;

            // an eleigible element is a splice of the original list with the length of numberOfIdenticalStops
            // the order of towns is also has to be the same in the spliced segment
            int splice = Utils.Utils.random.Next(0, epsilon);

            for (int i = 0; i < original.SolutionFragments.Count; i++)
            {
                if (i >= splice && i < splice + numberOfIdenticalStops)
                {
                    elements[i] = original.SolutionFragments[i];
                }
                else
                {
                    unusedElements.Enqueue(original.SolutionFragments[i]);
                }
            }

            //var unusedElements = original.RoutePath.Where(t => !elements.Contains(t));
            // the origin is fixed
            // due to hwo rand.Next works, we cannot roll the last element
            if (splice == 0)
            {
                elements[elements.Length - 1] = elements[0];

                // but to preserve eps we have to remove 1 element that is not at the beginning/end
                int removeRandIdx = Utils.Utils.random.Next(1, splice + numberOfIdenticalStops);
                unusedElements.Enqueue(elements[removeRandIdx]);
                elements[removeRandIdx] = null;
            }

            else
            {
                TSolFragment town = unusedElements.Dequeue();
                elements[0] = town;
                elements[elements.Length - 1] = town;
            }

            // shuffle:https://forgetcode.com/appium/2593-extension-method-to-shuffle-an-ienumerable-in-c
            var unused = new Queue<TSolFragment>(unusedElements.OrderBy(t => Guid.NewGuid()));
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i] == null)
                {
                    elements[i] = unused.Dequeue();
                }
            }

            return new TSol()
            {
                SolutionFragments = elements,
                Fitness = calculateFitness(elements)
            };


        }

        

        public static TSol GetRandomPermuation(IList<TSolFragment> basePool)
        {
            IList<TSolFragment> result = new List<TSolFragment>(basePool.Count + 2); // the start and stop is not part of the path now

            TSolFragment origin = basePool[Utils.Utils.random.Next(0, basePool.Count)];

            // start
            result.Add(origin);

            var pool = basePool.Where(t => t != origin).ToList();

            for (int i = 0; i < basePool.Count - 1; i++)
            {
                TSolFragment town = pool.ElementAt(Utils.Utils.random.Next(0, pool.Count()));
                result.Add(town);
                pool.Remove(town);
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
