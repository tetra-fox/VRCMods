using System;

namespace VXP
{
    public static class VocalProcessor
    {
        internal static Tuple<int, float> Process()
        {
            Random random = new Random();
            int expression = random.Next(0, 9);
            float confidence = (float) random.NextDouble();

            return Tuple.Create(expression, confidence);
        }
    }
}