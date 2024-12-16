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
        var partTwo = SolutionPartTwo();
        return (partOne.ToString(), partTwo.ToString());
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

    internal long SolutionPartOne()
    {
        var clawMachines = Input.Value;

        long result = 0;
        foreach (var configuration in clawMachines)
        {
            long tokens = CalculateCheapestWay(configuration);
            if (tokens != int.MaxValue)
            {
                result += tokens;
            }
        }

        return result;
    }

    internal long SolutionPartTwo()
    {
        var clawMachines = Input.Value;

        long result = 0;
        foreach (var configuration in clawMachines)
        {
            long tokens = CalculateCheapestWay(configuration, true);
            if (tokens != int.MaxValue)
            {
                result += tokens;
            }
        }

        return result;
    }

    private static long CalculateCheapestWay(string[] clawMachineConfiguration, bool includeConversion = false)
    {
        (int x, int y) A = ParseCoordinateLine(clawMachineConfiguration[0]);
        (int x, int y) B = ParseCoordinateLine(clawMachineConfiguration[1]);
        (int x, int y) prize = ParseCoordinateLine(clawMachineConfiguration[2]);

        var prizeX = (double)prize.x;
        var prizeY = (double)prize.y;
        if (includeConversion)
        {
            prizeX += 10000000000000;
            prizeY += 10000000000000;
        }
        
        var aX = (double)A.x;
        var aY = (double)A.y;
        var bX = (double)B.x;
        var bY = (double)B.y;

        long b = (long)Math.Round((prizeY - (prizeX / aX) * aY) / (bY - (bX / aX) * aY));
        long a = (long)Math.Round((prizeX - b * bX) / aX);

        long result = 0;
        var actualX = a * aX + b * bX;
        var actualY = a * aY + b * bY;
        if (actualX == prizeX && actualY == prizeY && a >= 0 && b >= 0)
        {
            result += a * 3 + b;
        }

        return result;
    }


    //private static int CalculateCheapestWay(string[] clawMachineConfiguration)
    //{
    //    (int x, int y) A = ParseCoordinateLine(clawMachineConfiguration[0]);
    //    (int x, int y) B = ParseCoordinateLine(clawMachineConfiguration[1]);
    //    (int x, int y) prize = ParseCoordinateLine(clawMachineConfiguration[2]);

    //    var result = int.MaxValue;
    //    var b_index = 1;
    //    while (true)
    //    {
    //        var x_position = B.x * b_index;
    //        var y_position = B.y * b_index;
    //        var a_index = 0;

    //        if (x_position > prize.x || y_position > prize.y)
    //        {
    //            break;
    //        }

    //        while (true)
    //        {
    //            a_index++;
    //            x_position += A.x;
    //            y_position += A.y;

    //            if (x_position > prize.x || y_position > prize.y)
    //            {
    //                break;
    //            }

    //            if (x_position == prize.x && y_position == prize.y)
    //            {
    //                var currentPrize = (a_index * 3) + b_index;
    //                if (currentPrize < result)
    //                {
    //                    result = currentPrize;
    //                }
    //                break;
    //            }
    //        }
    //        b_index++;
    //    }

    //    return result;
    //}

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
