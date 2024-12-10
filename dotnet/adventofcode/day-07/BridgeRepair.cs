namespace adventofcode;

internal class BridgeRepair
{
    private static Lazy<string[]> Input = new(() =>
    {
        var lines = File.ReadAllLines("day-07/input.txt");
        var input = lines.Select(line => line).ToArray();
        
        return input;
    });

    private static decimal ApplyOperation(decimal left, decimal right, string op)
    {
        return op switch
        {
            "+" => left + right,
            "*" => left * right,
            "||" => decimal.Parse(left.ToString("F0") + right.ToString("F0"))
        };
    }

    private static bool CanAchieveTarget(long targetValue, long[] values, bool useConcatenation = false)
    {
        if (values == null || values.Length < 2)
            return false;

        var operators = useConcatenation
            ? new[] { "+", "*", "||" }
            : new[] { "+", "*" };

        return TryAllOperatorCombinations(targetValue, values.Select(v => (decimal)v).ToArray(), operators);
    }

    private static bool TryAllOperatorCombinations(decimal target, decimal[] values, string[] operators)
    {
        var operatorPositions = new string[values.Length - 1];
        return TryOperators(0);

        bool TryOperators(int position)
        {
            if (position == operatorPositions.Length)
            {
                return EvaluateExpression(values, operatorPositions) == target;
            }

            foreach (var op in operators)
            {
                operatorPositions[position] = op;
                if (TryOperators(position + 1))
                    return true;
            }

            return false;
        }
    }

    private static decimal EvaluateExpression(decimal[] values, string[] operators)
    {
        var result = values[0];

        for (int idx = 0; idx < operators.Length; idx++)
        {
            result = ApplyOperation(result, values[idx + 1], operators[idx]);
        }

        return result;
    }

    internal static void SolutionPartOne()
    {
        var input = Input.Value;

        decimal result = 0;
        foreach (var line in input)
        {
            var parts = line.Split(':');

            long target = long.Parse(parts[0].Trim());
            long[] values = parts[1].Trim().Split(' ')
                                    .Select(long.Parse)
                                    .ToArray();

            if (CanAchieveTarget(target, values))
            {
                result += target;
            }
        }

        // result: 4364915411363
    }

    internal static void SolutionPartTwo()
    {
        var input = Input.Value;

        decimal result = 0;
        foreach (var line in input)
        {
            var parts = line.Split(':');

            long target = long.Parse(parts[0].Trim());
            long[] values = parts[1].Trim().Split(' ')
                                    .Select(long.Parse)
                                    .ToArray();

            if (CanAchieveTarget(target, values, useConcatenation: true))
            {
                result += target;
            }
        }

        // result: 38322057216320
    }
}
