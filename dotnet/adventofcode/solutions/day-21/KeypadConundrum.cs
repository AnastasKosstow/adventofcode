using System.Collections.Concurrent;
using System.Numerics;

namespace adventofcode;

public class KeypadConundrum : ExecutionMeasure, ISolution
{
    public int Day => 21;
    public string Puzzle => "Keypad Conundrum";

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        var resultPartOne = SolutionPartOne();
        var resultPartTwo = SolutionPartTwo();
        var millisecondsPartOne = MeasureExecutionTime(SolutionPartOne);
        var millisecondsPartTwo = MeasureExecutionTime(SolutionPartTwo);

        return ((resultPartOne.ToString(), millisecondsPartOne), (resultPartTwo.ToString(), millisecondsPartTwo));
    }

    private Dictionary<char, Vector2> NumericPad = new()
    {
        {'7', new Vector2(0, 0)},
        {'8', new Vector2(1, 0)},
        {'9', new Vector2(2, 0)},
        {'4', new Vector2(0, 1)},
        {'5', new Vector2(1, 1)},
        {'6', new Vector2(2, 1)},
        {'1', new Vector2(0, 2)},
        {'2', new Vector2(1, 2)},
        {'3', new Vector2(2, 2)},
        {'g', new Vector2(0, 3)},
        {'0', new Vector2(1, 3)},
        {'A', new Vector2(2, 3)},
    };

    private Dictionary<char, Vector2> DirectionPad = new()
    {
        {'g', new Vector2(0, 0)},
        {'^', new Vector2(1, 0)},
        {'A', new Vector2(2, 0)},
        {'<', new Vector2(0, 1)},
        {'v', new Vector2(1, 1)},
        {'>', new Vector2(2, 1)},
    };

    private ConcurrentDictionary<(char currentKey, char nextKey, int depth), long> Cache;
    private Lazy<Memory<string>> Input;

    public void SetInput(string inputSource)
    {
        Input = new(() =>
        {
            var lines = File.ReadAllLines(inputSource);
            var input = new string[lines.Length];

            for (int idx = 0; idx < lines.Length; idx++)
            {
                input[idx] = lines[idx];
            }

            return input.AsMemory();
        });
    }

    private long CalculateComplexity(string code, int depth)
    {
        Cache = [];
        long totalSequenceLength = FindShortestSequence(code, depth, depth);
        
        var result = long.Parse(code[..3]) * totalSequenceLength;
        return result;
    }

    private long FindShortestSequence(string code, int depth, int maximumDepth)
    {
        if (depth == 0)
        {
            return code.Length;
        }

        var currentKey = 'A';
        var length = 0L;
        
        foreach (var nextKey in code)
        {
            length += ProcessKey(currentKey, nextKey, depth, maximumDepth);
            currentKey = nextKey;
        }

        return length;
    }

    private long ProcessKey(char currentKey, char nextKey, int depth, int maxDepth)
    {
        return Cache.GetOrAdd((currentKey, nextKey, depth), _ =>
        {
            Dictionary<char, Vector2> keypad = DirectionPad;
            if (depth == maxDepth)
            {
                keypad = NumericPad;
            }

            Vector2 sourceVector = keypad[currentKey];
            Vector2 destinationVector = keypad[nextKey];

            string horizontalKeys = MoveHorizontally(sourceVector, destinationVector);
            string verticalKeys = MoveVertically(sourceVector, destinationVector);

            var cost = long.MaxValue;

            if (keypad['g'] != new Vector2(sourceVector.X, destinationVector.Y))
            {
                cost = Math.Min(cost, FindShortestSequence($"{verticalKeys}{horizontalKeys}A", depth - 1, maxDepth));
            }

            if (keypad['g'] != new Vector2(destinationVector.X, sourceVector.Y))
            {
                cost = Math.Min(cost, FindShortestSequence($"{horizontalKeys}{verticalKeys}A", depth - 1, maxDepth));
            }
            return cost;
        });
    }

    private string MoveHorizontally(Vector2 sourceVector, Vector2 destinationVector)
    {
        if (sourceVector.X == destinationVector.X)
        {
            return string.Empty;
        }

        int horizontalDistance = (int)(sourceVector.X - destinationVector.X);
        return new string(horizontalDistance > 0 ? '<' : '>', Math.Abs(horizontalDistance));
    }

    private string MoveVertically(Vector2 sourceVector, Vector2 destinationVector)
    {
        if (sourceVector.Y == destinationVector.Y)
        {
            return string.Empty;
        }

        int verticalDistance = (int)(sourceVector.Y - destinationVector.Y);
        return new string(verticalDistance > 0 ? '^' : 'v', Math.Abs(verticalDistance));
    }

    private long SolutionPartOne()
    {
        var input = Input.Value.Span;

        long result = 0;
        foreach (string code in input)
        {
            result += CalculateComplexity(code, 3);
        }

        return result;
    }

    private long SolutionPartTwo()
    {
        var input = Input.Value.Span;

        long result = 0;
        foreach (string code in input)
        {
            result += CalculateComplexity(code, 26);
        }

        return result;
    }
}
