using System.Text.RegularExpressions;

namespace adventofcode;

internal partial class MullItOver
{
    private static readonly Regex MultiplyPattern = MultiplicationPattern();
    private static readonly Regex MultiplyWithInstructionsPattern = MultiplicationWithInstructionsPattern();


    private static readonly Lazy<string> Input = new(() =>
    {
        var input = File.ReadAllText("day-03/input.txt");

        return input;
    });

    internal static void SolutionPartOne()
    {
        decimal result = 0;

        foreach (Match match in MultiplyPattern.Matches(Input.Value).Cast<Match>())
        {
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);
            result += x * y;
        }

        // result: 190604937
    }

    internal static void SolutionPartTwo()
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

        // result: 82857512
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled)]
    private static partial Regex MultiplicationPattern();

    [GeneratedRegex(@"(mul\((\d{1,3}),(\d{1,3})\))|(do\(\))|(don't\(\))", RegexOptions.Compiled)]
    private static partial Regex MultiplicationWithInstructionsPattern();
}
