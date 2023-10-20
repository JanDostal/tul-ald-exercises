using System;

namespace Exercise_08
{
    public class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();

            string line = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line)) line = Console.ReadLine();

            string[] inputNumbers = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var inputNumber in inputNumbers)
            {
                try
                {
                    tree.InsertIntoTree(int.Parse(inputNumber));
                }
                catch (FormatException)
                {
                }
            }

            Console.WriteLine("preorder");
            Console.Write(tree.GetTreeContentPreorder());

            Console.WriteLine("inorder");
            Console.Write(tree.GetTreeContentInorder());

            Console.WriteLine("postorder");
            Console.Write(tree.GetTreeContentPostorder());
        }
    }
}
