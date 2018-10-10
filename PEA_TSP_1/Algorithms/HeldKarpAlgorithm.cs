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

        public AlgorithmResult Result => _result;

        public HeldKarpAlgorithm(Graph graph)
        {
            _graph = graph;
            _result = new AlgorithmResult();
        }

        public void Invoke()
        {
            int startVertex = 0;
            var tempGraph = _graph.Vertices.ToHashSet();
            tempGraph.Remove(startVertex);

            _result.Weight = HeldKarp(startVertex, tempGraph);
        }

        private int HeldKarp(int start, HashSet<int> verticesSet)
        {
            if (!verticesSet.Any())
            {
                return _graph.GetWeight(start, 0);
            }

            int totalWeight = Int32.MaxValue;

            foreach (var vertex in verticesSet)
            {
                int weight = _graph.GetWeight(start, vertex);

                var tempSet = new HashSet<int>(verticesSet);
                tempSet.Remove(vertex);

                int otherWeight= HeldKarp(vertex, tempSet);

                int currentWeight = weight + otherWeight;

                if (currentWeight < totalWeight)
                    totalWeight = currentWeight;
            }

            return totalWeight;
        }
    }
}
