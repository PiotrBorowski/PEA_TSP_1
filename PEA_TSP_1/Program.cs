using System;

namespace PEA_TSP_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph("C:\\Users\\Piotr Borowski\\source\\repos\\PEA_TSP_1\\PEA_TSP_1\\data6.txt");

            Console.WriteLine(graph.GetWeight(1, 0));
            graph.Write();
            Console.Read();
        }
    }
}
