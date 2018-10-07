using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PEA_TSP_1
{
    public class Graph
    {
        private int[,] _graph;
        private int _numberOfCities;

        public Graph(int numberOfCities)
        {
            _numberOfCities = numberOfCities;
            _graph = new int[numberOfCities - 1, numberOfCities - 1];
        }

        public Graph(string fileName)
        {
            try
            {  
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string text = sr.ReadLine();

                    _numberOfCities = Int32.Parse(text);
                    _graph = new int[_numberOfCities, _numberOfCities];

                    for (int i = 0; i < _numberOfCities; i++)
                    {
                        text = sr.ReadLine();
                        var numbersAsText = text.Split(new string[]{"", " "}, StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < _numberOfCities; j++)
                        {
                            _graph[i, j] = Int32.Parse(numbersAsText[j]);         
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public int GetWeight(int x, int y)
        {
            return _graph[x, y];
        }

        public void Write()
        {
            for (int i = 0; i < _numberOfCities; i++)
            {
                for (int j = 0; j < _numberOfCities; j++)
                {
                    Console.Write(_graph[i,j] + " ");
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
