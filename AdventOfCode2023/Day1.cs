namespace AdventOfCode2023;
internal class Day1 : DayBase
{
    private string[] input;
    private readonly List<(string numberAsString, int number, string digitAsString)> numbersAsStrings;
    public override void Init()
    {
    }

    public override string Part1()
    {
        var lockObject = new object();
        input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day1.txt"));
        var sum = 0;
        Parallel.ForEach(input, line =>
        {
            var res = int.Parse(line.First(char.IsDigit).ToString() + line.Last(char.IsDigit).ToString());
            lock (lockObject)
            {
                sum += res;
            }
        });
        return sum.ToString();
    }


    public override string Part2()
    {
        input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day1.txt"));
        var numbersAsStrings = new List<(string numberAsString, int number, string digitAsString)>
        {
            new("one", 1, "1"),
            new("two", 2, "2"),
            new("three", 3, "3"),
            new("four", 4, "4"),
            new("five", 5, "5"),
            new("six", 6, "6"),
            new("seven", 7, "7"),
            new("eight", 8, "8"),
            new("nine", 9, "9")
        };
        var lockObject = new object();
        var sum = 0;
        Parallel.ForEach(input, line =>
        {
            var indexes = new List<(int index, int number, string digitAsString)>();
            foreach (var numberAsString in numbersAsStrings)
            {
                var index = line.AllIndexesOf(numberAsString.digitAsString);
                foreach (var i in index.Where(x => x != -1))
                {
                    indexes.Add((i, numberAsString.number, numberAsString.digitAsString));
                }
                var index2 = line.AllIndexesOf(numberAsString.numberAsString);
                foreach (var i in index2.Where(x => x != -1))
                {
                    indexes.Add((i, numberAsString.number, numberAsString.digitAsString));
                }
            }

            indexes = [.. indexes.OrderBy(x => x.index)];
            var res = int.Parse(indexes.First().digitAsString + indexes.Last().digitAsString);
            lock (lockObject)
            {
                sum += res;
            }
        });
        return sum.ToString();
    }
}
