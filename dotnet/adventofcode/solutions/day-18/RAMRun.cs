using System.Data;

namespace adventofcode;

public class RAMRun : ISolution
{
    public int Day => 18;
    public string Puzzle => "RAM Run";

    private Lazy<List<(int row, int col)>> Input;

    public (string partOne, string partTwo) Execute()
    {
        var partOne = SolutionPartOne();
        var partTwo = SolutionPartTwo();
        return (partOne.ToString(), partTwo);
    }

    public void SetInput(string inputSource)
    {
        Input = new(() =>
        {
            var lines = File.ReadAllLines(inputSource);

            var list = new List<(int row, int col)>();
            foreach (var line in lines)
            {
                var coordinates = line.Split(',').Select(int.Parse);
                list.Add((coordinates.Last(), coordinates.First()));
            }

            return list;
        });
    }

    private int FindShortestPath(bool[,] memory)
    {
        var directions = new List<(int, int)>()
        {
            { (-1, 0) }, { (0, 1) }, { (1, 0) }, { (0, -1) },
        };

        int rows = memory.GetLength(0);
        int cols = memory.GetLength(1);

        Dictionary<(int, int), int> paths = [];
        PriorityQueue<(int row, int col), int> queue = new();

        queue.Enqueue((0, 0), 0);
        while (queue.Count > 0)
        {
            var position = queue.Dequeue();
            int row = position.row;
            int col = position.col;

            if (memory[row, col] == false || (row == rows - 1 && col == cols - 1))
            {
                continue;
            }

            paths.TryAdd(position, 0);

            foreach (var direction in directions)
            {
                TryEnqueue(direction);
            }

            void TryEnqueue((int, int) direction)
            {
                var nextRow = row + direction.Item1;
                var nextCol = col + direction.Item2;

                if (nextRow >= 0 && nextCol >= 0 && nextRow < rows && nextCol < cols && (memory[nextRow, nextCol] == true || (nextRow == rows - 1 && nextCol == cols - 1)))
                {
                    var next = (nextRow, nextCol);
                    var newSteps = paths[position] + 1;

                    if (!paths.TryGetValue(next, out int value) || newSteps < value)
                    {
                        value = newSteps;
                        paths[next] = value;
                        queue.Enqueue(next, newSteps);
                    }
                }
            }
        }

        return paths.TryGetValue((rows - 1, cols - 1), out int steps) ? steps : -1;
    }

    private bool FindPath(bool[,] memory)
    {
        var directions = new List<(int, int)>()
        {
            { (-1, 0) }, { (0, 1) }, { (1, 0) }, { (0, -1) },
        };

        int rows = memory.GetLength(0);
        int cols = memory.GetLength(1);

        HashSet<(int, int)> visited = new();
        Queue<(int row, int col)> queue = new();

        queue.Enqueue((0, 0));
        while (queue.Count > 0)
        {
            var position = queue.Dequeue();
            int row = position.row;
            int col = position.col;

            if (row == rows - 1 && col == cols - 1)
            {
                return true;
            }

            if (memory[row, col] == true || !visited.Add(position))
            {
                continue;
            }

            foreach (var direction in directions)
            {
                TryEnqueue(direction);
            }

            void TryEnqueue((int, int) direction)
            {
                var nextRow = row + direction.Item1;
                var nextCol = col + direction.Item2;

                if (nextRow >= 0 && nextCol >= 0 && nextRow < rows && nextCol < cols && (memory[nextRow, nextCol] == false) && !visited.Contains((nextRow, nextCol)))
                {
                    queue.Enqueue((nextRow, nextCol));
                }
            }
        }

        return false;
    }

    public int SolutionPartOne()
    {
        var fallingBytesCoordinates = Input.Value;

        bool[,] memory = new bool[71, 71];
        for (int row = 0; row < memory.GetLength(0); row++)
        {
            for (int col = 0; col < memory.GetLength(1); col++)
            {
                memory[row, col] = true;
            }
        }

        var fallingBytesCount = 1024;
        for (int byteIdx = 0; byteIdx < fallingBytesCount; byteIdx++)
        {
            (int row, int col) = fallingBytesCoordinates[byteIdx];
            memory[row, col] = false;
        }

        var shortestPathSteps = FindShortestPath(memory);
        return shortestPathSteps;
    }

    public string SolutionPartTwo()
    {
        var fallingBytesCoordinates = Input.Value;

        var low = 0;
        var high = fallingBytesCoordinates.Count;

        var preventExitIndex = 0;
        while (true)
        {
            bool[,] memory = new bool[71, 71];
            var mid = low + (high - low) / 2;

            for (int byteIdx = 0; byteIdx < mid; byteIdx++)
            {
                (int row, int col) = fallingBytesCoordinates[byteIdx];
                memory[row, col] = true;
            }

            if (preventExitIndex == mid)
            {
                break;
            }

            if (FindPath(memory))
            {
                low = mid + 1;
            }
            else
            {
                preventExitIndex = mid;
                high = mid - 1;
            }
        }

        return $"{fallingBytesCoordinates[preventExitIndex - 1].col},{fallingBytesCoordinates[preventExitIndex - 1].row}";
    }
}
