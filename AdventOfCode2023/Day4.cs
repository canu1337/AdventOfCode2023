using System.Collections.Concurrent;

namespace AdventOfCode2023;
internal class Day4 : DayBase
{
    public override void Init()
    {
    }

    public override string Part1()
    {
        var sum = 0;
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day4.txt"));
        foreach (var line in input)
        {
            var parts = line.Split(':')[1].Split("|");
            var winnings = parts[0].Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse);
            var ours = parts[1].Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse);
            var gains = ours.Intersect(winnings);
            if (gains.Any())
            {
                double total = 1;
                total += Math.Pow(2, gains.Count() - 1) - 1;
                sum += (int)total;
            }
        }

        return sum.ToString();
    }

    public override string Part2()
    {
        var sum = 0;

        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day4.txt"));
        var dict = new ConcurrentDictionary<int, (int gains, int scratchCards)>();
        Parallel.For(0, input.Length, i =>
        {
            var parts = input[i].Split(':', 2)[1].Split('|', 2);
            var winnings = new HashSet<int>(parts[0].Split(' ', 10, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            var ours = parts[1].Split(' ', 25, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var gains = ours.Count(winnings.Contains);
            dict.TryAdd(i, (gains, 1));
        });

        foreach (var item in dict)
        {
            sum += item.Value.scratchCards;
            for (var i = 0; i < item.Value.gains; i++)
            {
                var index = item.Key + i + 1;
                if (dict.ContainsKey(index))
                {
                    var (gains, scratchCards) = dict[index];
                    dict[index] = new(gains, scratchCards + item.Value.scratchCards);
                }
            }
        }


        return sum.ToString();
    }
}
