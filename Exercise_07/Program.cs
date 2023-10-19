using System;

namespace Exercise_07
{
    public class Program
    {
        static void Main(string[] args)
        {
            Train train = new Train();

            ICollection<string> requestedCommands = new List<string>();
            string requestedCommand;

            while ((requestedCommand = Console.ReadLine()) != null)
            {
                requestedCommands.Add(requestedCommand);
            }

            string commandOutput;

            foreach (var command in requestedCommands)
            {
                commandOutput = train.ExecuteCommand(command);
                Console.Write(commandOutput);
            }
        }
    }
}
