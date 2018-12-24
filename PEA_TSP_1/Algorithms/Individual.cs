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
                    var key = ind1.Path[i];
                    ind1.Path[i] = GetPMXValue(map, key);

                    var value = ind2.Path[i];
                    ind2.Path[i] = GetPMXKey(map, value);               
                }
            }

            //for (int i = 0; i < ind1.Path.Count; i++)
            //{
            //    if (ind1.Path.Count(x => x == i) > 1)
            //        throw new Exception();
            //    if (ind2.Path.Count(x => x == i) > 1)
            //        throw new Exception();
            //}

            return new Tuple<Individual, Individual>(ind1, ind2);
        }

        private int GetPMXValue(Dictionary<int,int> map, int key)
        {
            if (map.ContainsKey(key))
            {
                key = GetPMXValue(map, map[key]);

            }//&& ind1.Path.Count(x => x == ind1.Path[i]) > 1)

            return key;
           
        }

        private int GetPMXKey(Dictionary<int, int> map, int value)
        {
            if (map.ContainsValue(value))
            {
                value = GetPMXKey(map, map.FirstOrDefault(x => x.Value == value).Key);

            }//&& ind2.Path.Count(x => x == individualValue) > 1)

            return value;
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
