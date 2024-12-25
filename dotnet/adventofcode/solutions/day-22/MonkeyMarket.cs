using static System.Runtime.InteropServices.JavaScript.JSType;

namespace adventofcode;

public class MonkeyMarket : ExecutionMeasure, ISolution
{
    public int Day => 22;
    public string Puzzle => "Monkey Market";

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
            var lines = File.ReadAllLines(inputSource);
            var input = new long[lines.Length];

            for (int idx = 0; idx < lines.Length; idx++)
            {
                input[idx] = long.Parse(lines[idx]);
            }

            return input;
        });
    }

    private Lazy<long[]> Input;

    private int Iterations = 2000;
    private Dictionary<(long, long, long, long), long> Sequences = [];

    private void ProcessSecretSequences(long[] secrets)
    {
        for (int idx = 0; idx < secrets.Length; idx++)
        {
            long s2 = 0, s3 = 0, s4 = 0;

            HashSet<(long, long, long, long)> firstApperance = [];
            for (int step = 0; step < Iterations; step++)
            {
                long prevPrice = secrets[idx] % 10;
                secrets[idx] = GeneratePseudoRandomNumber(secrets[idx]);
                long newPrice = secrets[idx] % 10;

                long s1 = s2;
                s2 = s3;
                s3 = s4;
                s4 = newPrice - prevPrice;

                if (step >= 3 && !firstApperance.Contains((s1, s2, s3, s4)))
                {
                    if (!Sequences.ContainsKey((s1, s2, s3, s4)))
                    {
                        Sequences[(s1, s2, s3, s4)] = 0;
                    }
                    Sequences[(s1, s2, s3, s4)] += newPrice;
                    firstApperance.Add((s1, s2, s3, s4));
                }
            }
        }

        long GeneratePseudoRandomNumber(long secret)
        {
            secret = ((secret * 64L) ^ secret) % 16777216L;
            long div = (long)Math.Floor((double)secret / 32);
            secret ^= div;
            secret %= 16777216L;
            secret = ((secret * 2048L) ^ secret) % 16777216L;
            return secret;
        }
    }

    private long SolutionPartOne()
    {
        var secrets = Input.Value;

        ProcessSecretSequences(secrets);

        var result = secrets.Sum();
        return result;
    }

    private long SolutionPartTwo()
    {
        var secrets = Input.Value;

        var result = Sequences.Max(keyValuePair => keyValuePair.Value);
        return result;
    }
}
