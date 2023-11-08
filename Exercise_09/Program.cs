using System;

namespace Exercise_09
{
    public class Program
    {
        static void Main(string[] args)
        {
            AVLTree avlTree = new AVLTree();

            string? line;
            string[] lineInputNumbers;
            int inputNumber;

            bool endLoading = false;

            while (endLoading == false) 
            {
                line = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line)) line = Console.ReadLine();

                lineInputNumbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                for (int colIndex = 0; colIndex < lineInputNumbers.Length && endLoading == false; colIndex++)
                {
                    try
                    {
                        inputNumber = int.Parse(lineInputNumbers[colIndex]);

                        avlTree.InsertIntoTree(inputNumber);

                        if (inputNumber == -1) endLoading = true;
                    }
                    catch (FormatException)
                    {
                    }
                }
            }

            Console.WriteLine("PREORDER");
            Console.Write(avlTree.GetTreeContentPreorder());

            Console.WriteLine("INORDER");
            Console.Write(avlTree.GetTreeContentInorder());

            Console.WriteLine("POSTORDER");
            Console.Write(avlTree.GetTreeContentPostorder());
        }
    }
}
