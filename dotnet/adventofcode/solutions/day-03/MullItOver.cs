using System.Text.RegularExpressions;

namespace adventofcode;

public partial class MullItOver : ExecutionMeasure, ISolution
{
    public int Day { get; } = 3;
    public string Puzzle { get; } = "Mull It Over";

    private static readonly Regex MultiplyPattern = MultiplicationPattern();
    private static readonly Regex MultiplyWithInstructionsPattern = MultiplicationWithInstructionsPattern();

    private Lazy<string> Input;

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
            var input = File.ReadAllText(inputSource);

            return input;
        });
    }

    internal decimal SolutionPartOne()
    {
        decimal result = 0;

        foreach (Match match in MultiplyPattern.Matches(Input.Value).Cast<Match>())
        {
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);
            result += x * y;
        }

        return result;
    }

    internal decimal SolutionPartTwo()
    {
        decimal result = 0;
        bool enabled = true;

        foreach (Match match in MultiplyWithInstructionsPattern.Matches(Input.Value).Cast<Match>())
        {
            string instruction = match.Groups[0].Value;

            if (instruction == "do()")
            {
                enabled = true;
                continue;
            }

            if (instruction == "don't()")
            {
                enabled = false;
                continue;
            }

            if (enabled)
            {
                int x = int.Parse(match.Groups[2].Value);
                int y = int.Parse(match.Groups[3].Value);
                result += x * y;
            }
        }

        return result;
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled)]
    private static partial Regex MultiplicationPattern();

    [GeneratedRegex(@"(mul\((\d{1,3}),(\d{1,3})\))|(do\(\))|(don't\(\))", RegexOptions.Compiled)]
    private static partial Regex MultiplicationWithInstructionsPattern();
}
