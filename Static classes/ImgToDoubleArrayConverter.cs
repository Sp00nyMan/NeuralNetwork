using System;
using System.Collections.Generic;
using System.Drawing;

namespace NeuralNetwork
{
    static class ImgToDoubleArrayConverter
    {
        public static List<double> GetBrightnessArray(string path, uint step = 1)
        {
            Bitmap img;
            try
            {
                img = new Bitmap(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            List<double> result = new List<double>();
            for (uint i = 0; i < img.Width; i += step)
            {
                for (uint j = 0; j < img.Height; j += step)
                {
                    result.Add(1 - img.GetPixel(Convert.ToInt32(i), Convert.ToInt32(j)).GetBrightness()); //1 - чёрный цвет, 0 - белый
                }
            }

            return result;
        }
    }
}
