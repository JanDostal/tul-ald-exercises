using System;
using System.Collections.ObjectModel;

namespace Exercise_10
{
    public class Program
    {
        static void Main(string[] args)
        {
            CitiesGraphDatabase citiesGraphDatabase = CitiesGraphDatabase.GetIntance();

            ICollection<string> navigationCommands = new Collection<string>();
            string navigationCommand;

            while ((navigationCommand = Console.ReadLine()) != null)
            {
                navigationCommands.Add(navigationCommand);
            }

            string commandOutput;

            foreach (var command in navigationCommands)
            {
                commandOutput = citiesGraphDatabase.ExecuteNavigationCommand(command);
                Console.Write(commandOutput);
            }
        }
    }
}