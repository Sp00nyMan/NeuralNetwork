using System;

namespace NeuralNetwork
{
    static class Randomizer
    {
        private static double last;
        private static readonly Random random;
        private const double TOLERANCE = 0.00000001;

        static Randomizer()
        {
            random = new Random(DateTime.Now.Millisecond);
            last = random.NextDouble();
        }

        public static double GetRandomDouble(double minValue, double maxValue)
        {
            double value;
            do
            {
                value = random.Next(Convert.ToInt32(minValue), Convert.ToInt32(maxValue));
                value += random.NextDouble();
                if (value > maxValue)
                    value -= 1;
            } while (Math.Abs(value - last) < TOLERANCE);
            last = value;
            return value;
        }
    }
}
