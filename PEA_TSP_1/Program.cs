using System;
using System.Linq;
using PEA_TSP_1.Algorithms;

namespace PEA_TSP_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph("C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data10.txt");

            Console.WriteLine(graph.GetWeight(1, 0));
            Write(graph);

            IAlgorithm algorithm = new BruteForceAlgorithm(graph);
            algorithm.Invoke();
            Console.WriteLine($"Brute Force: {algorithm.Result.Weight}");
            foreach (var item in algorithm.Result.Path)
            {
                Console.Write(item);
            }
            Console.WriteLine();

            algorithm = new HeldKarpAlgorithm(graph);
            algorithm.Invoke();
            Console.WriteLine($"HeldKarp: {algorithm.Result.Weight}");

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
