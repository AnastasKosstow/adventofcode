namespace adventofcode;

public class GardenGroups : ISolution
{
    private class Region
    {
        public char Type { get; set; }
        public int Size { get; set; } = 0;
        public int Cost { get; set; } = 0;
        public int Edges { get; set; } = 0;
        public List<(int row, int col)> PlotsCoordinates { get; set; } = [];
    }

    public int Day { get; } = 12;
    public string Puzzle { get; } = "Garden Groups";

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
            var rows = lines.Length;
            var cols = lines[0].Length;

            return lines
                .SelectMany((line, row) =>
                    line.Select((ch, col) => new { Value = ch, Row = row, Col = col }))
                .Aggregate(new char[rows, cols], (arr, item) =>
                {
                    arr[item.Row, item.Col] = item.Value;
                    return arr;
                });
        });
    }

    private List<Region> FindRegions(char[,] plots, bool countEdges = false)
    {
        var regions = new List<Region>();
        int rows = plots.GetLength(0);
        int cols = plots.GetLength(1);

        var directions = new (int rowOffset, int columnOffset)[]
        {
            (0, -1),
            (-1, 0),
            (0, 1),
            (1, 0)
        };

        var visited = new bool[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (visited[row, col] != true)
                {
                    var region = new Region()
                    {
                        Type = plots[row, col],
                        Size = 1,
                        Cost = 0,
                        PlotsCoordinates = [(row, col)]
                    };

                    DFS(row, col, region);

                    if (region != null && region.Size > 0)
                    {
                        if (countEdges)
                        {
                            CalculateRegionEdges(region);
                        }

                        regions.Add(region);
                    }
                }
            }
        }

        void DFS(int r, int c, Region region)
        {
            char plot = plots[r, c];
            visited[r, c] = true;

            foreach (var (rowOffset, columnOffset) in directions)
            {
                int nextRow = r + rowOffset;
                int nextCol = c + columnOffset;
                if (nextRow < 0 || nextRow >= rows || nextCol < 0 || nextCol >= cols)
                {
                    region.Cost++;
                    continue;
                }

                if (visited[nextRow, nextCol] == true)
                {
                    if (plots[nextRow, nextCol] != plot)
                    {
                        region.Cost++; ;
                    }
                    continue;
                }

                if (plots[nextRow, nextCol] == plot)
                {
                    region.Size++;
                    region.PlotsCoordinates.Add((nextRow, nextCol));
                    DFS(nextRow, nextCol, region);
                }
                else
                {
                    region.Cost++;
                }
            }
        }

        void CalculateRegionEdges(Region region)
        {
            foreach (var (row, col) in region.PlotsCoordinates)
            {
                if (!HasSamePlot(row - 1, col) && !HasSamePlot(row, col - 1)) region.Edges++;
                if (!HasSamePlot(row - 1, col) && !HasSamePlot(row, col + 1)) region.Edges++;
                if (!HasSamePlot(row + 1, col) && !HasSamePlot(row, col - 1)) region.Edges++;
                if (!HasSamePlot(row + 1, col) && !HasSamePlot(row, col + 1)) region.Edges++;

                if (HasSamePlot(row - 1, col) && HasSamePlot(row, col - 1) && !HasSamePlot(row - 1, col - 1)) region.Edges++;
                if (HasSamePlot(row - 1, col) && HasSamePlot(row, col + 1) && !HasSamePlot(row - 1, col + 1)) region.Edges++;
                if (HasSamePlot(row + 1, col) && HasSamePlot(row, col + 1) && !HasSamePlot(row + 1, col + 1)) region.Edges++;
                if (HasSamePlot(row + 1, col) && HasSamePlot(row, col - 1) && !HasSamePlot(row + 1, col - 1)) region.Edges++;
            }

            bool HasSamePlot(int row, int col)
            {
                if (row < 0 || row >= rows || col < 0 || col >= cols)
                {
                    return false;
                }

                return plots[row, col] == region.Type && region.PlotsCoordinates.Contains((row, col));
            }
        }

        return regions;
    }

    internal int SolutionPartOne()
    {
        var plots = Input.Value;

        var regions = FindRegions(plots);

        int result = 0;
        foreach (var region in regions)
        {
            result += region.Cost * region.Size;
        }

        return result;
    }

    internal int SolutionPartTwo()
    {
        var plots = Input.Value;

        var regions = FindRegions(plots, true);

        int result = 0;
        foreach (var region in regions)
        {
            result += region.Edges * region.Size;
        }

        return result;
    }
}
