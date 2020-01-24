using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    class Program
    {
        static void Main()
        {
            //Создания сета тренировочных данных на основе изображений, названных в формате "цифраНаИзображении_номерИзображения.png" и лежащих по пути path
            var trainingData = GetTrainingData(@"C:\Users\mrsto\YandexDisk\NeuralNetwork\image demos", 1, 3, 5);
            
            List<int> layersInfo = new List<int>() { 10, 5, 12 }; //Информация о структуре сети
            
            //Тренировка
            NeuralNetwork neuralNetwork = NetworksAcademy.GetTrainedNetwork(trainingData.inputs, trainingData.outputs, 
                                                                            10000, 100, layersInfo);

            //Формирование тестового набора входных данных
            List<double> testSet = new List<double>(ImgToDoubleArrayConverter.GetBrightnessArray(@"C:\Users\mrsto\YandexDisk\NeuralNetwork\image demos\3_TEST.png"));

            //Получение выходных данных сети на основе тестовых входных данных
            neuralNetwork.Output(testSet);
            Console.Beep();
            Console.ReadKey();
            Console.Clear();
            List<double> outputs = neuralNetwork.OutputInPercentage();
            Console.WriteLine(neuralNetwork.Fitness);
            Console.WriteLine();
            foreach (var o in outputs)
            {
                Console.WriteLine(o);
            }

            Console.ReadKey();
        }

        static (List<List<double>> inputs, List<List<double>> outputs) GetTrainingData(string path, int minAvalibleDigit, int maxAvalibleDigit, uint dataCount)
        {
            List<List<double>> outs = new List<List<double>>();
            List<List<double>> inputs = new List<List<double>>();

            //Добавление тренировочных входных данных
            for (int j = minAvalibleDigit; j <= maxAvalibleDigit; j++)
                for (int i = 1; i <= dataCount; i++)
                {
                    string imgName = path + $"\\{j}_{i}.png";
                    inputs.Add(ImgToDoubleArrayConverter.GetBrightnessArray(imgName));
                }

            for(int j = minAvalibleDigit; j <= maxAvalibleDigit; j++)
            {
                List<double> buffer = new List<double>(Convert.ToInt32(maxAvalibleDigit - minAvalibleDigit + 1));
                for (int i = 0; i < buffer.Capacity; i++)
                {
                    if (i == j - 1)
                        buffer.Add(1);
                    else
                        buffer.Add(0);
                }
                for(int i = 0; i < dataCount; i++)
                    outs.Add(new List<double>(buffer.ToArray()));
            }

            return (inputs, outs);
        }
    }
}