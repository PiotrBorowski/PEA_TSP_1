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
        private int _stopCondition;
        private Graph _graph;

        public AlgorithmResult Result { get; private set; }
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
            Population population = new Population(_graph.NumberOfCities, _graph.NumberOfCities, _mutationRate, _crossOverRate);
            population.CalculateWeight(_graph);
            Result = SearchBestResult(population);           
        }

        private Individual SearchBestResult(Population population)
        {
            int stopCounter = 0;
            Individual bestIndividual = Individual.GenerateIndividual(_graph.NumberOfCities);
            for (int i = 0; i < _stopCondition; i++)
            {              
                population.CrossOver();
                population.Mutate();

                var currentBest = population.NextGeneration();

                population.CalculateWeight(_graph);

                if (currentBest.CalculateWeight(_graph) < bestIndividual.CalculateWeight(_graph))
                {
                    bestIndividual = currentBest;
                    stopCounter = 0;
                }
                else
                {
                    if (stopCounter == 100)
                        //_mutationRate = 0.5f;
                        return bestIndividual;
                    stopCounter++;
                }
            }

            return bestIndividual;
        }
    }

   
    public class Population
    {
        private List<Individual> _population;
        private readonly float _mutationRate;
        private readonly float _crossOverRate;
        private readonly int _count;

        public Individual BestIndividual { get; private set; }

        public Population(int sizeOfIndividual, int count, float mutationRate, float crossOverRate)
        {
            _mutationRate = mutationRate;
            _crossOverRate = crossOverRate;
            _population = new List<Individual>();
            _count = count;
            for (int i = 0; i < count; i++)
            {
                _population.Add(Individual.GenerateIndividual(sizeOfIndividual));
            }
        }

        public void CalculateWeight(Graph graph)
        {
            foreach (var individual in _population)
            {
                individual.CalculateWeight(graph);
            }
        }

        public void CrossOver()
        {
            //TODO: CROSSOVER STRATEGY
            int iterations = _population.Count;
            for (int i = 0; i < iterations - 1; ++i)
            {
                for (int j = i+1; j < iterations-1; j++)
                {
                    var individual = _population[i];
                    var individual2 = _population[j];

                    var childs = individual.CrossOver(individual2, _crossOverRate);
                    if (childs.Item1 != null || childs.Item2 != null)
                    {
                        _population.Add(childs.Item1);
                        _population.Add(childs.Item2);
                    }
                }

            }
        }

        public void Mutate()
        {
            foreach (var individual in _population)
            {
                individual.Mutate(_mutationRate);
            }
        }

        public Individual NextGeneration()
        {
            _population.OrderByDescending(x => x.Weight);
            _population = new List<Individual>(_population.Take(_count));

            return new Individual(_population[0]);
        }
    }
}
