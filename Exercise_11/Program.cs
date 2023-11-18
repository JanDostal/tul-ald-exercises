using System;
using System.Globalization;

namespace Exercise_11
{
    public class Program
    {
        async static Task Main(string[] args)
        {
            var words = await InputUtils.ReadAllWords(Console.In);
            var phrases = InputUtils.ReadAllPhrases(words);

            var wordDict = new Dictionary<string, InputUtils.InputTextData>();
            var phraseDict = new Dictionary<string, InputUtils.InputTextData>();

            int detectionOrder = 1;

            foreach (var word in words) 
            {
                if (wordDict.ContainsKey(word))
                {
                    wordDict[word].TextCount++;
                }
                else
                {
                    wordDict[word] = new InputUtils.InputTextData(detectionOrder, 1);
                    detectionOrder++;
                }
            }

            detectionOrder = 1;

            foreach (var phrase in phrases)
            {
                if (phraseDict.ContainsKey(phrase))
                {
                    phraseDict[phrase].TextCount++;
                }
                else
                {
                    phraseDict[phrase] = new InputUtils.InputTextData(detectionOrder, 1);
                    detectionOrder++;
                }
            }

            var topFifteenWords = wordDict.OrderByDescending(x => x.Value.TextCount).
                ThenBy(x => x.Value.DetectionOrder).Take(15).AsEnumerable();

            var topFifteenPhrases = phraseDict.OrderByDescending(x => x.Value.TextCount).
                ThenBy(x => x.Value.DetectionOrder).Take(15).AsEnumerable();

            Console.WriteLine("Word Frequency:");

            foreach (var word in topFifteenWords) 
            {
                var freq = (double) word.Value.TextCount / words.Length * 100;

                Console.WriteLine($" - {word.Key, -12} {freq.ToString("F2", CultureInfo.InvariantCulture)}% ({word.Value.TextCount})");
            }

            Console.WriteLine("Phrase Frequency:");

            foreach (var phrase in topFifteenPhrases)
            {
                var freq = (double) phrase.Value.TextCount / words.Length * 100;

                Console.WriteLine($" - {phrase.Key, -20} {freq.ToString("F2", CultureInfo.InvariantCulture)}% ({phrase.Value.TextCount})");
            }

        }
    }
}

