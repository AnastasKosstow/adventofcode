using System.Data;

namespace adventofcode;

public class RaceCondition : ISolution
{
    public int Day => 20;
    public string Puzzle => "Race Condition";

    private Lazy<(char[,] racetrack, int[] start)> Input;

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

            char[,] racetrack = new char[lines.Length, lines[0].Length];
            int[] start = new int[2];
            int[] end = new int[2];

            for (int row = 0; row < lines.Length; row++)
            {
                for (int col = 0; col < lines[row].Length; col++)
                {
                    racetrack[row, col] = lines[row][col];
                    if (lines[row][col] == 'S')
                    {
                        start = [row, col];
                    }
                }
            }

            return (racetrack, start);
        });
    }

    private Dictionary<(int, int), int> GetPositionPicoseconds()
    {
        char[,] racetrack = Input.Value.racetrack;
        int row = Input.Value.start[0];
        int col = Input.Value.start[1];

        var directions = new List<(int r, int c)>()
        {
            { (-1, 0) }, { (0, 1) }, { (1, 0) }, { (0, -1) },
        };

        var visited = new bool[racetrack.GetLength(0), racetrack.GetLength(1)];

        int picoseconds = 0;
        var positionPicoseconds = new Dictionary<(int, int), int>()
        {
            { (row, col), 0 }
        };
        while (row >= 0 && col >= 0 && row < racetrack.GetLength(0) && col < racetrack.GetLength(1))
        {
            if (racetrack[row, col] == 'E')
            {
                break;
            }

            visited[row, col] = true;
            foreach (var (directionRow, directionCol) in directions)
            {
                if ((racetrack[row + directionRow, col + directionCol] == '.' || racetrack[row + directionRow, col + directionCol] == 'E') && visited[row + directionRow, col + directionCol] == false)
                {
                    row += directionRow;
                    col += directionCol;

                    positionPicoseconds[(row, col)] = ++picoseconds;
                    break;
                }
            }
        }

        return positionPicoseconds;
    }

    private Dictionary<int, int> GetAllCheatPicoseconds(Dictionary<(int, int), int> positionPicoseconds, int rule)
    {
        var racetrack = Input.Value.racetrack;

        var directions = new List<(int r, int c)>()
        {
            { (-1, 0) }, { (0, 1) }, { (1, 0) }, { (0, -1) },
        };

        var cheatsPicoseconds = new Dictionary<int, int>();
        var cheats = new Dictionary<(int, int), Dictionary<(int, int), int>>();

        var visited = new bool[racetrack.GetLength(0), racetrack.GetLength(1)];
        foreach (var position in positionPicoseconds)
        {
            var row = position.Key.Item1;
            var col = position.Key.Item2;

            visited[row, col] = true;
            foreach (var (directionRow, directionCol) in directions)
            {
                var nextRow = row + directionRow;
                var nextCol = col + directionCol;
                if (nextRow >= 0 && nextCol >= 0 && nextRow < racetrack.GetLength(0) && nextCol < racetrack.GetLength(1))
                {
                    if (racetrack[nextRow, nextCol] == '#')
                    {
                        FindShortPath(row + directionRow, col + directionCol, position.Key, 1, []);
                    }
                }
            }
        }

        void FindShortPath(int row, int col, (int, int) startCoordinates, int step, List<(int, int)> path)
        {
            if (visited[row, col] == true || step > rule)
            {
                return;
            }

            path.Add((row, col));
            foreach (var (directionRow, directionCol) in directions)
            {
                var nextRow = row + directionRow;
                var nextCol = col + directionCol;
                step++;

                if (nextRow >= 0 && nextCol >= 0 && nextRow < racetrack.GetLength(0) && nextCol < racetrack.GetLength(1))
                {
                    if (path.Contains((nextRow, nextCol)))
                    {
                        step--;
                        continue;
                    }

                    if (racetrack[nextRow, nextCol] == '#')
                    {
                        FindShortPath(row + directionRow, col + directionCol, startCoordinates, step, path);
                    }

                    else if (racetrack[nextRow, nextCol] == '.' || racetrack[nextRow, nextCol] == 'E')
                    {
                        if (positionPicoseconds[(startCoordinates.Item1, startCoordinates.Item2)] + step >= positionPicoseconds[(nextRow, nextCol)])
                        {
                            step--;
                            continue;
                        }

                        var picoseconds = positionPicoseconds[(nextRow, nextCol)] -
                            positionPicoseconds[(startCoordinates.Item1, startCoordinates.Item2)] - step;

                        if (!cheats.TryGetValue(startCoordinates, out var values))
                        {
                            values = [];
                            cheats[startCoordinates] = values;
                        }


                        if (values.TryAdd((nextRow, nextCol), picoseconds))
                        {
                            if (!cheatsPicoseconds.TryGetValue(picoseconds, out int cheatsPicosecondsValues))
                            {
                                cheatsPicosecondsValues = 0;
                                cheatsPicoseconds[picoseconds] = cheatsPicosecondsValues;
                            }

                            cheatsPicoseconds[picoseconds] = ++cheatsPicosecondsValues;
                        }
                    }
                }

                step--;
            }
        }

        return cheatsPicoseconds;
    }

    private int SolutionPartOne()
    {
        var positionPicoseconds = GetPositionPicoseconds();
        var rule = 1;

        var cheats = GetAllCheatPicoseconds(positionPicoseconds, rule);

        var result = 0;
        foreach (var cheat in cheats)
        {
            if (cheat.Key >= 100)
            {
                result += cheat.Value;
            }
        }

        return result;
    }

    private int SolutionPartTwo()
    {
        var positionPicoseconds = GetPositionPicoseconds();
        var rule = 20;

        var cheats = GetAllCheatPicoseconds(positionPicoseconds, rule);

        var result = 0;
        foreach (var cheat in cheats)
        {
            if (cheat.Key >= 100)
            {
                result += cheat.Value;
            }
        }

        return result;
    }
}
