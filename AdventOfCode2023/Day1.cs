namespace AdventOfCode2023;
internal class Day1 : DayBase
{
    public override string Part1()
    {
        var input = File.ReadAllLines("Day1.txt");
        var sum = 0;
        foreach (var line in input)
        {
            var numbers = line
                .Where(x => int.TryParse(x.ToString(), out _))
                .ToList();


            var res = int.Parse(numbers.First().ToString() + numbers.Last().ToString());
            sum += res;
        }
        return sum.ToString();
    }


    public override string Part2()
    {
        var numbersAsStrings = new List<(string numberAsString, int number)>
        {
            new ("one", 1),
            new ("two", 2),
            new ("three", 3),
            new ("four", 4),
            new ("five", 5),
            new ("six", 6),
            new ("seven", 7),
            new ("eight", 8),
            new ("nine", 9)
        };
        var input = File.ReadAllLines("Day1.txt");
        var sum = 0;
        foreach (var line in input)
        {
            var indexes = new List<(int index, int number)>();
            foreach (var numberAsString in numbersAsStrings)
            {
                var index = line.AllIndexesOf(numberAsString.number.ToString());
                foreach (var i in index.Where(x => x != -1))
                {
                    indexes.Add((i, numberAsString.number));
                }
                var index2 = line.AllIndexesOf(numberAsString.numberAsString.ToString());
                foreach (var i in index2.Where(x => x != -1))
                {
                    indexes.Add((i, numberAsString.number));
                }
            }

            indexes = [.. indexes.OrderBy(x => x.index)];
            var res = int.Parse(indexes.First().number.ToString() + indexes.Last().number.ToString());
            sum += res;
        }
        return sum.ToString();
    }
}
