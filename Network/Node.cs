using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    class Node : ICloneable
    {
        private readonly List<double> weights;
        public readonly int weightsCount;
        private double output;
        private readonly double correction;

        public Node(int previousLayerSize)
        {
            weights = new List<double>(previousLayerSize);
            for (uint i = 0; i < previousLayerSize; i++)
            {
                weights.Add(Randomizer.GetRandomDouble(-10, 10));
            }

            weightsCount = previousLayerSize;
            correction = Randomizer.GetRandomDouble(-10, 10);
        }

        public Node(Node node)
        {
            weights = new List<double>(node.weights);
            correction = node.correction;
            output = node.output;
            weightsCount = node.weightsCount;
        }

        public Node(Node node, double maxDeviation)
        {
            weights = new List<double>();
            foreach (double weight in node.weights)
            {
                weights.Add(weight + Randomizer.GetRandomDouble(-maxDeviation, maxDeviation));
            }

            correction = node.correction + Randomizer.GetRandomDouble(-maxDeviation, maxDeviation);
            weightsCount = node.weightsCount;
        }

        private static double activationFunction(double rawOutput) //Sigmoid
        {
            var value = 1 / (1 + Math.Exp(-rawOutput));
            return value;
        }

        public double Activate(List<double> input)
        {
            if (input.Count != weights.Count)
                throw new Exception("Input size error");
            output = 0;
            for (int i = 0; i < weights.Count; i++)
            {
                output += weights[i] * input[i];
            }

            output = activationFunction(output);
            return output;
        }

        public object Clone() => new Node(this);
    }
}