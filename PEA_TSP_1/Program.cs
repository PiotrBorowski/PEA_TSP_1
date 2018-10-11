using System;
using System.Diagnostics;
using System.Linq;
using PEA_TSP_1.Algorithms;

namespace PEA_TSP_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph("C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data10.txt");
            Console.WriteLine(graph.GetWeight(0, 5));
            Write(graph);

            IAlgorithm algorithm = new BruteForceAlgorithm(graph);
            var sw = new Stopwatch();
            sw.Start();
            algorithm.Invoke();
            sw.Stop();

            Console.WriteLine("Brute Force");
            Write(algorithm);
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");

            sw.Reset();

            algorithm = new HeldKarpAlgorithm(graph, 0);
            sw.Start();
            algorithm.Invoke();
            sw.Stop();
            Console.WriteLine("Held Karp");
            Write(algorithm);
            Console.WriteLine($"Time: {sw.ElapsedMilliseconds}");


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

        public static void Write(IAlgorithm algorithm)
        {
            Console.WriteLine($"{algorithm.Result.Weight}");
            foreach (var item in algorithm.Result.Path)
            {
                Console.Write(item);
            }
            Console.WriteLine();
        }
    }
}
