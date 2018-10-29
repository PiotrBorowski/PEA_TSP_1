using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    internal class HeldKarpAlgorithm : IAlgorithm
    {
        private readonly Graph _graph;
        private AlgorithmResult _result;
        private readonly int _startVertex;
        private readonly Dictionary<string, int> _weightOfSets;

        public AlgorithmResult Result => _result;

        public string Name { get; set; }

        public HeldKarpAlgorithm(Graph graph, int startVertex)
        {
            _graph = graph;
            _result = new AlgorithmResult();
            _startVertex = startVertex;
            _weightOfSets = new Dictionary<string, int>();            
        }

        public void Invoke()
        {
            _weightOfSets.Clear();
            var tempGraph = _graph.Vertices.ToHashSet();
            tempGraph.Remove(_startVertex);

            _result = HeldKarp(_startVertex, tempGraph);
            var b = _weightOfSets;
            //var weightres = _weightOfSets["0,5,1,9,6,7,8,2,4,3,0,"];
             _result.Path.Reverse();
        }

        private AlgorithmResult HeldKarp(int end, HashSet<int> verticesSet)
        {
            if (!verticesSet.Any())
            {
                var result = new AlgorithmResult()
                {
                    Weight = _graph.GetWeight(end, _startVertex)
                };
                result.Path.Add(_startVertex);
                result.Path.Add(end);
                return result;
            }

            int totalWeight = Int32.MaxValue;
            var reccurResult = new AlgorithmResult();

            foreach (var vertex in verticesSet)
            {
                //set without current vertex
                var tempSet = new HashSet<int>(verticesSet);
                tempSet.Remove(vertex);
                var otherResult = new AlgorithmResult();
                string Path;

                Path = $"{_startVertex},";
                if (tempSet.Any())
                {
                    Path += string.Join(",", tempSet) + ",";
                }               
                Path += $"{vertex},";

                if (!_weightOfSets.TryGetValue(Path, out var weightFromMemory))
                {
                    otherResult = HeldKarp(vertex, tempSet);

                    var innerPath = string.Join(",", otherResult.Path) + ",";
                    _weightOfSets[innerPath] = otherResult.Weight;
                }
                else
                {
                    otherResult.Weight = weightFromMemory;
                    otherResult.Path = new List<int>();
                    otherResult.Path.Add(_startVertex);        
                    otherResult.Path.AddRange(tempSet);
                    otherResult.Path.Add(vertex);
                }

                int weight = _graph.GetWeight(end, vertex);
                int currentWeight = weight + otherResult.Weight;

                if (currentWeight < totalWeight)
                {
                    totalWeight = currentWeight;
                    reccurResult.Weight = currentWeight;
                    reccurResult.Path = otherResult.Path;
                    reccurResult.Path.Add(end);
                }
            }

            return reccurResult;
        }
    }
}
