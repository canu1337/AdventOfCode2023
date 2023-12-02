namespace AdventOfCode2023;
internal class Day2 : DayBase
{
    public override void Init()
    {
    }

    private enum Color
    {
        Red,
        Green,
        Blue
    }

    private record Cube(int Number, Color Color);
    private record Game(int Id, List<List<Cube>> Sets);


    public override string Part1()
    {
        var games = ParseInput();

        var possibleGames = games.Where(g => g.Sets.All(s =>
        {
            if (s.Any(c => c.Color == Color.Red && c.Number > 12)) return false;
            if (s.Any(c => c.Color == Color.Green && c.Number > 13)) return false;
            if (s.Any(c => c.Color == Color.Blue && c.Number > 14)) return false;
            return true;
        })).ToList();
        var sum = possibleGames.Sum(g => g.Id);
        return sum.ToString();
    }

    public override string Part2()
    {
        var games = ParseInput();
        var sum = 0;
        foreach (var game in games)
        {
            var maxBlue = game.Sets.Select(s => s.Any(c => c.Color == Color.Blue) ? s.Where(c => c.Color == Color.Blue).Max(c => c.Number) : 0).Max();
            var maxRed = game.Sets.Select(s => s.Any(c => c.Color == Color.Red) ? s.Where(c => c.Color == Color.Red).Max(c => c.Number) : 0).Max();
            var maxGreen = game.Sets.Select(s => s.Any(c => c.Color == Color.Green) ? s.Where(c => c.Color == Color.Green).Max(c => c.Number) : 0).Max();
            var power = maxBlue * maxRed * maxGreen;
            sum += power;
        }
        return sum.ToString();
    }

    private static List<Game> ParseInput()
    {
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day2.txt"));
        var games = new List<Game>();
        foreach (var line in input)
        {
            var columnPos = line.IndexOf(':', StringComparison.Ordinal);
            var id = int.Parse(line[4..columnPos]);
            var sets = line[(columnPos + 1)..].Split(';');
            var parsedSets = new List<List<Cube>>();
            foreach (var set in sets)
            {
                var parsedCubes = new List<Cube>();
                foreach (var cube in set.Split(','))
                {
                    var words = cube.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s));
                    parsedCubes.Add(new Cube(int.Parse(words.ElementAt(0)), GetColor(words.ElementAt(1))));
                }
                parsedSets.Add(parsedCubes);
            }
            games.Add(new Game(id, parsedSets));
        }

        return games;
    }

    private static Color GetColor(string v)
    {
        return v switch
        {
            "red" => Color.Red,
            "green" => Color.Green,
            "blue" => Color.Blue,
            _ => throw new Exception("Unknown color"),
        };
    }
}
