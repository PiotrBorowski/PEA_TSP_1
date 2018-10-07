using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    class BruteForceAlgorithm
    {
        private readonly Graph _graph;
        private List<AlgorithmResult> _results;

        public AlgorithmResult Result => _results.OrderBy(x => x.Weight).FirstOrDefault();

        public BruteForceAlgorithm(Graph graph)
        {
            _graph = graph;
            _results = new List<AlgorithmResult>();
        }

        public void Invoke()
        {
            //TODO: getting graph verticies
            int[] array = {1, 2, 3};
            heapPermutation(array, array.Length, ref _results);
        }

        private void heapPermutation(int[] array, int size, ref List<AlgorithmResult> results)
        {
            if (size == 1)
            {
                int totalWeight = 0;

                for (int i = 0; i < array.Length - 1; i++)
                {
                    totalWeight += _graph.GetWeight(array[i], array[i + 1]);
                }
                //weight of path between last vertex and source
                totalWeight += _graph.GetWeight(array[array.Length - 1], array[0]);

                var resultPath = array.ToList();
                resultPath.Add(array[0]);

                results.Add(new AlgorithmResult()
                {
                    Weight = totalWeight,
                    Path = resultPath
                });
            }

            for (int i = 0; i < size; i++)
            {
                heapPermutation(array, size-1, ref results);

                if (size % 2 == 1)
                {
                    int temp = array[0];
                    array[0] = array[size - 1];
                    array[size - 1] = temp;
                }
                else
                {
                    int temp = array[i];
                    array[i] = array[size - 1];
                    array[size - 1] = temp;
                }
            }
        }
    }
}
