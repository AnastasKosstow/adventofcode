namespace adventofcode;

public class HistorianHysteria : ExecutionMeasure, ISolution
{
    public int Day { get; } = 1;
    public string Puzzle { get; } = "Historian Hysteria";

    private Lazy<(Memory<int> left, Memory<int> right)> Input;

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        var resultPartOne = SolutionPartOne();
        var resultPartTwo = SolutionPartTwo();
        var millisecondsPartOne = MeasureExecutionTime(SolutionPartOne);
        var millisecondsPartTwo = MeasureExecutionTime(SolutionPartTwo);

        return ((resultPartOne.ToString(), millisecondsPartOne), (resultPartTwo.ToString(), millisecondsPartTwo));
    }

    public void SetInput(string inputSource)
    {
        Input = new(() =>
        {
            int[] left = new int[1000];
            int[] right = new int[1000];
            int idx = 0;

            foreach (var line in File.ReadAllLines(inputSource))
            {
                var separatorIdx = line.IndexOf(' ');
                left[idx] = int.Parse(line[..separatorIdx]);
                right[idx] = int.Parse(line[(separatorIdx + 1)..]);
                idx++;
            }

            return (left.AsMemory(0, idx), right.AsMemory(0, idx));
        });
    }

    internal int SolutionPartOne()
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

        return result;
    }

    internal int SolutionPartTwo()
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

        return result;
    }
}
