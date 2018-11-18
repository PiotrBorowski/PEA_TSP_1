using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    class BruteForceAlgorithm : IAlgorithm
    {
        private readonly Graph _graph;
        private AlgorithmResult _result;

        public AlgorithmResult Result => _result;
        public string Name { get; set; }

        public BruteForceAlgorithm(Graph graph)
        {
            _graph = graph;
            _result = new AlgorithmResult
            {
                Weight = Int32.MaxValue
            };
        }

        public void Invoke()
        {
            HeapPermutation(_graph.Vertices, _graph.NumberOfCities);
        }

        private void swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        private void HeapPermutation(int[] array, int size)
        {
            if (size == 1)
            {
                int totalWeight = 0;

                //pobieramy całą wagę drogi
                for (int i = 0; i < array.Length - 1; i++)
                {
                    totalWeight += _graph.GetWeight(array[i], array[i + 1]);
                }
                //waga pomiedzy ostatnim a pierwszym
                totalWeight += _graph.GetWeight(array[array.Length - 1], array[0]);

                //droga
                var resultPath = array.ToList();
                resultPath.Add(array[0]);

                if (_result.Weight > totalWeight)
                {
                    _result.Weight = totalWeight;
                    _result.Path = resultPath;
                }

                return;
            }

            for (int i = 0; i < size; i++)
            {
                HeapPermutation(array, size-1);

                if (size % 2 == 1)
                {
                    swap(array, 0, size - 1);
                }
                else
                {
                    swap(array, i, size - 1);
                }
            }
        }
    }
}
