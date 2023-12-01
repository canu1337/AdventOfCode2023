using System.Diagnostics;

namespace AdventOfCode2023;

internal abstract class DayBase
{
    private static readonly long RUNS = 100;
    public void RunAndTime()
    {
        Init();
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        Console.WriteLine("Day " + GetType().Name);
        Console.WriteLine($"Part 1 {RUNS} samples");
        for (var i = 0; i < RUNS; i++)
        {
            Part1();
        }
        stopWatch.Stop();
        var time = stopWatch.ElapsedMilliseconds / (float)RUNS;
        Console.WriteLine("time:" + time + " ms");
        stopWatch.Restart();
        Console.WriteLine($"Part 2 {RUNS} samples");
        for (var i = 0; i < RUNS; i++)
        {
            Part2();
        }
        stopWatch.Stop();
        var time2 = stopWatch.ElapsedMilliseconds / (float)RUNS;
        Console.WriteLine("time:" + time2 + " ms");

    }
    public abstract string Part1();
    public abstract string Part2();
    public abstract void Init();
}