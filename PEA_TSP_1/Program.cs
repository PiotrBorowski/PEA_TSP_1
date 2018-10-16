using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using PEA_TSP_1.Algorithms;

namespace PEA_TSP_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph("C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data6.txt");
            Write(graph);
            IAlgorithm algorithm;

            algorithm = new BruteForceAlgorithm(graph) { Name = "BruteForce6" };
            ComputeAndSave(algorithm);

            algorithm = new HeldKarpAlgorithm(graph, 0){Name = "HeldKarp6"};
            ComputeAndSave(algorithm);

            graph = new Graph("C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data10.txt");

            algorithm = new BruteForceAlgorithm(graph){Name = "BruteForce10"};
            ComputeAndSave(algorithm);

            algorithm = new HeldKarpAlgorithm(graph, 0){Name = "HeldKarp10"};
            ComputeAndSave(algorithm);

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

        public static long MeasureTime(IAlgorithm algorithm)
        {
            var sw = new Stopwatch();

            sw.Start();
            algorithm.Invoke();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static void ComputeAndSave(IAlgorithm algorithm)
        {
            using (StreamWriter writer = new StreamWriter(algorithm.Name + ".txt"))
            {
                for (int i = 0; i < 20; i++)
                {
                    writer.WriteLine(MeasureTime(algorithm));
                }
                writer.Close();
            }
        }
    }
}
