using System;
using System.Linq;
using PEA_TSP_1.Algorithms;

namespace PEA_TSP_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph("C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data6.txt");

            Console.WriteLine(graph.GetWeight(1, 0));
            Write(graph);

            var brute = new BruteForceAlgorithm(graph);
            brute.Invoke();
            Console.WriteLine(brute.Result.Weight);
            foreach (var item in brute.Result.Path)
            {
                Console.Write(item);
            }


            Console.Read();
        }

        public static void Write(Graph graph)
        {
            for (int i = 0; i < graph.NumberOfCities; i++)
            {
                for (int j = 0; j < graph.NumberOfCities; j++)
                {
                    Console.Write(graph.GetWeight(i, j) + " ");
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
