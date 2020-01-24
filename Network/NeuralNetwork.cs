using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetwork
{
    class NeuralNetwork
    {
        private readonly List<List<Node>> body;
        private readonly List<double> outputs = new List<double>();
        public double Fitness { get; set;}
        public readonly int inputsCount;
        public double succRate;
        public NeuralNetwork(List<int> layersInfo, int inputsNumber)
        {
            if (inputsNumber <= 0)
                throw new Exception("invalid inputs number");
            inputsCount = inputsNumber;
            body = new List<List<Node>>();

            for (int i = 0; i < layersInfo.Count; i++)
            {
                body.Add(new List<Node>(layersInfo[i]));
                for (int j = 0; j < layersInfo[i]; j++)
                {
                    body[i].Add(new Node(inputsNumber));
                }

                inputsNumber = layersInfo[i];
            }
        }

        public NeuralNetwork(NeuralNetwork network, double mutationRate, double maxDeviation)
        {
            inputsCount = network.inputsCount;
            body = new List<List<Node>>();

            for (int i = 0; i < network.body.Count; i++)
            {
                body.Add(new List<Node>());
                for (int j = 0; j < network.body[i].Count; j++)
                {
                    if (Randomizer.GetRandomDouble(0, 100) <= mutationRate)
                        body[i].Add(new Node(network.body[i][j], maxDeviation));
                    else
                        body[i].Add((Node) network.body[i][j].Clone()); // Могут быть проблемы, оригинал кода -  new Node(network.body[i][j]));
                }
            }
        }
        public List<double> Output(in List<double> inputs)
        {
            if (inputs.Count != inputsCount)
                throw new RankException();
            List<double> nextLayerInput = new List<double>(inputs.ToArray());
            foreach (var layer in body)
            {
                outputs.Clear();
                foreach (var node in layer)
                {
                    outputs.Add(node.Activate(nextLayerInput));
                }

                nextLayerInput.Clear();
                nextLayerInput.AddRange(outputs);
            }
            return outputs;
        }

        public bool CalcFitness(in List<double> realOutputs)
        {
            if (realOutputs.Count != outputs.Count)
                throw new RankException();
           
            var index = outputs.IndexOf(outputs.Max());
            var avg = outputs.Average();
            bool flag = true;
            
            for (int i = 0; i < outputs.Count; i++)
            {
                if (realOutputs[i] == 0)
                    continue;
                if (i != index)
                    flag = false;
                Fitness += outputs[i] - avg;
                break;
            }
            return flag;
        }

        public List<double> OutputInPercentage()
        {
            double sum = outputs.Sum();
            List<double> outs = new List<double>(outputs.Count);
            foreach (double output in outputs)
            {
                outs.Add(100 * output / sum);
            }
            return outs;
        }

        public void ResetDeviation()
        {
            Fitness = 0;
        }

    }
}