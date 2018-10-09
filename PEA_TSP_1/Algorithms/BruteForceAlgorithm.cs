﻿using System;
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

        public BruteForceAlgorithm(Graph graph)
        {
            _graph = graph;
        }

        public void Invoke()
        {
            heapPermutation(_graph.Vertices, _graph.NumberOfCities);
        }

        private void heapPermutation(int[] array, int size)
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

                if (_result == null)
                {
                    _result = new AlgorithmResult()
                    {
                        Weight = totalWeight,
                        Path = resultPath
                    };
                }
                else
                {
                    if (_result.Weight > totalWeight)
                    {
                        _result.Weight = totalWeight;
                        _result.Path = resultPath;
                    }
                }
            }

            for (int i = 0; i < size; i++)
            {
                heapPermutation(array, size-1);

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
