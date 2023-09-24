using System;

namespace Exercise_02
{
    public class Program
    {
        static void Main(string[] args)
        {
            // do something
            int num;
            IList<int> outputNumbers = new List<int>();

            while ((num = int.Parse(Console.ReadLine())) != 42)
            {
                outputNumbers.Add(num);
            }

            foreach (var number in outputNumbers)
            {
                Console.WriteLine(number);
            }
        }
    }
}
