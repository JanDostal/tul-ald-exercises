using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Exercise_10
{
    public class CitiesGraphDatabase
    {
        private static CitiesGraphDatabase? instance;

        private IEnumerable<KeyValuePair<CommandName, CommandRecognizer>> NavigationCommands { get; }

        private ICollection<City> Cities { get; }

        private ICollection<CityConnectedWith> CitiesConnectedWith { get; }

        private enum CommandName
        {
            None,
            BestWayToDestination,
            ShortestWayToDestination
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

        public class City 
        {
            public int Id { get; }

            public string Name { get; }

            public City (int id, string name) 
            {
                Id = id;
                Name = name;
            }
        }

        public class CityConnectedWith 
        {
            public int SourceCityId { get; }
            
            public int DestinationCityId { get; }

            public int KilometersDistance { get; }

            public int MinutesDuration { get; }

            public CityConnectedWith(int sourceCityId, int destinationCityId, int kilometersDistance, int minutesDuration) 
            {
                SourceCityId = sourceCityId;
                DestinationCityId = destinationCityId;
                KilometersDistance = kilometersDistance;
                MinutesDuration = minutesDuration;
            }
        }

        private CitiesGraphDatabase() 
        {
            IDictionary<CommandName, CommandRecognizer> navigationCommands = new Dictionary<CommandName, CommandRecognizer>();

            navigationCommands.Add(CommandName.BestWayToDestination, new CommandRecognizer(matcherRegex: "^[\\w\\-]+ [\\w\\-]+ nejlepsi$", splitterRegex: "\\s"));
            navigationCommands.Add(CommandName.ShortestWayToDestination, new CommandRecognizer(matcherRegex: "^[\\w\\-]+ [\\w\\-]+ nejkratsi$", splitterRegex: "\\s"));

            NavigationCommands = navigationCommands.AsEnumerable();
            CitiesConnectedWith = new Collection<CityConnectedWith>();
            Cities = new Collection<City>();
        }

        private void SeedDatabase() 
        {
            Cities.Add(new City(1, "liberec"));
            Cities.Add(new City(2, "jablonec-nad-nisou"));
            Cities.Add(new City(3, "turnov"));
            Cities.Add(new City(4, "new-york"));
            Cities.Add(new City(5, "chrastava"));
            Cities.Add(new City(6, "ceska-lipa"));

            CitiesConnectedWith.Add(new CityConnectedWith(6, 5, 47, 40));
            CitiesConnectedWith.Add(new CityConnectedWith(6, 4, 30, 10));
            CitiesConnectedWith.Add(new CityConnectedWith(6, 3, 67, 52));

            CitiesConnectedWith.Add(new CityConnectedWith(5, 4, 14, 20));
            CitiesConnectedWith.Add(new CityConnectedWith(5, 1, 10, 12));
            CitiesConnectedWith.Add(new CityConnectedWith(5, 6, 47, 40));

            CitiesConnectedWith.Add(new CityConnectedWith(1, 4, 35, 24));
            CitiesConnectedWith.Add(new CityConnectedWith(1, 3, 26, 22));
            CitiesConnectedWith.Add(new CityConnectedWith(1, 2, 20, 20));
            CitiesConnectedWith.Add(new CityConnectedWith(1, 5, 10, 12));

            CitiesConnectedWith.Add(new CityConnectedWith(3, 4, 40, 15));
            CitiesConnectedWith.Add(new CityConnectedWith(3, 2, 24, 22));
            CitiesConnectedWith.Add(new CityConnectedWith(3, 1, 26, 22));
            CitiesConnectedWith.Add(new CityConnectedWith(3, 6, 67, 52));

            CitiesConnectedWith.Add(new CityConnectedWith(4, 2, 30, 30));
            CitiesConnectedWith.Add(new CityConnectedWith(4, 1, 35, 24));
            CitiesConnectedWith.Add(new CityConnectedWith(4, 3, 40, 15));
            CitiesConnectedWith.Add(new CityConnectedWith(4, 5, 14, 20));
            CitiesConnectedWith.Add(new CityConnectedWith(4, 6, 30, 10));

            CitiesConnectedWith.Add(new CityConnectedWith(2, 1, 20, 20));
            CitiesConnectedWith.Add(new CityConnectedWith(2, 4, 30, 30));
            CitiesConnectedWith.Add(new CityConnectedWith(2, 3, 24, 22));
        }

        public static CitiesGraphDatabase GetIntance() 
        {
            if (instance == null)
            {
                instance = new CitiesGraphDatabase();
                instance.SeedDatabase();
            }

            return instance;
        }

        private KeyValuePair<CommandName, CommandRecognizer> FindCommand(string commandSyntax)
        {
            CommandRecognizer foundCommandRecognizer;
            KeyValuePair<CommandName, CommandRecognizer> foundCommand = new KeyValuePair<CommandName, CommandRecognizer>(CommandName.None, null);

            foreach (var command in NavigationCommands)
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

        public string ExecuteNavigationCommand(string commandSyntax)
        {
            try
            {
                KeyValuePair<CommandName, CommandRecognizer> foundCommand = FindCommand(commandSyntax);

                if (foundCommand.Key == CommandName.None) throw new Exception("Zadany prikaz neodpovida dostupnym prikazum");

                CommandRecognizer foundCommandRecognizer = foundCommand.Value;
                CommandName foundCommandName = foundCommand.Key;
                string[] commandData = null;

                if (ReferenceEquals(foundCommandRecognizer.Splitter, null) == false) 
                    commandData = foundCommandRecognizer.Splitter.Split(commandSyntax);

                string commandTextOutput = null;

                switch (foundCommandName)
                {
                    case CommandName.BestWayToDestination:

                        commandTextOutput = GetBestWayToDestination(commandData[0], commandData[1]);
                        break;

                    case CommandName.ShortestWayToDestination:

                        commandTextOutput = GetShortestWayToDestination(commandData[0], commandData[1]);
                        break;
                }

                return commandTextOutput;
            }
            catch (Exception ex)
            {
                return $"{ex.Message}{Environment.NewLine}";
            }
        }

        private string GetBestWayToDestination(string sourceCityName, string destinationCityName) 
        {
            City? sourceCity = Cities.Where(x => x.Name.CompareTo(sourceCityName) == 0).SingleOrDefault();

            if (sourceCity == null) throw new Exception("zdrojove mesto neexistuje");

            City? destinationCity = Cities.Where(x => x.Name.CompareTo(destinationCityName) == 0).SingleOrDefault();

            if (sourceCity == null) throw new Exception("cilove mesto neexistuje");

            var sourceCityPaths = GetSourceCityPaths(sourceCity.Id);
            if (sourceCityPaths.Count() == 0) throw new Exception("Ze zadaneho zdrojoveho mesta nevede zadna cesta");

            ICollection<CityConnectedWith> pathToDestination;
            IList<ICollection<CityConnectedWith>> pathsToDestination = new List<ICollection<CityConnectedWith>>();
            foreach (var sourceCityPath in sourceCityPaths) 
            {
                pathToDestination = new Collection<CityConnectedWith>() { sourceCityPath };                
                FindPathToDestination(sourceCityPath.DestinationCityId, destinationCity.Id, 
                    pathToDestination, pathsToDestination);
            }

            if (pathsToDestination.Count() == 0) throw new Exception("Do cilove destinace neexistuje zadna cesta");

            var pathWithShortestTotalDuration = GetPathWithShortestTotalDuration(pathsToDestination);

            int pathTotalDistance = pathWithShortestTotalDuration.Sum(x => x.KilometersDistance);
            int pathTotalDuration = pathWithShortestTotalDuration.Sum(x => x.MinutesDuration);
            StringBuilder commandTextOutput =
                new StringBuilder($"({pathTotalDuration} min, {pathTotalDistance} km) ");

            commandTextOutput.Append(sourceCity.Name);

            foreach (var connection in pathWithShortestTotalDuration) 
            {
                var connectionDestinationCity = 
                    Cities.Where(city => city.Id == connection.DestinationCityId).Single();

                commandTextOutput.Append($" -> {connectionDestinationCity.Name}");
            }

            commandTextOutput.AppendLine();

            return commandTextOutput.ToString();
        }

        private ICollection<CityConnectedWith> GetPathWithShortestTotalDuration(IList<ICollection<CityConnectedWith>> pathsToDestination) 
        {
            IList<int> pathsTotalDurations = new List<int>();
            int totalDuration;

            foreach (var path in pathsToDestination)
            {
                totalDuration = path.Sum(x => x.MinutesDuration);
                pathsTotalDurations.Add(totalDuration);
            }

            int shortestTotalDuration = pathsTotalDurations.Min();

            var shortestTotalDurationsIndeces =
                pathsTotalDurations.Select((duration, index) => new {duration, index}).
                Where(x => x.duration == shortestTotalDuration).Select(x => x.index);
            var pathWithShortestTotalDuration =
                pathsToDestination.Where((path, index) => shortestTotalDurationsIndeces.Contains(index)).
                OrderBy(x => x.Count()).First();

            return pathWithShortestTotalDuration;
        }

        private ICollection<CityConnectedWith> GetPathWithShortestTotalDistance(IList<ICollection<CityConnectedWith>> pathsToDestination)
        {
            IList<int> pathsTotalDistances = new List<int>();
            int totalDistance;

            foreach (var path in pathsToDestination)
            {
                totalDistance = path.Sum(x => x.KilometersDistance);
                pathsTotalDistances.Add(totalDistance);
            }

            int shortestTotalDistance = pathsTotalDistances.Min();

            var shortestTotalDistancesIndeces =
                pathsTotalDistances.Select((distance, index) => new { distance, index }).
                Where(x => x.distance == shortestTotalDistance).Select(x => x.index);
            var pathWithShortestTotalDistance =
                pathsToDestination.Where((path, index) => shortestTotalDistancesIndeces.Contains(index)).
                OrderBy(x => x.Count()).First();

            return pathWithShortestTotalDistance;
        }

        private void FindPathToDestination(int pathCityId, int finalDestinationCityId, 
            ICollection<CityConnectedWith> pathToDestination, IList<ICollection<CityConnectedWith>> pathsToDestination) 
        {
            City? pathCity = Cities.Where(city => city.Id == pathCityId).SingleOrDefault();
            if (pathCity == null) throw new Exception("Aktualni mesto v ramci cesty neexistuje");

            if (pathCityId != finalDestinationCityId)
            {
                var pathCityPaths = GetSourceCityPaths(pathCity.Id);

                var pathCityValidPaths =
                    pathCityPaths.Where(path => pathToDestination.
                    Select(connection => connection.SourceCityId).Contains(path.DestinationCityId) == false);

                if (pathCityValidPaths.Count() != 0)
                {
                    ICollection<CityConnectedWith> updatedPathToDestination;
                    foreach (var pathCityPath in pathCityValidPaths)
                    {
                        updatedPathToDestination = new Collection<CityConnectedWith>(pathToDestination.ToList());
                        updatedPathToDestination.Add(pathCityPath);
                        FindPathToDestination(pathCityPath.DestinationCityId, finalDestinationCityId,
                            updatedPathToDestination, pathsToDestination);
                    }
                }
            }
            else pathsToDestination.Add(pathToDestination);
        }

        private IEnumerable<CityConnectedWith> GetSourceCityPaths(int sourceCityId) 
        {
            return CitiesConnectedWith.Where(x => x.SourceCityId == sourceCityId).AsEnumerable();
        }

        private string GetShortestWayToDestination(string sourceCityName, string destinationCityName) 
        {
            City? sourceCity = Cities.Where(x => x.Name.CompareTo(sourceCityName) == 0).SingleOrDefault();

            if (sourceCity == null) throw new Exception("zdrojove mesto neexistuje");

            City? destinationCity = Cities.Where(x => x.Name.CompareTo(destinationCityName) == 0).SingleOrDefault();

            if (sourceCity == null) throw new Exception("cilove mesto neexistuje");

            var sourceCityPaths = GetSourceCityPaths(sourceCity.Id);
            if (sourceCityPaths.Count() == 0) throw new Exception("Ze zadaneho zdrojoveho mesta nevede zadna cesta");

            ICollection<CityConnectedWith> pathToDestination;
            IList<ICollection<CityConnectedWith>> pathsToDestination = new List<ICollection<CityConnectedWith>>();
            foreach (var sourceCityPath in sourceCityPaths)
            {
                pathToDestination = new Collection<CityConnectedWith>() { sourceCityPath };
                FindPathToDestination(sourceCityPath.DestinationCityId, destinationCity.Id,
                    pathToDestination, pathsToDestination);
            }

            if (pathsToDestination.Count() == 0) throw new Exception("Do cilove destinace neexistuje zadna cesta");

            var pathWithShortestTotalDistance = GetPathWithShortestTotalDistance(pathsToDestination);

            int pathTotalDistance = pathWithShortestTotalDistance.Sum(x => x.KilometersDistance);
            int pathTotalDuration = pathWithShortestTotalDistance.Sum(x => x.MinutesDuration);
            StringBuilder commandTextOutput =
                new StringBuilder($"({pathTotalDuration} min, {pathTotalDistance} km) ");

            commandTextOutput.Append(sourceCity.Name);

            foreach (var connection in pathWithShortestTotalDistance)
            {
                var connectionDestinationCity =
                    Cities.Where(city => city.Id == connection.DestinationCityId).Single();

                commandTextOutput.Append($" -> {connectionDestinationCity.Name}");
            }

            commandTextOutput.AppendLine();

            return commandTextOutput.ToString();
        }

    }
}
