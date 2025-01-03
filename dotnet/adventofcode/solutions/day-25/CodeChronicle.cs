namespace adventofcode;

public static class Extensions
{
    public static (List<char[,]> Locks, List<char[,]> Keys) Partition(this IEnumerable<char[,]> source, Func<char[,], bool> predicate)
    {
        var (locks, keys) = source.Aggregate((Locks: new List<char[,]>(), Keys: new List<char[,]>()), (acc, item) =>
        {
            (predicate(item) ? acc.Locks : acc.Keys).Add(item);
            return acc;
        });

        return (locks, keys);
    }

    public static IEnumerable<char[,]> ConvertToMatrices(this IEnumerable<string> source)
    {
        var matrices = source.Select(pattern => 
        {
            var rows = pattern.Split("\r\n");
            var matrix = new char[rows.Length, rows[0].Length];

            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < rows[row].Length; col++)
                {
                    matrix[row, col] = rows[row][col];
                }
            }
            return matrix;
        });

        return matrices;
    }

    public static int[] GetHeights(this char[,] matrix, bool isLock)
    {
        int cols = matrix.GetLength(1);
        int rows = matrix.GetLength(0);
        var heights = new int[cols];

        for (int col = 0; col < cols; col++)
        {
            if (isLock)
            {
                for (int row = 1; row < rows; row++)
                {
                    if (matrix[row, col] != '#')
                    {
                        heights[col] = row - 1;
                        break;
                    }
                }
            }
            else
            {
                int height = 0;
                int row = rows - 2;
                while (true)
                {
                    if (row == 0 || matrix[row, col] == '.')
                    {
                        break;
                    }

                    height++;
                    row--;
                }
                heights[col] = height;
            }
        }
        return heights;
    }
}

public class CodeChronicle : ExecutionMeasure, ISolution
{
    public int Day => 25;
    public string Puzzle => "Code Chronicle";

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        var resultPartOne = SolutionPartOne();
        var resultPartTwo = SolutionPartTwo();
        var millisecondsPartOne = MeasureExecutionTime(SolutionPartOne);
        var millisecondsPartTwo = MeasureExecutionTime(SolutionPartTwo);

        return ((resultPartOne.ToString(), millisecondsPartOne), (resultPartTwo.ToString(), millisecondsPartTwo));
    }

    private List<char[,]> Locks = [];
    private List<char[,]> Keys = [];

    public void SetInput(string inputSource)
    {
        (Locks, Keys) = File.ReadAllText(inputSource)
            .Split("\r\n\r\n")
            .ConvertToMatrices()
            .Partition(matrix => matrix[0, 1] == '#');
    }

    private bool IsMatch(int[] lockHeights, int[] keyHeights)
    {
        for (int idx = 0; idx < lockHeights.Length; idx++)
        {
            if (lockHeights[idx] + keyHeights[idx] > 5)
            {
                return false;
            }
        }
        return true;
    }

    private int SolutionPartOne()
    {
        var lockHeights = Locks.Select(@lock => @lock.GetHeights(true)).ToList();
        var keyHeights = Keys.Select(key => key.GetHeights(false)).ToList();

        int result = 0;
        for (int @lock = 0; @lock < lockHeights.Count; @lock++)
        {
            for (int key = 0; key < keyHeights.Count; key++)
            {
                if (IsMatch(lockHeights[@lock], keyHeights[key]))
                    result++;
            }
        }

        return result;
    }

    private string SolutionPartTwo()
    {
        return "/";
    }
}
