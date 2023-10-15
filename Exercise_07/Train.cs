using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exercise_07
{
    public class Train
    {
        private static Dictionary<CommandName, CommandRecognizer> CommandsRecognizers { get; } = new Dictionary<CommandName, CommandRecognizer>();

        private Wagon? FirstWagon { get; set; }

        private Wagon? LastWagon { get; set; }

        private StringBuilder WagonsTextRepresentation { get; } = new StringBuilder();

        private BigInteger TotalWeight { get; set; } = 0;

        private BigInteger TotalWagonsCount { get; set; } = 0;

        static Train()
        {
            CommandsRecognizers.Add(CommandName.PushAtEnd, new CommandRecognizer(matcherRegex: "^push \\w+ [1-9][0-9]*$", splitterRegex: "\\s"));

            CommandsRecognizers.Add(CommandName.PopById, new CommandRecognizer(matcherRegex: "^pop id:\\w+$", splitterRegex: "\\s|:"));

            CommandsRecognizers.Add(CommandName.PopByPosition, new CommandRecognizer(matcherRegex: "^pop pos:[1-9][0-9]*$", splitterRegex: "\\s|:"));

            CommandsRecognizers.Add(CommandName.MoveToPosition, new CommandRecognizer(matcherRegex: "^move \\w+ [1-9][0-9]*$", "\\s"));


            CommandsRecognizers.Add(CommandName.GetTotalWeight, new CommandRecognizer(matcherRegex: "^total_weight$"));
            CommandsRecognizers.Add(CommandName.GetAllWagonsList, new CommandRecognizer(matcherRegex: "^print$"));

            CommandsRecognizers.Add(CommandName.GetAllWagonsList, new CommandRecognizer("das"));
        }

        private enum CommandName
        {
            PushAtEnd,
            PopById,
            PopByPosition,
            MoveToPosition,
            GetTotalWeight,
            GetAllWagonsList,
        }

        private class CommandRecognizer 
        {
            public Regex Matcher { get; }

            public Regex? Splitter { get; }

            public CommandRecognizer(string matcherRegex, string? splitterRegex = null) 
            {
                Matcher = new Regex(matcherRegex, RegexOptions.Compiled);
                Splitter = splitterRegex != null ? new Regex(splitterRegex, RegexOptions.Compiled) : null;
            }
        }

        private class WagonData
        {
            public string Id { get; }

            public BigInteger Weight { get; }

            public WagonData(string id, BigInteger weight)
            {
                Id = id;
                Weight = weight;
            }
        }

        private class Wagon
        {
            public Wagon Previous { get; set; }

            public Wagon Next { get; set; }

            public WagonData Data { get; }

            public Wagon(WagonData data, Wagon next, Wagon previous)
            {
                Data = data;
                Next = next;
                Previous = previous;
            }
        }

        public string ExecuteCommand(string command)
        {
            return "";
        }
    }
}
