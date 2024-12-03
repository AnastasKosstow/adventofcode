namespace adventofcode;

internal class HistorianHysteria
{
    private static readonly Lazy<(Memory<int> left, Memory<int> right)> Input = new(() =>
    {
        int[] left = new int[1000];
        int[] right = new int[1000];
        int idx = 0;

        foreach (var line in File.ReadAllLines("day-1/input.txt"))
        {
            var separatorIdx = line.IndexOf(' ');
            left[idx] = int.Parse(line[..separatorIdx]);
            right[idx] = int.Parse(line[(separatorIdx + 1)..]);
            idx++;
        }
        
        return (left.AsMemory(0, idx), right.AsMemory(0, idx));
    });

    internal static void SolutionPartOne()
    {
        var (left, right) = Input.Value;
        var leftSpan = left.Span;
        var rightSpan = right.Span;

        leftSpan.Sort();
        rightSpan.Sort();

        int result = 0;
        for (int idx = 0; idx < leftSpan.Length; idx++)
        {
            result += Math.Abs(leftSpan[idx] - rightSpan[idx]);
        }

        // result: 1646452
    }

    internal static void SolutionPartTwo()
    {
        var (left, right) = Input.Value;
        var rightCounts = right.Span.ToArray()
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => g.Count());

        int result = 0;
        for (int idx = 0; idx < left.Span.Length; idx++)
        {
            rightCounts.TryGetValue(left.Span[idx], out var value);

            result += value * left.Span[idx];
        }

        // result: 23609874
    }
}
