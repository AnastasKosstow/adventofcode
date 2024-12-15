using System.Text.RegularExpressions;

namespace adventofcode;

public partial class ClawContraption : ISolution
{
    public int Day { get; } = 13;
    public string Puzzle { get; } = "Claw Contraption";

    private Lazy<string[][]> Input;

    private static readonly Regex CoordinatePattern = AxesWeightPatter();

    public (string partOne, string partTwo) Execute()
    {
        var partOne = SolutionPartOne();
        return (partOne.ToString(), string.Empty);
    }

    public void SetInput(string inputSource)
    {
        Input = new(() =>
        {
            var lines = File.ReadAllLines(inputSource);

            return lines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Chunk(3)
                .ToArray();
        });
    }

    internal int SolutionPartOne()
    {
        var clawMachines = Input.Value;

        int result = 0;
        foreach (var configuration in clawMachines)
        {
            var tokens = CalculateCheapestWay(configuration);
            if (tokens != int.MaxValue)
            {
                result += tokens;
            }
        }

        return result;
    }

    internal void SolutionPartTwo()
    {
        var clawMachines = Input.Value;

    }

    private static int CalculateCheapestWay(string[] clawMachineConfiguration)
    {
        (int x, int y) A = ParseCoordinateLine(clawMachineConfiguration[0]);
        (int x, int y) B = ParseCoordinateLine(clawMachineConfiguration[1]);
        (int x, int y) prize = ParseCoordinateLine(clawMachineConfiguration[2]);

        var result = int.MaxValue;
        var b_index = 1;
        while (true)
        {
            var x_position = B.x * b_index;
            var y_position = B.y * b_index;
            var a_index = 0;

            if (x_position > prize.x || y_position > prize.y)
            {
                break;
            }

            while (true)
            {
                a_index++;
                x_position += A.x;
                y_position += A.y;

                if (x_position > prize.x || y_position > prize.y)
                {
                    break;
                }

                if (x_position == prize.x && y_position == prize.y)
                {
                    var currentPrize = (a_index * 3) + b_index;
                    if (currentPrize < result)
                    {
                        result = currentPrize;
                    }
                    break;
                }
            }
            b_index++;
        }

        return result;
    }

    private static (int x, int y) ParseCoordinateLine(string line)
    {
        var match = CoordinatePattern.Match(line);

        return (
            x: int.Parse(match.Groups[1].Value),
            y: int.Parse(match.Groups[2].Value)
        );
    }

    [GeneratedRegex(@"X[\+=](\d+),\s*Y[\+=](\d+)")]
    private static partial Regex AxesWeightPatter();
}
