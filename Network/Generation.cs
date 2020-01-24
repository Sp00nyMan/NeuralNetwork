using System;
using System.Collections.Generic;


namespace NeuralNetwork
{
    class Generation
    {
        public int Size { get; }
        public List<NeuralNetwork> neurals;
        public double maxDeviation = 0.2;

        public Generation(List<int> layersInfo, int inputsNumber, int size)
        {
            neurals = new List<NeuralNetwork>(size);
            Size = size;
            for (int i = 0; i < size; i++)
            {
                try
                {
                    neurals.Add(new NeuralNetwork(layersInfo, inputsNumber));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public void Upgrade(bool giveWeakAChance = false)
        {
            const double mutationRate = 70; //in percents
            SortByDeviation();
            List<NeuralNetwork> parents = new List<NeuralNetwork>();
            if (giveWeakAChance)
            {
                double sum = 0;
                foreach (var nn in neurals)
                {
                    sum += neurals[Size - 1].Fitness - nn.Fitness;
                }

                for (int i = 0; i < Size; i++)
                {
                    double fitness = neurals[Size - 1].Fitness - neurals[i].Fitness;
                    int chance = (int) Math.Round(fitness / sum * 100);
                    while (chance-- > 0)
                    {
                        parents.Add(neurals[i]);
                    }
                }
            }
            else
                parents.Add(neurals[0]);

            if (parents.Count == 0)
                parents.Add(neurals[0]);
            Random rnd = new Random(DateTime.Now.Millisecond);
            neurals.Clear();
            for (int i = 0; i < Size; i++)
            {
                neurals.Add(new NeuralNetwork(parents[rnd.Next(parents.Count)], mutationRate, maxDeviation));
            }
        }

        public void SortByDeviation()
        {
            neurals.Sort(new NNComparer());
        }

        public int RunTest(List<List<double>> inputs, List<List<double>> idealOutputs)
        {
            int j = 0;
            foreach (var nn in neurals)
            {
                nn.ResetDeviation();

                bool found = true;
                double succCount = 0;
                for(int i = 0; i < inputs.Count; i++)
                {
                    nn.Output(inputs[i]);
                    if(!nn.CalcFitness(idealOutputs[i]))
                    {
                        found = false;
                    }
                    else
                    {
                        succCount++;
                    }
                }

                nn.succRate = succCount / inputs.Count;
                if (found)
                    return j;
                j++;
            }

            return -1;
        }

        public NeuralNetwork Best()
        {
            SortByDeviation();
            return neurals[0];
        }
    }

    internal class NNComparer : IComparer<NeuralNetwork>
    {
        public int Compare(NeuralNetwork x, NeuralNetwork y)
        {
            if (x?.succRate > y?.succRate)
                return -1;
            if (x?.succRate < y?.succRate)
                return 1;
            if (x?.Fitness > y?.Fitness)
                return -1;
            if (y?.Fitness > x?.Fitness)
                return 1;
            return 0;
        }
    }
}