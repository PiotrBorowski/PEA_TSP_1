using System;
using System.Collections.Generic;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    class AlgorithmResult
    {
        public AlgorithmResult()
        {
            Path = new List<int>();
        }

        public int Weight { get; set; }
        public List<int> Path { get; set; }
    }
}
