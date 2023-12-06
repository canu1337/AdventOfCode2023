namespace AdventOfCode2023;
internal class Day6 : DayBase
{
    public Day6()
    {
    }

    public override void Init()
    {
    }

    private static List<(long Time, long Record)> ParseInput()
    {
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day6.txt"));
        var times = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToList();
        var distances = input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(long.Parse).ToList();
        var res = new List<(long Time, long Record)>();
        for (var i = 0; i < times.Count; i++)
        {
            res.Add((times[i], distances[i]));
        }
        return res;
    }

    public override string Part1()
    {
        var races = ParseInput();
        var solutions = new List<long>();
        foreach (var (Time, Record) in races)
        {
            var minPushTime = 0L;
            var maxPushTime = Time;
            var distance = 0L;
            while (distance <= Record)
            {
                minPushTime++;
                distance = GetDistance(minPushTime, Time);
            }
            distance = 0;
            while (distance <= Record)
            {
                maxPushTime--;
                distance = GetDistance(maxPushTime, Time);
            }
            solutions.Add(maxPushTime - minPushTime + 1);
        }
        return solutions.Aggregate((a, b) => a * b).ToString();
    }

    private static long GetDistance(long pushTime, long maxTime)
    {
        var speed = pushTime;
        return speed * (maxTime - pushTime);
    }

    public override string Part2()
    {
        (var Time, var Record) = ParseInputPart2();
        var minPushTime = 0L;
        var maxPushTime = Time;
        var distance = 0L;
        while (distance <= Record)
        {
            minPushTime++;
            distance = GetDistance(minPushTime, Time);
        }
        distance = 0;
        while (distance <= Record)
        {
            maxPushTime--;
            distance = GetDistance(maxPushTime, Time);
        }
        var res = maxPushTime - minPushTime + 1;
        return res.ToString();
    }

    private static (long Time, long Record) ParseInputPart2()
    {
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day6.txt"));
        var time = long.Parse(input[0].Remove(0, 5).Replace(" ", string.Empty));
        var distance = long.Parse(input[1].Remove(0, 9).Replace(" ", string.Empty));
        return (time, distance);
    }
}
