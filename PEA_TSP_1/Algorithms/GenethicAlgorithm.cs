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

            var ind = new Individual{Path = new List<int>{1,2,3,4,5,6,7,8,9}};
            var ind2 = new Individual { Path = new List<int> { 9,8,7,6,5,4,3,2,1 } };
            ind.CrossOver(ref ind2);

            var result = SearchBestResult(population);
            
        }

        private Individual SearchBestResult(List<Individual> population)
        {
            Individual bestIndividual = null;

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

        public void CrossOver(ref Individual individual)
        {
            int index1 = Path.Count / 3;
            int index2 = Path.Count * 2 / 3;

            Dictionary<int,int> map = new Dictionary<int, int>();

            for (int i = index1; i < index2 - index1 + 1; i++)
            {
                int city1 = Path[i];
                int city2 = individual.Path[i];

                if(city2 != city1)
                    map.Add(city1, city2);

                Path[i] = city2;
                individual.Path[i] = city1;
            }

            for (int i = 0; i < Path.Count; i++)
            {
                if (i < index1 && i > index2)
                {
                    if (map.ContainsKey(Path[i]))
                        Path[i] = map[Path[i]];

                    if (map.ContainsValue(individual.Path[i]))
                    {
                        individual.Path[i] = map.FirstOrDefault(x => x.Value == Path[i]).Key;
                    }
                }         
            }
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
