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
        private readonly float _crossOverRate;
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
                population.CrossOver();
                population.Mutate();

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
                        case 30:
                            if (!consumption)
                            {
                                _mutationRate = 0.8f;
                                population.Count = population.Count *2/3;
                                consumption = true;
                            }
                            stopCounter = 0;
                            break;
                        case 40:
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
        private int _count;

        public Individual BestIndividual { get; private set; }

        public int Count
        {
            get { return _count; }
            set { _count = value; }
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
            _population = new List<Individual>(sorted.Take(Count));

            return new Individual(_population[0]);
        }
    }
}
