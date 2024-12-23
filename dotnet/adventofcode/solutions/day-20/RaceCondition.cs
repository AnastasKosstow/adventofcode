using System.Numerics;

namespace adventofcode;

public class RaceCondition : ExecutionMeasure, ISolution
{
    public int Day => 20;
    public string Puzzle => "Race Condition";

    private int Width;
    private int Height;
    private Vector2 Start;
    private Vector2 End;
    private HashSet<Vector2> Walls = [];

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
        var lines = File.ReadAllLines(inputSource);

        Height = lines.Length;
        Width = lines[0].Length;

        for (int row = 0; row < lines.Length; row++)
        {
            for (int col = 0; col < lines[row].Length; col++)
            {
                if (lines[row][col] == 'S')
                {
                    Start = new Vector2(row, col);
                }
                else if (lines[row][col] == 'E')
                {
                    End = new Vector2(row, col);
                }
                else if (lines[row][col] == '#')
                {
                    Walls.Add(new Vector2(row, col));
                }
            }
        }
    }

    private List<Vector2> Race(Vector2 start, Vector2 end)
    {
        var directions = new Vector2[4] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1), };

        Vector2 current = start;
        List<Vector2> racetrack = [current];

        while (current != end)
        {
            foreach (Vector2 dir in directions)
            {
                Vector2 nextNode = current + dir;
                if (Valid(nextNode) && !racetrack.Contains(nextNode) && !Walls.Contains(nextNode))
                {
                    current = nextNode;
                    racetrack.Add(nextNode);
                    break;
                }
            }
        }

        return racetrack;
    }

    private int FindCheats(List<Vector2> racetrack,int minSaveDistance, int rule)
    {
        int cheats = 0;
        for (int row = 0; row < racetrack.Count; row++)
        {
            for (int col = row + 1; col < racetrack.Count; col++)
            {
                int distance = ManhattanDistance(racetrack[row], racetrack[col]);
                if (distance > rule)
                {
                    continue;
                }

                int saveDistance = col - row - distance;
                if (saveDistance >= minSaveDistance)
                {
                    cheats++;
                }
            }
        }

        return cheats;
    }

    private bool Valid(Vector2 node)
    {
        int x = (int)node.X;
        int y = (int)node.Y;
        return x >= 0 && x <= Width && y >= 0 && y <= Height;
    }

    private int ManhattanDistance(Vector2 start, Vector2 end)
    {
        return (int)Math.Abs(start.X - end.X) + (int)Math.Abs(start.Y - end.Y);
    }

    private int SolutionPartOne()
    {
        var rule = 2;
        List<Vector2> racetrack = Race(Start, End);

        var result = FindCheats(racetrack, 100, rule);
        return result;
    }

    private int SolutionPartTwo()
    {
        var rule = 20;
        List<Vector2> racetrack = Race(Start, End);

        var result = FindCheats(racetrack, 100, rule);
        return result;
    }
}
