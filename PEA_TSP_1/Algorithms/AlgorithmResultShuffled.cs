using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    public class AlgorithmResultShuffled : AlgorithmResult
    {
        private static Random rng = new Random();

        public AlgorithmResultShuffled() : base()
        {

        }

        public AlgorithmResultShuffled(AlgorithmResultShuffled alg)
        {
            Path = new List<int>(alg.Path);
            Weight = alg.Weight;
        }

        public int CalculateWeight(Graph _graph)
        {
            int weight = 0;
            for (int i = 0; i < _graph.NumberOfCities - 1; i++)
            {
                weight += _graph.GetWeight(Path[i], Path[i + 1]);
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

        public static AlgorithmResultShuffled GenerateResult(int size)
        {
            var init = new AlgorithmResultShuffled()
            {
                Path = Enumerable.Range(0, size).ToList()
            };
            init.Shuffle();

            return init;
        }
    }
}
