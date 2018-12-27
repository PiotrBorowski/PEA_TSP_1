using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace PEA_TSP_1.Algorithms
{
    public class GenethicAlgorithm : IAlgorithm
    {
        private float _mutationRate;
        private float _crossOverRate;
        private readonly int _stopCondition;
        private readonly int _countOfIndividuals;
        private readonly Graph _graph;

        public AlgorithmResult Result { get; private set; }
        public string Name { get; set; }

        public GenethicAlgorithm(Graph graph, int countOfIndividuals, float mutationRate, float crossOverRate, int stopCondition)
        {
            _mutationRate = mutationRate;
            _crossOverRate = crossOverRate;
            _stopCondition = stopCondition;
            _countOfIndividuals = countOfIndividuals;
            _graph = graph;
        }

        public void Invoke()
        {
            Population population = new Population(_graph.NumberOfCities, _countOfIndividuals, _mutationRate, _crossOverRate);
            population.CalculateWeight(_graph);
            Result = SearchBestResult(population);           
        }

        private Individual SearchBestResult(Population population)
        {
            int stopCounter = 0;
            bool consumption = false;
            Individual bestIndividual = Individual.GenerateIndividual(_graph.NumberOfCities);
            for (int i = 0; i < _stopCondition; i++)
            {

                var currentBest = population.NextGeneration(_graph);

                if (currentBest.CalculateWeight(_graph) < bestIndividual.CalculateWeight(_graph))
                {
                    bestIndividual = currentBest;
                    stopCounter = 0;
                }
                else
                {
                    stopCounter++;

                    if (!consumption && stopCounter == _stopCondition / 50)
                    {
                        population.MutationRate = 0.9f;
                        population.CrossOverRate = 0.05f;
                        consumption = true;
                        //population.Count /= 2;
                        stopCounter = 0;
                    }
                    else
                    if (consumption && stopCounter == _stopCondition / 10)
                    {
                        return bestIndividual;
                    }

                }

                population.CrossOver();
                population.Mutate();
            }

            return bestIndividual;
        }
    }

   
    public class Population
    {
        private List<Individual> _population;
        private float _mutationRate;
        private float _crossOverRate;
        private int _count;

        public Individual BestIndividual { get; private set; }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public float MutationRate
        {
            get => _mutationRate;
            set => _mutationRate = value;
        }

        public float CrossOverRate
        {
            get => _crossOverRate;
            set => _crossOverRate = value;
        }

        public Population(int sizeOfIndividual, int count, float mutationRate, float crossOverRate)
        {
            _mutationRate = mutationRate;
            _crossOverRate = crossOverRate;
            _population = new List<Individual>();
            Count = count;
            for (int i = 0; i < count; i++)
            {
                _population.Add(Individual.GenerateIndividual(sizeOfIndividual));
            }
        }

        public void CalculateWeight(Graph graph)
        {
            for (int i = 0; i < _population.Count; i++)
            {
                var ind = _population[i];
                ind.CalculateWeight(graph);
            }
        }

        public void CrossOver()
        {
            Random rand = new Random();
            int iterations = _population.Count;
            for (int i = 0; i < iterations ; ++i)
            {
                var individual = _population[i];

                int index;
                do
                {
                    index = rand.Next() % _population.Count;
                } while (index == i);

                var individual2 = _population[index];

                var childs = individual.CrossOver(individual2, CrossOverRate);
                if (childs.Item1 != null || childs.Item2 != null)
                {
                    _population.Add(childs.Item1);
                    _population.Add(childs.Item2);
                }
            }
        }

        public void Mutate()
        {
            int iterations = _population.Count;
            for (int i = 0; i < iterations; i++)
            {
                var ind = _population[i];
                ind = ind.Mutate(MutationRate);

                if(ind != null)
                    _population.Add(ind);
            }
        }

        public Individual NextGeneration(Graph graph)
        {
            var sorted =_population.OrderBy(x => x.CalculateWeight(graph));
            _population = new List<Individual>(sorted.Take(Count));

            return new Individual(_population[0]);
        }
    }
}
