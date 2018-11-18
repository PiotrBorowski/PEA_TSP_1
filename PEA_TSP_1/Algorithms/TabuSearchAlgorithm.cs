using PEA_TSP_1.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    public class TabuSearchAlgorithm : IAlgorithm
    {
        public AlgorithmResult Result { get; set; }
        public string Name { get; set; }

        private Queue<Move> tabuList;
        private StopCondition stopCondition;
        private int maxTabuSize = 50;
        private Graph _graph;

        public TabuSearchAlgorithm(Graph graph)
        {
            _graph = graph;
            stopCondition = new StopCondition();
        }

        public void Invoke()
        {
            var init = new TabuAlgorithmResult()
            {
                Path = new List<int> { 0,1,2,3,4,5,6,7,8,9},
                Weight = 999
            };

            var result = TabuSearch(init);

            Result = new AlgorithmResult{Path = result.Path, Weight = result.Weight};
        }

        public TabuAlgorithmResult TabuSearch(TabuAlgorithmResult initialSolution)
        {
            TabuAlgorithmResult bestSolution = initialSolution;
            TabuAlgorithmResult currentSolution = initialSolution;
            tabuList = new Queue<Move>();
            stopCondition.MaxIterations = 5000;

            int currentIteration = 0;
            while (!stopCondition.mustStop(++currentIteration))
            {
                TabuAlgorithmResult bestNeighborFound =
                    FindBestNeighbor(currentSolution, tabuList.ToList(), out var move);

                if (bestNeighborFound.Weight < bestSolution.Weight)
                {
                    bestSolution = bestNeighborFound;
                }

                tabuList.Enqueue(move);
                currentSolution = bestNeighborFound;

                if (tabuList.Count > maxTabuSize)
                {
                    tabuList.Dequeue();
                }
            }

            return bestSolution;
        }


        public TabuAlgorithmResult FindBestNeighbor(TabuAlgorithmResult currentSolution, List<Move> tabuMoves,
            out Move move)
        {
            int bestCost = Int32.MaxValue;
            move = new Move(0,0);
            TabuAlgorithmResult bestResult = new TabuAlgorithmResult();
            var pathList = new List<int>(currentSolution.Path);

            for (int i = 0; i < currentSolution.Path.Count; i++)
            {
                for (int j = 1; j < currentSolution.Path.Count; j++)
                {
                    var currMove = new Move(i, j);
                    if (j != i && !tabuMoves.Contains(currMove))
                    {
                        TabuAlgorithmResult temp = new TabuAlgorithmResult();
                        temp.Path = Swap(i, j, pathList);
                        int currCost = temp.CalculateWeight(_graph);

                        if (currCost < bestCost)
                        {
                            bestCost = currCost;
                            move = currMove;
                            bestResult = temp;
                        }
                    }
                }
            }

            return bestResult;
        }

        public List<int> Swap(int x, int y, List<int> path)
        {
            int temp = path[x];
            path[x] = path[y];
            path[y] = temp;

            return path;
        }
    }

    internal class StopCondition
    {
        public int MaxIterations { get; set; }

        public bool mustStop(int current)
        {
            return current >= MaxIterations;
        }
    }

    public class TabuAlgorithmResult : AlgorithmResult
    {
        public int CalculateWeight(Graph _graph)
        {
            int weight = 0;
            for (int i = 0; i < _graph.NumberOfCities-1; i++)
            {
                weight += _graph.GetWeight(Path[i],Path[i+1]);
            }
            weight += _graph.GetWeight(Path[_graph.NumberOfCities - 1], 0);

            Weight = weight;
            return weight;
        }
    }

    public class Move
    {
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }

        public Move(int v1, int v2)
        {
            Vertex1 = v1;
            Vertex2 = v2;
        }
    }
}


