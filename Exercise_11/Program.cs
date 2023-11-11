using System;

namespace Exercise_11
{
    public class Program
    {
        async static Task Main(string[] args)
        {
            var filename = Path.Combine(Directory.GetCurrentDirectory(), "..");
            //var inputReader = File.OpenRead(filename);

            var words = await InputUtils.ReadAll(Console.In);
            var wordDict = new Dictionary<string, int>();

            foreach (var word in words) 
            {
                if (wordDict.ContainsKey(word))
                {
                    wordDict[word]++;
                }
                else
                {
                    wordDict[word] = 1;
                }
            }

            Console.WriteLine($"Total words: {words.Length}");

            var topTenWords = wordDict.OrderByDescending(x => x.Value).Take(10).ToList();


            foreach (var word in topTenWords) 
            {
                var freq = (double) word.Value / words.Length;
                Console.WriteLine($" - {word.Key, -13} {freq, 2:P} {word.Value, 5}");
            }
        }
    }
}

