using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Exercise_05
{
    public class Program
    {
        static void Main(string[] args)
        {
            GenericStack<string> sentencesStack = new GenericStack<string>();
            string sentence = null;

            while ((sentence = Console.ReadLine()) != null)
            {
                sentencesStack.Push(sentence);
            }

            StringBuilder text = new StringBuilder();
            Regex regexForEditingText = new Regex("(^\\w|[^\\w'\\-]+\\w)", RegexOptions.Multiline);
            string editedText;

            while (sentencesStack.Count != 0)
            {
                sentence = sentencesStack.Pop();
                text.Append(sentence).Append("\n");
            }

            text.Remove(text.Length - 1, 1);

            editedText = regexForEditingText.Replace(text.ToString(), (firstLetter) => firstLetter.Value.ToUpper());

            Console.Write(editedText);
        }
    }
}
