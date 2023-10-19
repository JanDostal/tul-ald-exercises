using System;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Exercise_07
{
    public class Train
    {
        private static IEnumerable<KeyValuePair<CommandName, CommandRecognizer>> Commands { get; }

        private Wagon? FirstWagon { get; set; }

        private Wagon? LastWagon { get; set; }

        private StringBuilder WagonsTextRepresentation { get; } = new StringBuilder();

        private BigInteger TotalWeight { get; set; } = 0;

        private BigInteger TotalWagonsCount { get; set; } = 0;

        static Train()
        {
            IDictionary<CommandName, CommandRecognizer> commands = new Dictionary<CommandName, CommandRecognizer>();

            commands.Add(CommandName.PushAtEnd, new CommandRecognizer(matcherRegex: "^push \\w+ [1-9][0-9]*$", splitterRegex: "\\s"));
            commands.Add(CommandName.PopById, new CommandRecognizer(matcherRegex: "^pop id:\\w+$", splitterRegex: "\\s|:"));
            commands.Add(CommandName.PopByPosition, new CommandRecognizer(matcherRegex: "^pop pos:[1-9][0-9]*$", splitterRegex: "\\s|:"));
            commands.Add(CommandName.MoveToPosition, new CommandRecognizer(matcherRegex: "^move \\w+ [1-9][0-9]*$", splitterRegex: "\\s"));
            commands.Add(CommandName.GetTotalWeight, new CommandRecognizer(matcherRegex: "^total_weight$"));
            commands.Add(CommandName.GetAllWagonsList, new CommandRecognizer(matcherRegex: "^print$"));

            Commands = commands.AsEnumerable();
        }

        private enum CommandName
        {
            None,
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
                Splitter = ReferenceEquals(splitterRegex, null) == false ? new Regex(splitterRegex, RegexOptions.Compiled) : null;
            }
        }

        private class Wagon
        {
            public Wagon? Previous { get; set; }

            public Wagon? Next { get; set; }

            public WagonData Data { get; }

            public Wagon(string id, BigInteger weight)
            {
                Data = new WagonData(id, weight);
            }

            public class WagonData
            {
                public string Id { get; }

                public BigInteger Weight { get; }

                public WagonData(string id, BigInteger weight)
                {
                    Id = id;
                    Weight = weight;
                }

                public override string ToString()
                {
                    return $"{Id}:{Weight}";
                }
            }

            public override string ToString()
            {
                return Data.ToString();
            }
        }

        private static KeyValuePair<CommandName, CommandRecognizer> FindCommand(string commandSyntax) 
        {
            CommandRecognizer foundCommandRecognizer;
            KeyValuePair<CommandName, CommandRecognizer> foundCommand = new KeyValuePair<CommandName, CommandRecognizer>(CommandName.None, null);

            foreach (var command in Commands)
            {
                foundCommandRecognizer = command.Value;

                if (foundCommandRecognizer.Matcher.IsMatch(commandSyntax))
                {
                    foundCommand = command;
                    break;
                }
            }

            return foundCommand;
        }

        public string ExecuteCommand(string commandSyntax)
        {
            try 
            {
                KeyValuePair<CommandName, CommandRecognizer> foundCommand = FindCommand(commandSyntax);

                if (foundCommand.Key == CommandName.None) throw new Exception();

                CommandRecognizer foundCommandRecognizer = foundCommand.Value;
                CommandName foundCommandName = foundCommand.Key;
                string[] commandData = null;

                if (ReferenceEquals(foundCommandRecognizer.Splitter, null) == false) commandData = foundCommandRecognizer.Splitter.Split(commandSyntax);

                string commandTextOutput = null;

                switch (foundCommandName)
                {
                    case CommandName.PushAtEnd:

                        commandTextOutput = PushWagonAtEnd(commandData[1], BigInteger.Parse(commandData[2]));
                        break;

                    case CommandName.PopById:

                        commandTextOutput = PopWagonById(commandData[2]);
                        break;

                    case CommandName.PopByPosition:

                        commandTextOutput = PopWagonByPosition(BigInteger.Parse(commandData[2]));
                        break;

                    case CommandName.MoveToPosition:

                        commandTextOutput = MoveWagonToPositionById(commandData[1], BigInteger.Parse(commandData[2]));
                        break;

                    case CommandName.GetTotalWeight:

                        commandTextOutput = GetTrainTotalWeight();
                        break;

                    case CommandName.GetAllWagonsList:

                        commandTextOutput = GetAllWagonsList();
                        break;
                }

                return commandTextOutput;
            }
            catch (Exception) 
            {
                return $"error{Environment.NewLine}";
            }
        }

        private Wagon? GetWagon(string wagonId)
        {
            BigInteger foundWagonPosition;

            return GetWagon(wagonId, out foundWagonPosition);
        }

        private Wagon? GetWagon(string wagonId, out BigInteger foundWagonPosition)
        {
            Wagon? foundWagon = null;
            foundWagonPosition = BigInteger.Zero;

            if (FirstWagon != null)
            {
                bool isWagonFound = false;
                foundWagon = FirstWagon;

                do
                {
                    foundWagonPosition++;

                    if (foundWagon.Data.Id.CompareTo(wagonId) == 0) isWagonFound = true;
                    else foundWagon = foundWagon.Next;
                }
                while (foundWagon != null && isWagonFound == false);

                if (isWagonFound == false)
                {
                    foundWagonPosition = BigInteger.Zero;
                }
            }

            return foundWagon;
        }

        private Wagon GetWagon(BigInteger wagonPositionOrderFromStart, BigInteger wagonPositionOrderFromEnd) 
        {
            Wagon foundWagon;

            if (wagonPositionOrderFromStart <= wagonPositionOrderFromEnd)
            {
                foundWagon = FirstWagon;

                if (wagonPositionOrderFromStart != 1)
                {
                    for (BigInteger position = 2; position <= wagonPositionOrderFromStart; position++) foundWagon = foundWagon.Next;
                }
            }
            else
            {
                foundWagon = LastWagon;

                if (wagonPositionOrderFromStart != TotalWagonsCount)
                {
                    for (BigInteger position = 2; position <= wagonPositionOrderFromEnd; position++) foundWagon = foundWagon.Previous;
                }
            }

            return foundWagon;
        }

        private void SwapWagons(Wagon leftSideWagon, Wagon rightSideWagon) 
        {
            Wagon? leftSideWagonPreviousWagon = leftSideWagon.Previous;
            Wagon leftSideWagonNextWagon = leftSideWagon.Next;
            Wagon rightSideWagonPreviousWagon = rightSideWagon.Previous;
            Wagon? rightSideWagonNextWagon = rightSideWagon.Next;

            if (ReferenceEquals(leftSideWagonNextWagon, rightSideWagon))
            {
                rightSideWagon.Next = leftSideWagon;
                leftSideWagon.Previous = rightSideWagon;
            }
            else if (ReferenceEquals(leftSideWagonNextWagon, rightSideWagon) == false)
            {
                rightSideWagon.Next = leftSideWagonNextWagon;
                leftSideWagonNextWagon.Previous = rightSideWagon;
                leftSideWagon.Previous = rightSideWagonPreviousWagon;
                rightSideWagonPreviousWagon.Next = leftSideWagon;
            }

            if (ReferenceEquals(leftSideWagon, FirstWagon) && ReferenceEquals(rightSideWagon, LastWagon))
            {
                rightSideWagon.Previous = null;
                leftSideWagon.Next = null;
                FirstWagon = rightSideWagon;
                LastWagon = leftSideWagon;
            }
            else if (ReferenceEquals(leftSideWagon, FirstWagon) && ReferenceEquals(rightSideWagon, LastWagon) == false)
            {
                rightSideWagon.Previous = null;
                leftSideWagon.Next = rightSideWagonNextWagon;
                rightSideWagonNextWagon.Previous = leftSideWagon;
                FirstWagon = rightSideWagon;
            }
            else if (ReferenceEquals(leftSideWagon, FirstWagon) == false && ReferenceEquals(rightSideWagon, LastWagon))
            {
                leftSideWagon.Next = null;
                rightSideWagon.Previous = leftSideWagonPreviousWagon;
                leftSideWagonPreviousWagon.Next = rightSideWagon;
                LastWagon = leftSideWagon;
            }
            else if (ReferenceEquals(leftSideWagon, FirstWagon) == false && ReferenceEquals(rightSideWagon, LastWagon) == false)
            {
                leftSideWagon.Next = rightSideWagonNextWagon;
                rightSideWagon.Previous = leftSideWagonPreviousWagon;
                leftSideWagonPreviousWagon.Next = rightSideWagon;
                rightSideWagonNextWagon.Previous = leftSideWagon;
            }

            Regex leftSideWagonTextRepresentationMatcher = new Regex($"^{leftSideWagon.ToString()}{Environment.NewLine}", RegexOptions.Multiline);
            Regex leftSideWagonPlaceholderMatcher = new Regex($"^placeholder{Environment.NewLine}", RegexOptions.Multiline);
            Regex rightSideWagonTextRepresentationMatcher = new Regex($"^{rightSideWagon.ToString()}{Environment.NewLine}", RegexOptions.Multiline);
            
            string editedWagonsTextRepresentation = leftSideWagonTextRepresentationMatcher.Replace(WagonsTextRepresentation.ToString(), 
                $"placeholder{Environment.NewLine}", 1);
            editedWagonsTextRepresentation = rightSideWagonTextRepresentationMatcher.Replace(editedWagonsTextRepresentation,
                $"{leftSideWagon.ToString()}{Environment.NewLine}", 1);
            editedWagonsTextRepresentation = leftSideWagonPlaceholderMatcher.Replace(editedWagonsTextRepresentation,
                $"{rightSideWagon.ToString()}{Environment.NewLine}", 1);

            WagonsTextRepresentation.Clear();
            WagonsTextRepresentation.Append(editedWagonsTextRepresentation);
        }

        private string PushWagonAtEnd(string wagonId, BigInteger wagonWeight) 
        {
            if (GetWagon(wagonId) != null) throw new Exception();

            Wagon newWagon = new Wagon(wagonId, wagonWeight);

            if (ReferenceEquals(FirstWagon, null)) 
            {
                FirstWagon = newWagon;
            }
            else 
            {
                newWagon.Previous = LastWagon;
                LastWagon.Next = newWagon;
            }

            LastWagon = newWagon;

            WagonsTextRepresentation.AppendLine(newWagon.ToString());
            TotalWagonsCount++;
            TotalWeight += newWagon.Data.Weight;

            return $"ok{Environment.NewLine}";
        }

        private string PopWagonById(string wagonId) 
        {
            if (TotalWagonsCount == 0) throw new Exception();

            Wagon? foundWagon;

            foundWagon = GetWagon(wagonId);

            if (foundWagon == null)
            {
                throw new Exception();
            }
            else
            {
                if (TotalWagonsCount == 1)
                {
                    FirstWagon = null;
                    LastWagon = null;
                }
                else if (ReferenceEquals(foundWagon, FirstWagon) && TotalWagonsCount != 1)
                {
                    FirstWagon = FirstWagon.Next;
                    FirstWagon.Previous = null;
                }
                else if (ReferenceEquals(foundWagon, LastWagon) && TotalWagonsCount != 1)
                {
                    LastWagon = LastWagon.Previous;
                    LastWagon.Next = null;
                }
                else if (ReferenceEquals(foundWagon, FirstWagon) == false && 
                    ReferenceEquals(foundWagon, LastWagon) == false && TotalWagonsCount != 1)
                {
                    Wagon previousWagon = foundWagon.Previous;
                    Wagon nextWagon = foundWagon.Next;
                    previousWagon.Next = nextWagon;
                    nextWagon.Previous = previousWagon;
                }
            }

            Regex foundWagonTextRepresentationMatcher = new Regex($"^{foundWagon.ToString()}{Environment.NewLine}", RegexOptions.Multiline);
            string editedWagonsTextRepresentation = foundWagonTextRepresentationMatcher.Replace(WagonsTextRepresentation.ToString(), "", 1);
            WagonsTextRepresentation.Clear();
            WagonsTextRepresentation.Append(editedWagonsTextRepresentation);

            TotalWagonsCount--;
            TotalWeight -= foundWagon.Data.Weight;

            return $"ok{Environment.NewLine}";
        }

        private string PopWagonByPosition(BigInteger wagonPosition) 
        {
            if (wagonPosition < 1 || wagonPosition > TotalWagonsCount) throw new Exception();

            BigInteger wagonPositionOrderFromStart = wagonPosition;
            BigInteger wagonPositionOrderFromEnd = TotalWagonsCount - wagonPositionOrderFromStart + 1;

            Wagon foundWagon = null;

            if (TotalWagonsCount == 1)
            {
                foundWagon = FirstWagon;
                FirstWagon = null;
                LastWagon = null;
            }
            else if (wagonPositionOrderFromStart == 1 && TotalWagonsCount != 1)
            {
                foundWagon = FirstWagon;
                FirstWagon = FirstWagon.Next;
                FirstWagon.Previous = null;
            }
            else if (wagonPositionOrderFromStart == TotalWagonsCount && TotalWagonsCount != 1)
            {
                foundWagon = LastWagon;
                LastWagon = LastWagon.Previous;
                LastWagon.Next = null;
            }
            else if (wagonPositionOrderFromStart > 1 && wagonPositionOrderFromStart < TotalWagonsCount && TotalWagonsCount != 1)
            {
                foundWagon = GetWagon(wagonPositionOrderFromStart, wagonPositionOrderFromEnd);

                Wagon previousWagon = foundWagon.Previous;
                Wagon nextWagon = foundWagon.Next;
                previousWagon.Next = nextWagon;
                nextWagon.Previous = previousWagon;
            }

            Regex foundWagonTextRepresentationMatcher = new Regex($"^{foundWagon.ToString()}{Environment.NewLine}", RegexOptions.Multiline);
            string editedWagonsTextRepresentation = foundWagonTextRepresentationMatcher.Replace(WagonsTextRepresentation.ToString(), "", 1);
            WagonsTextRepresentation.Clear();
            WagonsTextRepresentation.Append(editedWagonsTextRepresentation);

            TotalWagonsCount--;
            TotalWeight -= foundWagon.Data.Weight;

            return $"ok{Environment.NewLine}";
        }

        private string MoveWagonToPositionById(string sourceWagonId, BigInteger destinationWagonPosition)  
        {
            Wagon? sourceWagon;
            BigInteger sourceWagonPositionOrderFromStart;

            if ((sourceWagon = GetWagon(sourceWagonId, out sourceWagonPositionOrderFromStart)) == null || 
                destinationWagonPosition < 1 || destinationWagonPosition > TotalWagonsCount) throw new Exception();
        
            BigInteger destinationWagonPositionOrderFromStart = destinationWagonPosition;
            BigInteger destinationWagonPositionOrderFromEnd = TotalWagonsCount - destinationWagonPositionOrderFromStart + 1;

            if (sourceWagonPositionOrderFromStart == destinationWagonPositionOrderFromStart) throw new Exception();

            Wagon leftSideWagon;
            Wagon rightSideWagon;

            if (sourceWagonPositionOrderFromStart < destinationWagonPositionOrderFromStart) 
            {
                leftSideWagon = sourceWagon;
                rightSideWagon = GetWagon(destinationWagonPositionOrderFromStart, destinationWagonPositionOrderFromEnd);
            }
            else 
            {
                leftSideWagon = GetWagon(destinationWagonPositionOrderFromStart, destinationWagonPositionOrderFromEnd);
                rightSideWagon = sourceWagon;
            }

            SwapWagons(leftSideWagon, rightSideWagon);

            return $"ok{Environment.NewLine}";
        }

        private string GetTrainTotalWeight() 
        {
            return $"{TotalWeight.ToString()}{Environment.NewLine}";
        }

        private string GetAllWagonsList()
        {
            if (TotalWagonsCount == 0) 
            {
                return $"empty{Environment.NewLine}";
            }
            else 
            {
                return WagonsTextRepresentation.ToString();
            }
        }
    }
}
