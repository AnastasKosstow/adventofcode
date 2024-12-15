namespace adventofcode;

public class PlutonianPebbles : ISolution
{
    public int Day { get; } = 11;
    public string Puzzle { get; } = "Plutonian Pebbles";

    private Lazy<decimal[]> Input;

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
            var input = lines[0]
                .Split(' ')
                .Select(decimal.Parse)
                .ToArray();

            return input;
        });
    }

    private decimal CountStones(LinkedList stones)
    {
        var count = 0;
        var node = stones.Head;

        while (node != null)
        {
            count++;
            node = node.Next;
        }

        return count;
    }

    internal decimal SolutionPartOne()
    {
        var input = Input.Value;
        var blinks = 25;

        var stones = new LinkedList();
        foreach (var value in input)
        {
            stones.Add(value);
        }

        do
        {
            blinks--;

            Console.WriteLine(stones.Length);

            var node = stones.Head;
            while (node != null)
            {
                if (node.Value == 0)
                {
                    node.Value = 1;
                }
                else if ((int)(Math.Log10((double)node.Value) + 1) % 2 == 0)
                {
                    stones.Split(node);
                    node = node.Next;
                }
                else
                {
                    node.Value *= 2024;
                }

                node = node.Next;
            }
        }
        while (blinks > 0);

        decimal result = CountStones(stones);
        return result;
    }

    internal decimal SolutionPartTwo()
    {
        var input = Input.Value;
        var blinks = 75;

        var stones = new Dictionary<decimal, decimal>();
        for (var idx = 0; idx < input.Length; idx++)
        {
            if (!stones.TryGetValue(input[idx], out _))
            {
                stones.Add(input[idx], 0);
            }

            stones[input[idx]]++;
        }

        do
        {
            blinks--;

            var additions = new List<(decimal key, decimal count)>();
            var deletions = new List<(decimal key, decimal count)>();
            foreach (var stone in stones)
            {
                if (stone.Key == 0)
                {
                    additions.Add((1, stone.Value));
                }
                else if ((int)(Math.Log10((double)stone.Key) + 1) % 2 == 0)
                {
                    (decimal left, decimal right) splitStones = Split(stone.Key);
                    additions.Add((splitStones.left, stone.Value));
                    additions.Add((splitStones.right, stone.Value));
                }
                else
                {
                    additions.Add((stone.Key * 2024, stone.Value));
                }
                deletions.Add((stone.Key, stone.Value));
            }

            foreach (var (key, count) in additions)
            {
                if (stones.TryGetValue(key, out decimal existingCount))
                {
                    stones[key] = existingCount + count;
                }
                else
                {
                    stones[key] = count;
                }
            }

            foreach (var (key, count) in deletions)
            {
                if (stones.TryGetValue(key, out _))
                {
                    stones[key] -= count;
                    if (stones[key] <= 0)
                    {
                        stones.Remove(key);
                    }
                }
            }
        }
        while (blinks > 0);

        (decimal, decimal) Split(decimal value)
        {
            string valueStr = value.ToString();
            var halfLength = valueStr.Length / 2;

            var left = decimal.Parse(valueStr[halfLength..]);
            var right = decimal.Parse(valueStr[..halfLength]);
            return (left, right);
        }

        decimal result = 0;
        foreach (decimal value in stones.Values)
        {
            result += value;
        }

        return result;
    }
}
