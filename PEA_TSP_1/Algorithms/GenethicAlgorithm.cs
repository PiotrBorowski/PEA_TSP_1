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
                population.Mutate();

                population.CrossOver();
                var currentBest = population.NextGeneration(_graph);

                if (currentBest.CalculateWeight(_graph) < bestIndividual.CalculateWeight(_graph))
                {
                    bestIndividual = currentBest;
                    stopCounter = 0;
                }
                else
                {
                    switch (stopCounter)
                    {
                        case 70:
                            _mutationRate = 0.8f;
                            stopCounter = 0;
                            break;
                        case 100:
                            return bestIndividual;
                    }
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
            for (int i = 0; i < _population.Count; i++)
            {
                var ind = _population[i];
                ind.CalculateWeight(graph);
            }
        }

        public void CrossOver()
        {
            //TODO: CROSSOVER STRATEGY
            int iterations = _population.Count/2;
            for (int i = 0; i < iterations ; ++i)
            {
                for (int j = i+1; j < iterations; j++)
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
            for (int i = 0; i < _population.Count; i++)
            {
                var ind = _population[i];
                ind.Mutate(_mutationRate);
            }
        }

        public Individual NextGeneration(Graph graph)
        {
            var sorted =_population.OrderBy(x => x.CalculateWeight(graph));
            _population = new List<Individual>(sorted.Take(_count));

            return new Individual(_population[0]);
        }
    }
}
