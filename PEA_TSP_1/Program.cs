using System;
using System.Collections.Generic;
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
            InstanceTestsDeviation(17, 39);
            //InstanceTestsDeviation(26, 937); //937
            InstanceTestsDeviation(42, 699); //699
            InstanceTestsDeviation(52, 7542);
            InstanceTestsDeviation(120, 6942);

            //InstanceTestsDeviation(48, 10628); //10628
            Console.Read();
        }

        //public static void InstanceTests(int cities)
        //{
        //    var graph = new Graph($"C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data{cities}.txt");

        //    Write(graph);

        //    IAlgorithm algorithm;
        //    //algorithm = new BruteForceAlgorithm(graph) { Name = $"BruteForce{cities}" };
        //    //ComputeAndSave(algorithm);

        //    //algorithm = new HeldKarpAlgorithm(graph, 0) { Name = $"HeldKarp{cities}" };
        //    //ComputeAndSave(algorithm);

        //    algorithm = new TabuSearchAlgorithm(graph, 1000, 50){Name = $"TabuSearch{cities}"};
        //    ComputeAndSave(algorithm);
        //}


        public static void InstanceTestsDeviation(int cities, int refWeight)
        {
            var graph = new Graph($"C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data{cities}.txt");

            //Write(graph);

            IAlgorithm algorithm;


            //algorithm = new GenethicAlgorithm(graph, 30,0.8f,0.2f,2000){Name = $"GeneticAlgorithm{cities}IND30"};
            //ComputeAndSaveDeviation(algorithm,refWeight,10);

            algorithm = new GenethicAlgorithm(graph, graph.NumberOfCities, 0.8f, 0.2f,2000){Name = $"GeneticAlgorithm{cities}INDN" };
            ComputeAndSaveDeviation(algorithm,refWeight,10);
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

        public static void Write(List<int> list)
        {
            foreach (var item in list)
            {
                Console.Write(item + " ");
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
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(algorithm.Name);
                    long time = MeasureTime(algorithm);
                    Console.WriteLine("Time");
                    Console.WriteLine(time);
                   writer.WriteLine(time);

                    Console.WriteLine("Path:");
                    Write(algorithm.Result.Path);
                    Console.WriteLine("Weight:");
                    Console.WriteLine(algorithm.Result.Weight);

                    Console.WriteLine();
               }
                writer.Close();
            }
        }

        public static void ComputeAndSaveDeviation(IAlgorithm algorithm, int refWeight, int iterations)
        {
            //using (StreamWriter writer = new StreamWriter(algorithm.Name + ".txt"))
            //{
                long timeSum = 0;
                var results = new List<int>();
                for (int i = 0; i < iterations; i++)
                {
                    Console.WriteLine(algorithm.Name);
                    long time = MeasureTime(algorithm);
                    timeSum += time;
                    Console.WriteLine("Time");
                    Console.WriteLine(time);
                    //writer.WriteLine(time);

                    Console.WriteLine("Path:");
                    Write(algorithm.Result.Path);
                    Console.WriteLine("Weight:");
                    Console.WriteLine(algorithm.Result.Weight);
                    Console.WriteLine();
                    results.Add(algorithm.Result.Weight);
                }
                //writer.WriteLine("Deviation");
                Console.WriteLine("Deviation");
                float sum = 0;
                foreach (var result in results)
                {
                    sum += (result - refWeight) * 100f / refWeight;
                }
                sum /= results.Count;
                //writer.WriteLine(sum + "%");
                Console.WriteLine(sum + "%");

                //writer.WriteLine("Average Time:");
                Console.WriteLine("Average Time:");
                //writer.WriteLine(timeSum / iterations);
                Console.WriteLine(timeSum / iterations);
                //writer.WriteLine();
                Console.WriteLine();
                //writer.Close();
            //}
        }
    }
}
