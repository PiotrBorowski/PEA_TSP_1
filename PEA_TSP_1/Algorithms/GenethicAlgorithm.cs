using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    public class GenethicAlgorithm : IAlgorithm
    {
        private float _mutationRate;
        private float _crossOverRate;
        private int _stopCondition;
        private Graph _graph;

        public AlgorithmResult Result { get; }
        public string Name { get; set; }

        public GenethicAlgorithm(Graph graph, float mutationRate, float crossOverRate, int stopCondition)
        {
            _mutationRate = mutationRate;
            _crossOverRate = crossOverRate;
            _stopCondition = stopCondition;
            _graph = graph;
        }

        public void Invoke()
        {
            List<Individual> population = CreatePopulation(50);

            var result = SearchBestResult(population);
            
        }

        private Individual SearchBestResult(List<Individual> population)
        {
            Individual bestIndividual;

            foreach (var individual in population)
            {
                individual.CalculateWeight(_graph);
            }

            return bestIndividual;
        }

        private List<Individual> CreatePopulation(int size)
        {
            List<Individual> population = new List<Individual>();
            for (int i = 0; i < size; i++)
            {
                population.Add(Individual.GenerateIndividual(_graph.NumberOfCities));
            }

            return population;
        }
    }

    public class Individual : AlgorithmResultShuffled
    {
        public void Mutate(float probability)
        {
            Random random = new Random();
            if (random.NextDouble() < probability)
            {
                int index = random.Next()%Path.Count;
                int index2;
                do
                {
                    index2 = random.Next() % Path.Count;
                } while (index2 == index);

                int temp = Path[index2];
                Path[index2] = Path[index];
                Path[index] = temp;
            }
        }

        public void CrossOver(Individual individual)
        {
            throw new NotImplementedException();
        }

        public static Individual GenerateIndividual(int size)
        {
            var init = new Individual
            {
                Path = Enumerable.Range(0, size).ToList()
            };

            init.Shuffle();
            return init;
        }
    }
}
