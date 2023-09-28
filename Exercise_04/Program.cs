using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace Exercise_03
{
    public class Program
    {
        static void Main(string[] args)
        {
            BigInteger inputNumber = 0;
            IList<BigInteger> inputNumbers = new List<BigInteger>();

            while (inputNumber != -1)
            {
                try
                {
                    inputNumber = BigInteger.Parse(Console.ReadLine());

                    if (inputNumber < 0) continue;

                    inputNumbers.Add(inputNumber);
                }
                catch (FormatException)
                {
                    inputNumber = 0;
                }
                catch (ArgumentNullException)
                {
                    inputNumber = 0;
                }
            }

            IList<BigInteger> candidates;

            NumberFormatInfo bigIntegerFormatter = new NumberFormatInfo();
            int inputNumberDigitsLength;

            int middleElementIndex;

            foreach (var num in inputNumbers.AsEnumerable())
            {
                candidates = new List<BigInteger>();

                inputNumberDigitsLength = num.ToString("D", bigIntegerFormatter).Length;

                middleElementIndex = 
            }
        }
    }
}
