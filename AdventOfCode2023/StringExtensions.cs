namespace AdventOfCode2023;
internal static class StringExtensions
{
    //stolen from https://stackoverflow.com/a/24016130
    public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
    {
        var minIndex = str.IndexOf(searchstring);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(searchstring, minIndex + 1);
        }
    }
}
