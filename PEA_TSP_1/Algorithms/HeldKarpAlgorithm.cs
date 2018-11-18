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
             _result.Path.Reverse();
        }

        private AlgorithmResult HeldKarp(int end, HashSet<int> verticesSet)
        {
            if (!verticesSet.Any())
            {
                //jezeli zbior jest pusty to zwracamy wynik dla wierzchołka końcowego dla danego wywołania i startowego globalnego
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
                //zbior bez obecnego wierzchołka
                var tempSet = new HashSet<int>(verticesSet);
                tempSet.Remove(vertex);
                var otherResult = new AlgorithmResult();
                string Path;

                //sciezka
                Path = $"{_startVertex},";
                if (tempSet.Any())
                {
                    Path += string.Join(",", tempSet) + ",";
                }               
                Path += $"{vertex},";

                //jezeli waga dla sciezki bez obecnego wierzchołka znajduje sie w słowniku to ja pobieramy
                if (!_weightOfSets.TryGetValue(Path, out var weightFromMemory))
                {
                    //jezeli nie to wywołujemy rekurencję
                    otherResult = HeldKarp(vertex, tempSet);
                    //i zapisujemy do słownika
                    var innerPath = string.Join(",", otherResult.Path) + ",";
                    _weightOfSets[innerPath] = otherResult.Weight;
                }
                else
                {
                    //jezeli jest w słowniku to przepisujemy wagę
                    otherResult.Weight = weightFromMemory;
                    otherResult.Path = new List<int>();
                    otherResult.Path.Add(_startVertex);        
                    otherResult.Path.AddRange(tempSet);
                    otherResult.Path.Add(vertex);
                }

                int weight = _graph.GetWeight(end, vertex);
                //waga całego cyklu
                int currentWeight = weight + otherResult.Weight;

                //jezeli jest to najkrótsza z dotychczasowych sciezek to ja zapisuje do rozwiazania koncowego
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
