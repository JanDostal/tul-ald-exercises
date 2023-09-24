using System;
using System.Text;

namespace Exercise_03
{
    public class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string inputString;
            IList<string> inputStrings = new List<string>();

            byte[] tempBytes;
            string utfString;

            while ((inputString = Console.ReadLine()) != null)
            {
                inputString = string.Concat(inputString.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c) && 
                                                                   !char.IsPunctuation(c) && !char.IsSeparator(c)));

                tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(inputString);
                utfString = Encoding.UTF8.GetString(tempBytes);
                utfString = utfString.ToLower();

                if (string.IsNullOrEmpty(utfString)) continue;

                inputStrings.Add(utfString);
            }

            char[] utfCharArray;
            int leftSideIndex, rightSideIndex;

            bool isPalindrom;

            foreach (var item in inputStrings.AsEnumerable())
            {
                isPalindrom = true;
                utfCharArray = item.ToCharArray();

                for (leftSideIndex = 0, rightSideIndex = item.Length - 1;
                    leftSideIndex < rightSideIndex && isPalindrom == true; leftSideIndex++, rightSideIndex--) 
                {
                    if (utfCharArray[leftSideIndex].CompareTo(utfCharArray[rightSideIndex]) != 0) isPalindrom = false;
                }

                Console.WriteLine(isPalindrom == true ? "ano" : "ne");
            }
        }
    }
}