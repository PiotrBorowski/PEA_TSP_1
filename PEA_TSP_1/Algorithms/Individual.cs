using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PEA_TSP_1.Algorithms
{
    public class Individual : AlgorithmResultShuffled
    {
        public Individual() : base()
        {

        }

        public Individual(Individual individual) : base(individual)
        {
        }

        public void Mutate(float probability)
        {
            Random random = new Random();
            if (random.NextDouble() > probability)
                return;

            var index = random.Next() % Path.Count;
            int index2;
            do
            {
                index2 = random.Next() % Path.Count;
            } while (index2 == index);

            var temp = Path[index2];
            Path[index2] = Path[index];
            Path[index] = temp;
        }

        public Tuple<Individual, Individual> CrossOver(Individual individual, float probability)
        {
            Random random = new Random();
            if (random.NextDouble() > probability)
                return new Tuple<Individual, Individual>(null, null);

            int index1 = Path.Count / 3;
            int index2 = Path.Count * 2 / 3;

            Dictionary<int, int> map = new Dictionary<int, int>();

            Individual ind1 = new Individual(this);
            //TODO: PRZETESTOWAC CZEMU NIE DZIALA DOBRZE BEZ BEZ KONSTRUKTORA
            Individual ind2 = new Individual(individual);

            //zamiana srodkowych czesci
            for (int i = index1; i < index2 + 1; i++)
            {
                int city1 = ind1.Path[i];
                int city2 = ind2.Path[i];

                ind1.Path[i] = city2;
                ind2.Path[i] = city1;

                if (city2 != city1)
                    map.Add(city2, city1);
            }

            for (int i = 0; i < ind1.Path.Count; i++)
            {
                if (i < index1 || i > index2)
                {
                    if (map.ContainsKey(Path[i]) && ind1.Path.Count(x => x == Path[i]) > 1)
                        ind1.Path[i] = map[ind1.Path[i]];

                    var individualValue = ind2.Path[i];
                    if (map.ContainsValue(individualValue) && ind2.Path.Count(x => x == individualValue) > 1)
                    {
                        ind2.Path[i] = map.FirstOrDefault(x => x.Value == individualValue).Key;
                    }
                }
            }

            return new Tuple<Individual, Individual>(ind1, ind2);
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
