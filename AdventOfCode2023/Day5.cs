using System.Data;

namespace AdventOfCode2023;
internal class Day5 : DayBase
{
    private static readonly Category[] _categories = [Category.Seed, Category.Soil, Category.Fertilizer, Category.Water, Category.Light, Category.Temperature, Category.Humidity];
    public override void Init()
    {
    }

    public override string Part1()
    {
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day5.txt"));
        var seeds = GetSeeds(input);
        var maps = ParseInput(input);

        var locations = GetLocations(seeds, maps);

        var res = locations.Min();
        return res.ToString();
    }

    private const long SEGMENT_SIZE = 100000;
    public override string Part2()
    {
        var lockObject = new object();
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day5.txt"));
        var seeds = GetSeedsPart2(input);
        var maps = ParseInput(input);
        var ranges = new List<(long Start, long End)>();
        var res = new List<(long Start, long End)>();

        var lowestLocation = -SEGMENT_SIZE;
        var found = false;
        List<long> results = [];
        while (!found)
        {
            lowestLocation += SEGMENT_SIZE;
            var segments = new List<long>();
            for (var i = 0; i < SEGMENT_SIZE; i++)
            {
                segments.Add(lowestLocation + i);
            }
            Parallel.ForEach(segments, segment =>
            {
                var currentValue = segment;
                foreach (var category in _categories.Reverse())
                {
                    var map = maps[category];
                    var range = map.Ranges.FirstOrDefault(r => r.DestinationStart + r.Length > currentValue && r.DestinationStart <= currentValue);
                    if (range is null)
                    {
                        continue;
                    }
                    currentValue = range.SourceStart - range.DestinationStart + currentValue;
                }
                if (seeds.Any(s => s.Start <= currentValue && s.End > currentValue))
                {
                    found = true;
                    lock (lockObject)
                    {
                        results.Add(segment);
                    }
                }
            });
        }
        return results.Min().ToString();
    }

    private static List<long> GetLocations(IEnumerable<long> seeds, Dictionary<Category, Map> maps)
    {
        var locations = new List<long>();
        foreach (var seed in seeds)
        {
            var currentValue = seed;
            foreach (var category in _categories)
            {
                if (category == Category.Location)
                {
                    locations.Add(currentValue);
                    break;
                }
                var map = maps[category];
                var destination = map.Ranges.FirstOrDefault(r => r.SourceStart + r.Length > currentValue && r.SourceStart <= currentValue);
                if (destination is not null)
                {
                    currentValue = destination.DestinationStart + (currentValue - destination.SourceStart);
                }
                // else current value does not change
            }
        }
        return locations;
    }


    private static IEnumerable<long> GetSeeds(string[] input)
    {
        return input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(long.Parse);
    }

    private static IEnumerable<(long Start, long End)> GetSeedsPart2(string[] input)
    {
        var ranges = input[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .Select(long.Parse);


        return GroupValues(ranges)
            .Select(r => (r.Item1, r.Item1 + r.Item2 - 1));
    }



    private static Dictionary<Category, Map> ParseInput(string[] input)
    {
        var maps = new Dictionary<Category, Map>();

        var currentMap = new Map();
        var currentCategory = Category.Seed;
        foreach (var line in input.Skip(2))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                maps.Add(currentCategory, currentMap);
                currentMap = new Map();
                continue;
            }
            if (line.EndsWith(':'))
            {
                var cats = line[..^5]
                    .Split("-");

                currentCategory = ParseCategory(cats[0]);
                currentMap.DestinationCategory = ParseCategory(cats[2]);
            }
            else
            {
                var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var sourceStart = long.Parse(parts[1]);
                var destinationStart = long.Parse(parts[0]);
                var length = long.Parse(parts[2]);

                currentMap.Ranges.Add(new Range
                {
                    SourceStart = sourceStart,
                    DestinationStart = destinationStart,
                    Length = length
                });
            }

        }
        maps.Add(currentCategory, currentMap);
        return maps;
    }

    private static Category ParseCategory(string input)
    {
        return input switch
        {
            "seed" => Category.Seed,
            "soil" => Category.Soil,
            "fertilizer" => Category.Fertilizer,
            "water" => Category.Water,
            "light" => Category.Light,
            "temperature" => Category.Temperature,
            "humidity" => Category.Humidity,
            "location" => Category.Location,
            _ => throw new Exception("Unknown category")
        };
    }
    private static IEnumerable<(long, long)> GroupValues(IEnumerable<long> values)
    {
        var groupedValues = new List<(long, long)>();
        var enumerator = values.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var firstValue = enumerator.Current;
            if (enumerator.MoveNext())
            {
                var secondValue = enumerator.Current;
                groupedValues.Add((firstValue, secondValue));
            }
        }
        return groupedValues;
    }

    private record Range
    {
        public long SourceStart;
        public long DestinationStart;
        public long Length;

        public (long Start, long End) GetSourceRange() => (SourceStart, SourceStart + Length);
        public (long Start, long End) GetDestinationRange() => (DestinationStart, DestinationStart + Length);
    }

    private struct Map
    {
        public Map()
        {
            Ranges = [];
        }
        public List<Range> Ranges;
        public Category DestinationCategory;
    }

    private enum Category
    {
        Seed, Soil, Fertilizer, Water, Light, Temperature, Humidity, Location
    }
}
