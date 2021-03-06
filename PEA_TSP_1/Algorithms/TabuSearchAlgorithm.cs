﻿using PEA_TSP_1.Algorithms;
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

        private Queue<Move> _tabuList;
        private StopCondition _stopCondition;
        private int _maxTabuSize;
        private int _maxCritCounter;
        private Graph _graph;
        private TabuAlgorithmResult bestSolution;

        public TabuSearchAlgorithm(Graph graph, int maxIterations, int maxTabuSize, int maxCritCounter)
        {
            _graph = graph;
            _stopCondition = new StopCondition();
            _stopCondition.MaxIterations = maxIterations;
            _maxTabuSize = maxTabuSize;
            _maxCritCounter = maxCritCounter;
        }

        public void Invoke()
        {
            var init = TabuAlgorithmResult.GenerateResult(_graph.NumberOfCities);

            init.CalculateWeight(_graph);
            var result = TabuSearch(init);

            Result = new AlgorithmResult{Path = result.Path, Weight = result.Weight};
        }

        public TabuAlgorithmResult TabuSearch(TabuAlgorithmResult initialSolution)
        {
            bestSolution = initialSolution;
            TabuAlgorithmResult currentSolution = initialSolution;
            _tabuList = new Queue<Move>();
            int critCounter = 0;

            int currentIteration = 0;
            while (!_stopCondition.mustStop(++currentIteration))
            {
                //algorytm najblizszego sasiada
                TabuAlgorithmResult bestNeighborFound =
                    FindBestNeighbor(currentSolution, _tabuList.ToList(), out var move);

                if (bestNeighborFound.Weight < bestSolution.Weight)
                {
                    bestSolution = bestNeighborFound;
                    critCounter = 0;
                }
                else
                {
                    //dywersyfikacja
                    critCounter++;
                    if (critCounter == _maxCritCounter && _maxCritCounter != 0)
                    {
                        currentSolution = TabuAlgorithmResult.GenerateResult(_graph.NumberOfCities);
                        critCounter = 0;
                    }
                }

                //dodawanie ostatniego ruchu
                _tabuList.Enqueue(move);
                currentSolution = bestNeighborFound;

                //max tabu size
                if (_tabuList.Count > _maxTabuSize)
                {
                    _tabuList.Dequeue();
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


            for (int i = 0; i < currentSolution.Path.Count; i++)
            {
                for (int j = 1; j < currentSolution.Path.Count; j++)
                {
                    var pathList = new List<int>(currentSolution.Path);
                    if (j != i)
                    {
                        var currMove = new Move(i, j);

                        TabuAlgorithmResult temp = new TabuAlgorithmResult();
                        temp.Path = Swap(i, j, pathList);
                        int currCost = temp.CalculateWeight(_graph);

                        if (!tabuMoves.Contains(currMove))
                        {
                            if (currCost < bestCost || currCost < bestSolution.Weight)
                            {
                                bestCost = currCost;
                                move = new Move(currMove);
                                bestResult = new TabuAlgorithmResult(temp);
                            }
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
        private static Random rng = new Random();

        public TabuAlgorithmResult()
        {
            Path = new List<int>();
        }

        public TabuAlgorithmResult(TabuAlgorithmResult alg)
        {
            Path = new List<int>(alg.Path);
            Weight = alg.Weight;
        }

        public int CalculateWeight(Graph _graph)
        {
            int weight = 0;
            for (int i = 0; i < _graph.NumberOfCities-1; i++)
            {
                weight += _graph.GetWeight(Path[i],Path[i+1]);
            }
            weight += _graph.GetWeight(Path[_graph.NumberOfCities - 1], Path[0]);

            Weight = weight;
            return weight;
        }

        public void Shuffle()
        {
            int n = Path.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = Path[k];
                Path[k] = Path[n];
                Path[n] = value;
            }
        }

        public static TabuAlgorithmResult GenerateResult(int size)
        {
            var init = new TabuAlgorithmResult()
            {
                Path = Enumerable.Range(0, size).ToList()
            };
            init.Shuffle();

            return init;
        }
    }

    public class Move
    {
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }

        public Move(Move move)
        {
            Vertex1 = move.Vertex1;
            Vertex2 = move.Vertex2;
        }

        public Move(int v1, int v2)
        {
            Vertex1 = v1;
            Vertex2 = v2;
        }

        public override bool Equals(object obj)
        {
            var objMove = (Move) obj;

            if ((Vertex1 == objMove.Vertex1 && Vertex2 == objMove.Vertex2) ||
                (Vertex2 == objMove.Vertex1 && Vertex1 == objMove.Vertex2))
                return true;

           return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}


