using System.Collections.Generic;

namespace adventofcode;

internal class DiskFragmenter
{
    private static Lazy<int[]> Input = new(() =>
    {
        var lines = File.ReadAllLines("day-9/input.txt");
        var input = lines[0].Select(ch => int.Parse(ch.ToString())).ToArray();

        return input;
    });

    internal static void SolutionPartOne()
    {
        var map = Input.Value;

        int fileIndex = 0;
        var file = new List<string>();
        for (int idx = 0;  idx < map.Length; idx++)
        {
            if (idx % 2 == 0)
            {
                file.AddRange(Enumerable.Repeat(fileIndex.ToString(), map[idx]));
                fileIndex++;
            }
            else
            {
                file.AddRange(Enumerable.Repeat(".", map[idx]));
            }
        }

        int left = 0;
        int right = file.Count - 1;
        while (left < right)
        {
            while (left < right && file[left] != ".")
            {
                left++;
            }

            while (right > left && file[right] == ".")
            {
                right--;
            }

            if (left < right)
            {
                (file[left], file[right]) = (file[right], file[left]);
                left++;
                right--;
            }
        }

        decimal result = 0;
        for (int idx = 0; idx < file.Count; idx++)
        {
            if (file[idx] == ".")
            {
                break;
            }

            result += (idx * int.Parse(file[idx].ToString()));
        }

        // result: 6340197768906
        // benchmark: ~35 milliseconds
    }

    internal static void SolutionPartTwo()
    {
        var map = Input.Value;

        int fileIndex = 0;
        var file = new List<(string, int)>();
        for (int idx = 0; idx < map.Length; idx++)
        {
            if (idx % 2 == 0)
            {
                file.Add((fileIndex.ToString(), map[idx]));
                fileIndex++;
            }
            else
            {
                file.Add((".", map[idx]));
            }
        }

        for (int backIdx = file.Count - 1; backIdx >= 0; backIdx--)
        {
            if (file[backIdx].Item1 == ".")
            {
                continue;
            }

            for (int frontIdx = 0; frontIdx < backIdx; frontIdx++)
            {
                if (file[frontIdx].Item1 == ".")
                {
                    if (file[frontIdx].Item2 >= file[backIdx].Item2)
                    {
                        (file[frontIdx], file[backIdx]) = (file[backIdx], file[frontIdx]);
                        if (file[backIdx].Item2 - file[frontIdx].Item2 == 0)
                        {
                            file.RemoveAt(backIdx);
                        }
                        else
                        {
                            file[frontIdx] = (file[frontIdx].Item1, file[frontIdx].Item2 - file[backIdx].Item2);
                        }

                        break;
                    }
                }
            }
        }

        decimal result = 0;
        for (int idx = 0; idx < file.Count; idx++)
        {
            if (file[idx].Item1 == ".")
            {
                continue;
            }

            result += (idx * int.Parse(file[idx].Item1.ToString()));
        }

        // result: 
        // benchmark: ~35 milliseconds
    }
}
