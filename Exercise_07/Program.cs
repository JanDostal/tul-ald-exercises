using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Exercise_07
{
    public enum Ahoj 
    {
        DEBIL,
        TADY
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string[] a = Regex.Split("ahoj pos:123", "\\s|:", RegexOptions.Compiled);

            for (int i = 0; i < a.Length; i++) 
            {
                Console.WriteLine(a[i]);
            }
        }
    }
}
