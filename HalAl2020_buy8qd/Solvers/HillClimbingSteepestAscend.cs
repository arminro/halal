using HalAl2020_buy8qd.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HalAl2020_buy8qd.Solvers
{
    public class HillClimbingSteepestAscend<TSol, TSolFragment>
        where TSol : ISolution<TSolFragment>, new()
        where TSolFragment : class
    {
        public static TSol Solve(IList<TSolFragment> towns,
            Func<IList<TSolFragment>, float> calculateFitness,
            int eps,
            int maxIteration)
        {
            var p = GetRandomPermuation(towns);
            bool stuck = false;

            int iteration = 0;
            while (!stuck && !StopCondition(iteration, maxIteration))
            {
                var q = GetRouteFromEpsilonDistanceRoutesWithMinimalFitness(p, eps, 5000, calculateFitness);
                p.Fitness = calculateFitness(p.SolutionFragments);

                if (q.Fitness < p.Fitness)
                {
                    p = q;
                }
                else
                {
                    stuck = true;
                }
                Console.WriteLine($"{iteration}. p: {p.Fitness}");
                iteration++;
            }
            return p;
        }

        public static TSol GetRouteFromEpsilonDistanceRoutesWithMinimalFitness(TSol p, int epsilon, int numberOfEpsilons, Func<IList<TSolFragment>, float> calculateFitness)
        {
            // we use a limit on epsilons so that the program wont crash
            List<TSol> solutions = new List<TSol>();
            for (int i = 0; i < numberOfEpsilons; i++)
            {
                solutions.Add(GetRandomElementWithEpsDifference(p, epsilon, calculateFitness));
            }
            return solutions.Min();
        }

        public static bool StopCondition(int currentIter, int maxIteration)
        {
            return currentIter > maxIteration;
        }

        public static float CalculateFitnessForSolution(IList<TSolFragment> route, Func<TSolFragment, TSolFragment, float> calculatedifferenceBetweenFragments)
        {
            float routeLength = 0;
            for (int i = 0; i < route.Count - 1; i++)
            {
                TSolFragment town1 = route[i];
                TSolFragment town2 = route[i + 1];

                routeLength += calculatedifferenceBetweenFragments(town1, town2);
            }

            return routeLength;
        }

        public static TSol GetRandomPermuation(IList<TSolFragment> basePool)
        {
            IList<TSolFragment> result = new List<TSolFragment>(basePool.Count + 2); // the start and stop is not part of the path now

            TSolFragment origin = basePool[Utils.random.Next(0, basePool.Count)];

            // start
            result.Add(origin);

            var pool = basePool.Where(t => !t.Equals(origin)).ToList();

            for (int i = 0; i < basePool.Count - 1; i++)
            {
                TSolFragment town = pool.ElementAt(Utils.random.Next(0, pool.Count()));
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

        static TSol GetRandomElementWithEpsDifference(TSol original, int epsilon, Func<IList<TSolFragment>, float> calculateFitness)
        {
            TSolFragment[] elements = new TSolFragment[original.SolutionFragments.Count];
            Queue<TSolFragment> unusedElements = new Queue<TSolFragment>();
            if (epsilon > original.SolutionFragments.Count)
            {
                throw new ArgumentException("Letöröm a kezed"); // I <3 prog2
            }

            int numberOfIdenticalStops = original.SolutionFragments.Count - epsilon;

            // an eleigible element is a splice of the original list with the length of numberOfIdenticalStops
            // the order of towns also has to be the same in the spliced segment
            int splice = Utils.random.Next(0, epsilon);

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

            // the origin s fixed
            // due to hwo rand.Next works, we cannot roll the last element
            if (splice == 0)
            {
                elements[elements.Length - 1] = elements[0];

                // but to preserve eps we have to remove 1 element that is not at the beginning/end
                int removeRandIdx = Utils.random.Next(1, splice + numberOfIdenticalStops);
                unusedElements.Enqueue(elements[removeRandIdx]);
                elements[removeRandIdx] = null;
            }

            else
            {
                TSolFragment town = unusedElements.Dequeue();
                elements[0] = town;
                elements[elements.Length - 1] = town;
                //unusedElements = unusedElements.Where(u => u != town);
            }

            // shuffle:https://forgetcode.com/appium/2593-extension-method-to-shuffle-an-ienumerable-in-c

            var unused = new Queue<TSolFragment>(unusedElements.OrderBy(x => Guid.NewGuid()));
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

    }
}
