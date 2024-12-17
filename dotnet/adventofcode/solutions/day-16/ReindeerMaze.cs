namespace adventofcode.solutions;

public enum Direction
{
    North,
    East,
    South,
    West
}

public class PathStep(int row, int col, Direction facing)
{
    public int Row { get; set; } = row;
    public int Col { get; set; } = col;
    public Direction Facing { get; set; } = facing;
}

public class State
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int Score { get; set; }
    public Direction Facing { get; set; }
    public List<PathStep> Path { get; set; }

    public State(int row, int col, int score, Direction facing, List<PathStep> previousPath = null)
    {
        Row = row;
        Col = col;
        Score = score;
        Facing = facing;
        Path = previousPath?.ToList() ?? new List<PathStep>();
        Path.Add(new PathStep(row, col, facing));
    }

    public override bool Equals(object obj)
    {
        if (obj is State other)
        {
            return Row == other.Row && Col == other.Col && Facing == other.Facing;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col, Facing);
    }
}

public class ReindeerMaze : ISolution
{
    public int Day => 16;
    public string Puzzle => "Reindeer Maze";

    private Lazy<char[,]> Input;

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

            char[,] maze = new char[lines.Length, lines[0].Length];
            for (int row = 0; row < lines.Length; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    maze[row, col] = lines[row][col];
                }
            }

            return maze;
        });
    }

    private Dictionary<State, List<(int Score, List<PathStep> Path)>> FindShortestPaths(char[,] maze)
    {
        var directionsIndices = new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West };
        var directions = new Dictionary<Direction, (int, int)>()
        {
            { directionsIndices[0], (-1, 0) },
            { directionsIndices[1], (0, 1) },
            { directionsIndices[2], (1, 0) },
            { directionsIndices[3], (0, -1) },
        };

        int rows = maze.GetLength(0);
        int cols = maze.GetLength(1);

        Dictionary<State, List<(int Score, List<PathStep> Path)>> paths = [];
        PriorityQueue<State, int> queue = new();

        var startingPosition = new State(maze.GetLength(0) - 2, 1, 0, Direction.East);
        queue.Enqueue(startingPosition, startingPosition.Score);

        while (queue.Count > 0)
        {
            var state = queue.Dequeue();
            int row = state.Row;
            int col = state.Col;

            if (maze[row, col] == '#' || maze[row, col] == 'E')
            {
                continue;
            }

            if (!paths.ContainsKey(state))
            {
                paths.Add(state, new List<(int Score, List<PathStep> Path)>());
            }

            int directionIndex = Array.IndexOf(directionsIndices, state.Facing);
            var leftDirectionIndex = (directionIndex - 1 + directionsIndices.Length) % directionsIndices.Length;
            var rightDirectionIndex = (directionIndex + 1) % directionsIndices.Length;

            TryEnqueue(leftDirectionIndex, state.Score + 1001, state.Path);
            TryEnqueue(directionIndex, state.Score + 1, state.Path);
            TryEnqueue(rightDirectionIndex, state.Score + 1001, state.Path);

            void TryEnqueue(int directionIndex, int score, List<PathStep> currentPath)
            {
                var direction = directionsIndices[directionIndex];
                var nextRow = row + directions[direction].Item1;
                var nextCol = col + directions[direction].Item2;

                if (nextRow >= 0 && nextCol >= 0 && nextRow < rows && nextCol < cols && (maze[nextRow, nextCol] == '.' || maze[nextRow, nextCol] == 'E'))
                {
                    var nextState = new State(nextRow, nextCol, score, direction, currentPath);

                    var minScore = paths.TryGetValue(nextState, out var existingPaths) && existingPaths.Count != 0
                        ? existingPaths.Min(p => p.Score)
                        : int.MaxValue;

                    if (nextState.Score <= minScore)
                    {
                        if (nextState.Score < minScore && existingPaths != null)
                        {
                            existingPaths.Clear();
                        }

                        if (!paths.TryGetValue(nextState, out List<(int Score, List<PathStep> Path)> value))
                        {
                            value = ([]);
                            paths[nextState] = value;
                        }

                        value.Add((nextState.Score, nextState.Path));
                        queue.Enqueue(nextState, nextState.Score);
                    }
                }
            }
        }

        return paths;
    }

    private int SolutionPartOne()
    {
        char[,] maze = Input.Value;

        var paths = FindShortestPaths(maze);

        var endPaths = paths.Where(p => p.Key.Row == 1 && p.Key.Col == maze.GetLength(1) - 2)
                            .SelectMany(p => p.Value)
                            .ToList();

        var result = endPaths.Min(p => p.Score);
        return result;
    }

    private int SolutionPartTwo()
    {
        char[,] maze = Input.Value;
        var paths = FindShortestPaths(maze);

        var endPaths = paths.Where(p => p.Key.Row == 1 && p.Key.Col == maze.GetLength(1) - 2)
                            .SelectMany(p => p.Value)
                            .ToList();

        var minScore = endPaths.Min(p => p.Score);
        var minScorePaths = endPaths.Where(p => p.Score == minScore);

        var uniqueTiles = new HashSet<(int Row, int Col)>();
        foreach (var path in minScorePaths)
        {
            foreach (var step in path.Path)
            {
                uniqueTiles.Add((step.Row, step.Col));
            }
        }

        return uniqueTiles.Count;
    }
}
