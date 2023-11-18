using System;

namespace Exercise_11
{
    public static class InputUtils
    {
        private static char[] WordsSeparators = new char[] { '\n', '\r', ' ' };

        public class InputTextData
        {
            public int TextCount { get; set; }

            public int DetectionOrder { get; }

            public InputTextData(int detectionOrder, int textCount) 
            {
                DetectionOrder = detectionOrder;
                TextCount = textCount;
            }
        }

        public static string[] ReadAllPhrases(string[] words) 
        {
            int phrasesLength = 0;

            if (words.Length == 0) 
            {
                phrasesLength = 0;
            }
            else 
            {
                phrasesLength = words.Length - 1;
            }

            var phrases = new string[phrasesLength];

            for (int phraseIndex = 0; phraseIndex < phrases.Length; phraseIndex++) 
            {
                phrases[phraseIndex] = $"{words[phraseIndex]} {words[phraseIndex + 1]}";
            }

            return phrases;
        }

        public static async Task<string[]> ReadAllWords(TextReader reader) 
        {
            var input = await reader.ReadToEndAsync();

            var words = input.ToLower().Split(WordsSeparators, StringSplitOptions.RemoveEmptyEntries);

            return words;
        }
    }
}
