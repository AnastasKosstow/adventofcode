namespace adventofcode;

public class CrossedWires : ExecutionMeasure, ISolution
{
    public int Day => 24;
    public string Puzzle => "Crossed Wires";

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        var resultPartOne = SolutionPartOne();
        var resultPartTwo = SolutionPartTwo();
        var millisecondsPartOne = MeasureExecutionTime(SolutionPartOne);
        var millisecondsPartTwo = MeasureExecutionTime(SolutionPartTwo);

        return ((resultPartOne.ToString(), millisecondsPartOne), (resultPartTwo.ToString(), millisecondsPartTwo));
    }

    private Dictionary<string, int> Wires = [];
    private Queue<string> Instructions = [];

    public void SetInput(string inputSource)
    {
        var lines = File.ReadAllLines(inputSource);

        int separatorIndex = Array.IndexOf(lines, string.Empty);

        Wires = lines
            .Take(separatorIndex)
            .Select(line => line.Split(':', StringSplitOptions.TrimEntries))
            .ToDictionary(parts => parts[0], parts => int.Parse(parts[1]));

        Instructions = new Queue<string>(
            lines.Skip(separatorIndex + 1)
                .Where(line => !string.IsNullOrEmpty(line))
        );
    }

    private int SolutionPartOne()
    {
        throw new NotImplementedException();
    }

    private int SolutionPartTwo()
    {
        throw new NotImplementedException();
    }
}
