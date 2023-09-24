using System;

namespace Exercise_01
{
    public class Program
    {
        static void Main(string[] args)
        {
            // do something
            var num = int.Parse(Console.ReadLine());

            for (int i = 0; i < num; i++)
            {
                Console.WriteLine("Hello world!");
            }
        }
    }
}