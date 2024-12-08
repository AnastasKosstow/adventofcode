namespace adventofcode;

internal class GuardGallivant
{
    private static Lazy<char[][]> Input = new(() =>
    {
        var lines = File.ReadAllLines("day-6/input.txt");
        var input = lines.Select(line => line.ToCharArray()).ToArray();

        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < input[row].Length; col++)
            {
                input[row][col] = lines[row][col];
            }
        }

        return input;
    });

    private static readonly int[][] Moves = new int[][]
    {
        [ -1, 0 ],  // up (^)
        [ 0, 1 ],   // right (>)
        [ 1, 0 ],   // down (v)
        [ 0, -1 ],  // left (<)
    };

    private readonly static char[] Directions = ['^', '>', 'v', '<'];
    private readonly static Dictionary<char, int[]> MoveDirections = new()
    {
        { Directions[0], new[] { -1, 0 } }, // ^
        { Directions[1], new[] { 0, 1 } },  // >
        { Directions[2], new[] { 1, 0 } },  // v
        { Directions[3], new[] { 0, -1 } }, // <
    };

    private static int[] GetStartingPosition()
    {
        var grid = Input.Value;

        var startPosition = new int[2];
        for (int row = 0; row < grid.Length; row++)
        {
            bool found = false;
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == '^')
                {
                    startPosition = [row, col];
                    found = true;
                    break;
                }
            }
            if (found) break;
        }

        return startPosition;
    }

    internal static void SolutionPartOne()
    {
        var grid = Input.Value;
        var result = 0;

        var direction = '^';
        var position = GetStartingPosition();
        var visited = new bool[grid.Length, grid[0].Length];

        var moveDirection = MoveDirections[direction];
        while (position[0] < grid.Length && position[1] < grid.Length && position[0] >= 0 && position[1] >= 0)
        {
            if (grid[position[0]][position[1]] == '#')
            {
                position[0] += moveDirection[0] * -1;
                position[1] += moveDirection[1] * -1;

                var nextDirectionIndex = (Array.IndexOf(Directions, direction) + 1) % 4;
                direction = Directions[nextDirectionIndex];
                moveDirection = MoveDirections[direction];
            }
            else
            {
                if (!visited[position[0], position[1]])
                {
                    visited[position[0], position[1]] = true;
                    result++;
                }
            }

            position[0] += moveDirection[0];
            position[1] += moveDirection[1];
        }

        // result: 5269
    }

    internal static void SolutionPartTwo()
    {
        var grid = Input.Value;
        var result = 0;

        var direction = '^';
        var position = GetStartingPosition();

        var moveDirection = MoveDirections[direction];
        while (true)
        {
            var nextRow = position[0] + moveDirection[0];
            var nextCol = position[1] + moveDirection[1];

            if (nextRow < 0 || nextCol < 0 || nextRow >= grid.Length || nextCol >= grid[nextRow].Length)
            {
                break;
            }

            if (grid[nextRow][nextCol] == '#')
            {
                var nextDirectionIndex = (Array.IndexOf(Directions, direction) + 1) % 4;
                direction = Directions[nextDirectionIndex];
                moveDirection = MoveDirections[direction];
            }
            else
            {
                grid[nextRow][nextCol] = '#';
                bool found = SearchForLoops(grid, (int[])position.Clone(), direction);
                if (found)
                {
                    result++;
                }

                grid[nextRow][nextCol] = '.';
                position[0] = nextRow;
                position[1] = nextCol;
            }
        }

        // result: 2198
    }

    private static bool SearchForLoops(char[][] grid, int[] position, char direction)
    {
        var moveDirection = MoveDirections[direction];
        bool[,,] visited = new bool[grid.Length, grid[0].Length, 4];

        while (true)
        {
            var nextRow = position[0] + moveDirection[0];
            var nextCol = position[1] + moveDirection[1];

            if (nextRow < 0 || nextCol < 0 || nextRow >= grid.Length || nextCol >= grid[nextRow].Length)
            {
                break;
            }

            if (grid[nextRow][nextCol] == '#')
            {
                var nextDirectionIndex = (Array.IndexOf(Directions, direction) + 1) % 4;
                direction = Directions[nextDirectionIndex];
                moveDirection = MoveDirections[direction];
            }
            else
            {
                var currentDirection = Array.IndexOf(Directions, direction);
                if (visited[nextRow, nextCol, currentDirection])
                {
                    return true;
                }

                visited[nextRow, nextCol, currentDirection] = true;
                position[0] = nextRow;
                position[1] = nextCol;
            }
        }

        return false;
    }
}
