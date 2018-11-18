using PEA_TSP_1.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    public class TabuSearchAlgorithm : IAlgorithm
    {
        public AlgorithmResult Result { get; }
        public string Name { get; set; }

        private Queue<TabuAlgorithmResult> tabuList;
        private StopCondition stopCondition;
        private BestNeighborSolutionLocator solutionLocator;
        private int maxTabuSize = 10;

        public void Invoke()
        {


        }

        public TabuAlgorithmResult TabuSearch(TabuAlgorithmResult initialSolution)
        {
            TabuAlgorithmResult bestSolution = initialSolution;
            TabuAlgorithmResult currentSolution = initialSolution;
            tabuList = new Queue<TabuAlgorithmResult>();
            tabuList.Enqueue(initialSolution);

            int currentIteration = 0;
            while (!stopCondition.mustStop(++currentIteration, bestSolution))
            {

                List<TabuAlgorithmResult> candidateNeighbors = currentSolution.Neighbors;
                List<TabuAlgorithmResult> solutionsInTabu = tabuList.ToList();

                TabuAlgorithmResult bestNeighborFound = solutionLocator.FindBestNeighbor(candidateNeighbors, solutionsInTabu);

                if (bestNeighborFound.Weight < bestSolution.Weight)
                {
                    bestSolution = bestNeighborFound;
                }

                tabuList.Enqueue(bestNeighborFound);

                if (tabuList.Count > maxTabuSize)
                {
                    tabuList.Dequeue();
                }
            }

            return bestSolution;
        }    
    }

}

    internal class BestNeighborSolutionLocator
    {
        public TabuAlgorithmResult FindBestNeighbor(List<TabuAlgorithmResult> neighbors, List<TabuAlgorithmResult> tabuNeighbors)
        {
            foreach (var item in tabuNeighbors)
            {
                neighbors.Remove(item);
            }

            return neighbors.OrderBy(x => x.Weight).First();
        }
    }

    internal class StopCondition
    {
        public int MaxIterations { get; set; }

        public bool mustStop(int current, TabuAlgorithmResult bestResult)
        {
            return current >= MaxIterations;
        }
    }


    public class TabuAlgorithmResult : AlgorithmResult
    {
        public List<TabuAlgorithmResult> Neighbors { get; set; }
    }

