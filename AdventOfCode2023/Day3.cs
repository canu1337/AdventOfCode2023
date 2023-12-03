namespace AdventOfCode2023;
internal class Day3 : DayBase
{
    public override void Init()
    {
    }

    public override string Part1()
    {
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day3.txt"));
        var engine = ParseInput(input);
        var sum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                var currentSymbol = engine[i, j].Symbol;
                if (!char.IsDigit(currentSymbol) && currentSymbol != '.')
                {
                    var parts = GetPartsAroundCell(input, engine, i, j);
                    sum += parts.Sum();
                }
            }
        }
        return sum.ToString();
    }

    public override string Part2()
    {
        var sum = 0;
        var input = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Day3.txt"));
        var engine = ParseInput(input);
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                var currentSymbol = engine[i, j].Symbol;
                if (currentSymbol == '*')
                {
                    var parts = GetPartsAroundCell(input, engine, i, j);

                    if (parts.Count == 2)
                    {
                        sum += parts[0] * parts[1];
                    }

                }
            }
        }

        return sum.ToString();
    }

    private static List<int> GetPartsAroundCell(string[] input, Cell[,] engine, int i, int j)
    {
        var parts = new List<int>();
        var left = j - 1 >= 0 ? engine[i, j - 1] : new Cell();
        parts.Add(GetPart(input, engine, i, j - 1, left));
        var right = j + 1 < input[0].Length ? engine[i, j + 1] : new Cell();
        parts.Add(GetPart(input, engine, i, j + 1, right));
        var up = i - 1 >= 0 ? engine[i - 1, j] : new Cell();
        parts.Add(GetPart(input, engine, i - 1, j, up));
        var down = i + 1 < input.Length ? engine[i + 1, j] : new Cell();
        parts.Add(GetPart(input, engine, i + 1, j, down));
        var leftUp = i - 1 >= 0 && j - 1 >= 0 ? engine[i - 1, j - 1] : new Cell();
        parts.Add(GetPart(input, engine, i - 1, j - 1, leftUp));
        var rightUp = i - 1 >= 0 && j + 1 < input[0].Length ? engine[i - 1, j + 1] : new Cell();
        parts.Add(GetPart(input, engine, i - 1, j + 1, rightUp));
        var leftDown = i + 1 < input.Length && j - 1 >= 0 ? engine[i + 1, j - 1] : new Cell();
        parts.Add(GetPart(input, engine, i + 1, j - 1, leftDown));
        var rightDown = i + 1 < input.Length && j + 1 < input[0].Length ? engine[i + 1, j + 1] : new Cell();
        parts.Add(GetPart(input, engine, i + 1, j + 1, rightDown));
        parts = parts.Where(x => x != 0).ToList();
        return parts;
    }

    private struct Cell
    {
        public Cell(char symbol, bool isChecked)
        {
            Symbol = symbol;
            Checked = isChecked;
        }
        public Cell()
        {
            Symbol = default;
            Checked = true;
        }
        public char Symbol { get; set; }
        public bool Checked { get; set; }

    };

    private static Cell[,] ParseInput(string[] input)
    {
        var engine = new Cell[input.Length, input[0].Length];
        Parallel.For(0, input.Length, i =>
        {
            var span = input[i].AsSpan();
            for (var j = 0; j < span.Length; j++)
            {
                engine[i, j] = new Cell(span[j], false);
            }
        });
        return engine;
    }

    private static int GetPart(string[] input, Cell[,] engine, int i, int j, Cell cell)
    {
        if (!cell.Checked && char.IsDigit(cell.Symbol))
        {
            var number = FindNumber(input[i], j);
            foreach (var index in number.indexes)
            {
                engine[i, index] = new Cell(input[i][index], true);
            }
            return number.number;
        }

        return 0;
    }

    private static (int number, List<int> indexes) FindNumber(string line, int pos)
    {
        var indexes = new List<int>();
        for (var i = pos; i < line.Length; i++)
        {
            if (char.IsDigit(line[i]))
            {
                indexes.Add(i);
            }
            else
            {
                break;
            }
        }
        for (var i = pos; i >= 0; i--)
        {
            if (char.IsDigit(line[i]))
            {
                indexes.Add(i);
            }
            else
            {
                break;
            }
        }
        var number = 0;

        foreach (var index in indexes.Distinct().Order())
        {
            var digit = line[index] - '0';
            number = number * 10 + digit;
        }
        return (number, indexes);
    }
}
