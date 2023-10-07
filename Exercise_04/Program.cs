using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace Exercise_04
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

            IList<BigInteger> palindromicNumberCandidates;
            IList<BigInteger> inputNumberCandidatePrefixes;

            NumberFormatInfo bigIntegerFormatter = new NumberFormatInfo();
            int inputNumberDigitsLength;

            int inputNumberMiddleElementPositionOrder;
            BigInteger inputNumberPrefix;
            char[] inputNumberCandidateStringPrefix;
            string palindromicNumberConstructedCandidate;


            BigInteger minDifferenceToClosestPalindromicNumber;
            BigInteger closestPalindromicNumber;

            foreach (var number in inputNumbers.AsEnumerable())
            {
                palindromicNumberCandidates = new List<BigInteger>();

                inputNumberDigitsLength = number.ToString("D", bigIntegerFormatter).Length;

                if (number < 9)
                {
                    Console.WriteLine(number + 1);
                    continue;
                }

                inputNumberMiddleElementPositionOrder = (inputNumberDigitsLength + 1) / 2;

                palindromicNumberCandidates.Add(BigInteger.Pow(10, inputNumberDigitsLength) + 1);

                inputNumberPrefix = BigInteger.Parse(number.ToString("D", bigIntegerFormatter).Substring(0, inputNumberMiddleElementPositionOrder));

                inputNumberCandidatePrefixes = new List<BigInteger>() { inputNumberPrefix, inputNumberPrefix + 1 };

                foreach (var prefix in inputNumberCandidatePrefixes.AsEnumerable())
                {
                    inputNumberCandidateStringPrefix = prefix.ToString("D", bigIntegerFormatter).ToCharArray();

                    if (inputNumberDigitsLength % 2 != 0) inputNumberCandidateStringPrefix = inputNumberCandidateStringPrefix.SkipLast(1).Cast<char>().ToArray();

                    Array.Reverse(inputNumberCandidateStringPrefix);

                    palindromicNumberConstructedCandidate = prefix + new string(inputNumberCandidateStringPrefix);

                    palindromicNumberCandidates.Add(BigInteger.Parse(palindromicNumberConstructedCandidate));
                }

                minDifferenceToClosestPalindromicNumber = BigInteger.Parse($"{decimal.MaxValue}");
                closestPalindromicNumber = number;

                foreach (var candidate in palindromicNumberCandidates.AsEnumerable())
                {
                    if (candidate > number && minDifferenceToClosestPalindromicNumber > candidate - number)
                    {
                        closestPalindromicNumber = candidate;
                        minDifferenceToClosestPalindromicNumber = candidate - number;
                    }
                }

                Console.WriteLine(closestPalindromicNumber);
            }
        }
    }
}
