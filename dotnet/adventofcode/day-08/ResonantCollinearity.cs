internal class ResonantCollinearity
{
    private static Lazy<string[]> Input = new(() =>
    {
        var lines = File.ReadAllLines("day-08/input.txt");
        var input = lines.Select(line => line).ToArray();

        return input;
    });

    private readonly record struct Point(int Row, int Col);
    private readonly record struct Vector(int DeltaRow, int DeltaCol)
    {
        public static Vector FromPoints(Point x, Point y)
        {
            return new(y.Row - x.Row, y.Col - x.Col);
        }
        
        public Point Apply(Point start)
        {
            return new(start.Row + DeltaRow, start.Col + DeltaCol);
        }
    }

    private static Dictionary<char, List<Point>> FindAllFrequencies()
    {
        var grid = Input.Value;
        var frequencies = new Dictionary<char, List<Point>>();

        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                if (grid[row][col] == '.')
                {
                    continue;
                }

                if (!frequencies.TryGetValue(grid[row][col], out var points))
                {
                    points = [];
                    frequencies[grid[row][col]] = points;
                }

                points.Add(new Point(row, col));
            }
        }
        return frequencies;
    }

    internal static void SolutionPartOne()
    {
        var grid = Input.Value;

        var frequencies = FindAllFrequencies();

        var antinodes = new HashSet<(int, int)>();
        foreach (var frequency in frequencies)
        {
            var antennaFrequencies = frequency.Value;
            for (int idx = 0; idx < antennaFrequencies.Count; idx++)
            {
                Point coordinates = antennaFrequencies[idx];

                for (int innerIdx = idx + 1; innerIdx < antennaFrequencies.Count; innerIdx++)
                {
                    Point pairCoordinates = antennaFrequencies[innerIdx];

                    FindAntinodes(coordinates, pairCoordinates);
                    FindAntinodes(pairCoordinates, coordinates);

                    void FindAntinodes(Point basePoint, Point reflectionPoint)
                    {
                        var vector = Vector.FromPoints(reflectionPoint, basePoint);
                        var antinode = vector.Apply(basePoint);

                        if (antinode.Row >= 0 && antinode.Row < grid.Length && antinode.Col >= 0 && antinode.Col < grid[0].Length)
                        {
                            antinodes.Add((antinode.Row, antinode.Col));
                        }
                    }
                }
            }
        }

        var result = antinodes.Count;

        // result: 273
    }

    internal static void SolutionPartTwo()
    {
        var grid = Input.Value;

        var frequencies = FindAllFrequencies();

        var antinodes = new HashSet<Point>();
        foreach (var frequency in frequencies)
        {
            var antennaFrequencies = frequency.Value;
            for (int idx = 0; idx < antennaFrequencies.Count; idx++)
            {
                Point coordinates = antennaFrequencies[idx];
                antinodes.Add(coordinates);

                for (int innerIdx = idx + 1; innerIdx < antennaFrequencies.Count; innerIdx++)
                {
                    Point pairCoordinates = antennaFrequencies[innerIdx];

                    FindAntinodes(coordinates, pairCoordinates);
                    FindAntinodes(pairCoordinates, coordinates);

                    void FindAntinodes(Point basePoint, Point reflectionPoint)
                    {
                        var vector = Vector.FromPoints(reflectionPoint, basePoint);
                        var current = basePoint;

                        while (true)
                        {
                            current = vector.Apply(current);

                            if (current.Row < 0 || current.Row >= grid.Length || current.Col < 0 || current.Col >= grid[0].Length)
                            {
                                break;
                            }

                            antinodes.Add(new Point(current.Row, current.Col));
                        }
                    }
                }
            }
        }

        var result = antinodes.Count;

        // result: 1017
    }
}
