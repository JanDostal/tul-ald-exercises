using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Exercise_06
{
    public class Program
    {
        static void Main(string[] args)
        {
            GenericQueue<string> sentencesQueue = new GenericQueue<string>();
            string sentence = null;

            while ((sentence = Console.ReadLine()) != null)
            {
                sentencesQueue.Push(sentence);
            }

            StringBuilder text = new StringBuilder();
            Regex regexForEditingText = new Regex("(^\\w|[^\\w'\\-]+\\w)", RegexOptions.Multiline);
            string editedText;

            while (sentencesQueue.Count != 0)
            {
                sentence = sentencesQueue.Pop();
                text.Append(sentence).Append("\n");
            }

            text.Remove(text.Length - 1, 1);

            editedText = regexForEditingText.Replace(text.ToString(), (firstLetter) => firstLetter.Value.ToUpper());

            Console.Write(editedText);
        }
    }
}
