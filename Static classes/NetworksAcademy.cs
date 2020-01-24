using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    static class NetworksAcademy
    {
        public static NeuralNetwork GetTrainedNetwork(List<List<double>> trainingInputs, List<List<double>> idealOutputs, int genetarionSize, int maximumGenerations, List<int> hiddenLayersInfo)
        {
            if (trainingInputs.Count != idealOutputs.Count)
                throw new RankException("Count of training inputs must be the same as count of ideal outputs");
            if (!CheckForEqality(trainingInputs))
                throw new RankException($"Some of training input's size isn't { trainingInputs[0].Count }");
            if (!CheckForEqality(idealOutputs))
                throw new RankException($"Some of ideal output's size isn't { idealOutputs[0].Count }");
            hiddenLayersInfo.Add(idealOutputs[0].Count);
         
            Generation generation = new Generation(hiddenLayersInfo, trainingInputs[0].Count, genetarionSize);
            double lastFitness = 0;
            List<double> fitnesses = new List<double>();

            for (int j = maximumGenerations; j > 0; j--)
            {
                int retVal = generation.RunTest(trainingInputs, idealOutputs);

                Console.Clear();
                Console.WriteLine($"Generation {maximumGenerations - j + 1} of {maximumGenerations}");

                if (retVal >= 0)
                    return generation.neurals[retVal];

                double bestFitness = generation.Best().Fitness;
                fitnesses.Add(bestFitness);

                {
                    double deltaFitness = -(lastFitness - bestFitness);

                    Console.WriteLine($"Progress = {deltaFitness}");
                    
                    if (deltaFitness < 0.01 && generation.maxDeviation <= 10)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        generation.maxDeviation *= 2;
                    }
                    else if (generation.maxDeviation != 0.2)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        generation.maxDeviation /= 2;
                    }
                    
                    Console.WriteLine($"Maximal deviation = {generation.maxDeviation}");
                    Console.ForegroundColor = ConsoleColor.White;

                    lastFitness = bestFitness;
                }

                Console.WriteLine($"Best = { bestFitness}");
                Console.WriteLine($"Guessed {Convert.ToInt32(15 * generation.Best().succRate)}/15");

                generation.Upgrade(true);
            }
            return generation.Best();
        }

        private static bool CheckForEqality(List<List<double>> arrayToCheck)
        {
            for (int i = 1; i < arrayToCheck.Count; i++)
                if (arrayToCheck[i].Count != arrayToCheck[i - 1].Count)
                    return false;
            return true;
        }
    }
}
