namespace adventofcode;

public class WarehouseWoes : ExecutionMeasure, ISolution
{
    public int Day => 15;
    public string Puzzle => "Warehouse Woes";

    private (char[,] Warehouse, char[] Instructions) Input;
    private readonly Dictionary<char, int[]> Directions = new()
    {
        { '^', new[] { -1, 0 } },
        { '>', new[] { 0, 1 } },
        { 'v', new[] { 1, 0 } },
        { '<', new[] { 0, -1 } },
    };

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
        Input = ParseInput(inputSource);
    }

    private (char[,] Warehouse, char[] Instructions) ParseInput(string inputSource)
    {
        var lines = File.ReadAllLines(inputSource);

        int separatorIndex = Array.FindIndex(lines, string.IsNullOrEmpty);
        int rows = separatorIndex;
        int cols = lines[0].Length;

        char[,] matrix = new char[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                matrix[row, col] = lines[row][col];
            }
        }

        List<char> instructions = new();
        for (int idx = separatorIndex + 1; idx < lines.Length; idx++)
        {
            if (!string.IsNullOrEmpty(lines[idx]))
            {
                instructions.AddRange(lines[idx].ToCharArray());
            }
        }
        return (matrix, instructions.ToArray());
    }

    private int[] FindStartingPosition(char[,] warehouse)
    {
        for (int row = 0; row < warehouse.GetLength(0); row++)
        {
            for (int col = 0; col < warehouse.GetLength(1); col++)
            {
                if (warehouse[row, col] == '@')
                    return [row, col];
            }
        }

        return [0, 0];
    }

    private long SumBoxesCoordinates(char[,] warehouse, char box)
    {
        long result = 0;
        for (int row = 1; row < warehouse.GetLength(0) - 1; row++)
        {
            for (int col = 1; col < warehouse.GetLength(1) - 1; col++)
            {
                if (warehouse[row, col] == box)
                    result += 100 * row + col;
            }
        }

        return result;
    }

    private long SolutionPartOne()
    {
        var warehouse = (char[,])Input.Warehouse.Clone();
        var instructions = Input.Instructions;

        var currentPosition = FindStartingPosition(warehouse);
        for (int idx = 0; idx < instructions.Length; idx++)
        {
            var direction = Directions[instructions[idx]];

            var nextPosition = new[] { currentPosition[0] + direction[0], currentPosition[1] + direction[1] };
            if (warehouse[nextPosition[0], nextPosition[1]] == '#')
            {
                continue;
            }

            if (warehouse[nextPosition[0], nextPosition[1]] == 'O')
            {
                var boxes = 1;
                var index = 1;
                while (true)
                {
                    if (warehouse[nextPosition[0] + (direction[0] * index), nextPosition[1] + (direction[1] * index)] != 'O')
                    {
                        break;
                    }
                    boxes++;
                    index++;
                }

                var boxNextPosition = new[] { nextPosition[0] + (direction[0] * boxes), nextPosition[1] + (direction[1] * boxes) };
                if (warehouse[boxNextPosition[0], boxNextPosition[1]] != '#')
                {
                    warehouse[boxNextPosition[0], boxNextPosition[1]] = 'O';
                }
                else
                {
                    continue;
                }
            }

            warehouse[nextPosition[0], nextPosition[1]] = '@';
            warehouse[currentPosition[0], currentPosition[1]] = '.';
            currentPosition = nextPosition;
        }

        var result = SumBoxesCoordinates(warehouse, 'O');
        return result;
    }

    private string SolutionPartTwo()
    {
        return "/";
    }
}
