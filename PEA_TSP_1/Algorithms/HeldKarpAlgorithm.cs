using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    class HeldKarpAlgorithm : IAlgorithm
    {
        private Graph _graph;
        private AlgorithmResult _result;
        private int _startVertex;

        public AlgorithmResult Result => _result;

        public HeldKarpAlgorithm(Graph graph, int startVertex)
        {
            _graph = graph;
            _result = new AlgorithmResult();
            _startVertex = startVertex;
        }

        public void Invoke()
        {
            var tempGraph = _graph.Vertices.ToHashSet();
            tempGraph.Remove(_startVertex);

            _result = HeldKarp(_startVertex, tempGraph);
            _result.Path.Reverse(); //wrong side
        }

        private AlgorithmResult HeldKarp(int start, HashSet<int> verticesSet)
        {
            if (!verticesSet.Any())
            {
                var result = new AlgorithmResult()
                {
                    Weight = _graph.GetWeight(start, _startVertex)
                };
                result.Path.Add(_startVertex);
                result.Path.Add(start);
                return result;
            }

            int totalWeight = Int32.MaxValue;
            AlgorithmResult reccurResult = new AlgorithmResult();

            foreach (var vertex in verticesSet)
            {
                //set without current vertex
                var tempSet = new HashSet<int>(verticesSet);
                tempSet.Remove(vertex);

                AlgorithmResult otherResult = HeldKarp(vertex, tempSet);
                
                int weight = _graph.GetWeight(start, vertex);
                int currentWeight = weight + otherResult.Weight;

                if (currentWeight < totalWeight)
                {
                    totalWeight = currentWeight;
                    reccurResult.Weight = currentWeight;
                    reccurResult.Path = otherResult.Path;
                    reccurResult.Path.Add(start);
                }
            }

            return reccurResult;
        }
    }
}
