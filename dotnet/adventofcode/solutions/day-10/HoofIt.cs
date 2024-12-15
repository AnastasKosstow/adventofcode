namespace adventofcode;

public class HoofIt : ISolution
{
    public int Day { get; } = 10;
    public string Puzzle { get; } = "Hoof It";

    private Lazy<int[,]> Input;

    private static HashSet<(int, int, int, int)> HikingTrails = [];
    private static List<(int, int, int, int)> HikingTrailsWithUniquePaths = [];

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
            var rows = lines.Length;
            var cols = lines[0].Length;

            return lines
                .SelectMany((line, row) =>
                    line.Select((ch, col) => new { Value = int.Parse(ch.ToString()), Row = row, Col = col }))
                .Aggregate(new int[rows, cols], (arr, item) =>
                {
                    arr[item.Row, item.Col] = item.Value;
                    return arr;
                });
        });
    }

    private List<int[]> FindPath(int row, int col)
    {
        var coordinates = new List<int[]>();
        int[,] map = Input.Value;
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        void DFS(int r, int c)
        {
            int currentValue = map[r, c];

            var directions = new (int rowOffset, int columnOffset)[]
            {
                (0, -1),
                (-1, 0),
                (0, 1),
                (1, 0)
            };

            foreach (var (rowOffset, columnOffset) in directions)
            {
                int nextRow = r + rowOffset;
                int nextCol = c + columnOffset;
                if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= cols) 
                    continue;

                int nextValue = map[nextRow, nextCol];
                if (nextValue == currentValue + 1)
                {
                    if (nextValue == 9)
                    {
                        coordinates.Add([nextRow, nextCol]);
                    }
                    else
                    {
                        DFS(nextRow, nextCol);
                    }
                }
            }
        }

        DFS(row, col);
        return coordinates;
    }

    private void ValidateCoordinatesForHashSet(int[] startCoordinates, List<int[]> trailCoordinates)
    {
        foreach (var coordinate in trailCoordinates)
        {
            var trail = (startCoordinates[0], startCoordinates[1], coordinate[0], coordinate[1]);
            if (!HikingTrails.Contains(trail))
                HikingTrails.Add(trail);
        }
    }

    private void ValidateCoordinatesForList(int[] startCoordinates, List<int[]> trailCoordinates)
    {
        foreach (var coordinate in trailCoordinates)
        {
            HikingTrailsWithUniquePaths.Add((startCoordinates[0], startCoordinates[1], coordinate[0], coordinate[1]));
        }
    }

    internal int SolutionPartOne()
    {
        int[,] map = Input.Value;

        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (map[row, col] == 0)
                {
                    var coordinates = FindPath(row, col);
                    ValidateCoordinatesForHashSet(new[] { row, col }, coordinates);
                }
            }
        }

        int result = HikingTrails.Count;
        return result;
    }

    internal int SolutionPartTwo()
    {
        int[,] map = Input.Value;

        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (map[row, col] == 0)
                {
                    var coordinates = FindPath(row, col);
                    ValidateCoordinatesForList(new[] { row, col }, coordinates);
                }
            }
        }

        int result = HikingTrailsWithUniquePaths.Count;
        return result;
    }
}
