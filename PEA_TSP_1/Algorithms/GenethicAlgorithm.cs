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
            Population population = new Population(_graph.NumberOfCities, 10, _mutationRate, _crossOverRate);

            var result = SearchBestResult(population);           
        }

        private Individual SearchBestResult(Population population)
        {
            Individual bestIndividual = null;

            population.CalculateWeight(_graph);

            population.CrossOver();

            return bestIndividual;
        }
    }

   
    public class Population
    {
        private readonly List<Individual> _population;
        private readonly float _mutationRate;
        private readonly float _crossOverRate;

        public Population(int sizeOfIndividual, int count, float mutationRate, float crossOverRate)
        {
            _mutationRate = mutationRate;
            _crossOverRate = crossOverRate;
            _population = new List<Individual>();
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
            for (int i = 0; i < iterations - 1; i++)
            {
                var individual = _population[i];
                var individual2 = _population[i + 1];

                var childs = individual.CrossOver(individual2, _crossOverRate);
                if (childs.Item1 != null || childs.Item2 != null)
                {
                    _population.Add(childs.Item1);
                    _population.Add(childs.Item2);
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
    }
}
