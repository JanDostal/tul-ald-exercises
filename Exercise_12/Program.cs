using System;
using System.Text.RegularExpressions;

namespace Exercise_12
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Regex replacer = new Regex("[^a-z]+", RegexOptions.IgnoreCase);

            IList<string> lines = new List<string>();
            string line = null;

            while ((line = Console.ReadLine()) != null || lines.Count % 2 != 0)
            {
                line = line == null ? "" : replacer.Replace(line, "");
                lines.Add(line);
            }

            string firstString;
            string secondString;

            int[] firstStringCharsDiff;
            int[] secondStringCharsDiff;

            int firstStringNotMatchingCharsCount;
            int firstStringOneDiffCount;

            int secondStringNotMatchingCharsCount;
            int secondStringOneDiffCount;

            char currentChar;
            int firstStringOneDiffIndex = 0;
            int secondStringOneDiffIndex = 0;

            char[] firstStringUniqueChars;
            char[] secondStringUniqueChars;

            for (int index = 0; index < lines.Count; index += 2)
            {
                firstString = lines[index].ToLower();
                secondString = lines[index + 1].ToLower();

                if (string.IsNullOrEmpty(firstString) || string.IsNullOrEmpty(secondString)) continue;

                firstStringUniqueChars = firstString.Distinct().ToArray();
                firstStringCharsDiff = new int[firstStringUniqueChars.Length];

                for (int firstStringIndex = 0; firstStringIndex < firstStringCharsDiff.Length; firstStringIndex++)
                {
                    currentChar = firstStringUniqueChars[firstStringIndex];

                    firstStringCharsDiff[firstStringIndex] = Math.Abs(secondString.Where(s => s == currentChar).Count() -
                        firstString.Where(o => o == currentChar).Count());
                }

                secondStringUniqueChars = secondString.Distinct().ToArray();
                secondStringCharsDiff = new int[secondStringUniqueChars.Length];

                for (int secondStringIndex = 0; secondStringIndex < secondStringCharsDiff.Length; secondStringIndex++) 
                {
                    currentChar = secondStringUniqueChars[secondStringIndex];

                    secondStringCharsDiff[secondStringIndex] = Math.Abs(firstString.Where(s => s == currentChar).Count() -
                        secondString.Where(o => o == currentChar).Count());
                  
                }

                firstStringNotMatchingCharsCount = firstStringCharsDiff.Where(x => x != 0 && x != 1).Count();
                firstStringOneDiffCount = firstStringCharsDiff.Where(x => x == 1).Count();

                if (firstStringOneDiffCount == 1)               
                    firstStringOneDiffIndex = firstStringCharsDiff.Select((diff, ind) => new { diff, ind }).
                        Where(x => x.diff == 1).Select(x => x.ind).SingleOrDefault();

                secondStringNotMatchingCharsCount = secondStringCharsDiff.Where(x => x != 0 && x != 1).Count();
                secondStringOneDiffCount = secondStringCharsDiff.Where(x => x == 1).Count();

                if (secondStringOneDiffCount == 1)
                    secondStringOneDiffIndex = secondStringCharsDiff.Select((diff, ind) => new { diff, ind }).
                        Where(x => x.diff == 1).Select(x => x.ind).SingleOrDefault();


                if (firstStringCharsDiff.All(x => x == 0) && secondStringCharsDiff.All(x => x == 0)) Console.WriteLine("ANAGRAMS");
                else if (firstStringNotMatchingCharsCount == 0
                         && secondStringNotMatchingCharsCount == 0 &&
                         ((firstStringOneDiffCount == 1 ^ secondStringOneDiffCount == 1) || 
                         (firstStringOneDiffCount == 1 && secondStringOneDiffCount == 1 && 
                         firstStringUniqueChars[firstStringOneDiffIndex] == secondStringUniqueChars[secondStringOneDiffIndex]))) 
                            Console.WriteLine("NEAR ANAGRAMS");
                else Console.WriteLine("NOT ANAGRAMS");
            }
        }
    }
}

