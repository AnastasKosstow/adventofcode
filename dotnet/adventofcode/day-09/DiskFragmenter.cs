namespace adventofcode;

internal class DiskFragmenter
{
    private static Lazy<int[]> Input = new(() =>
    {
        var lines = File.ReadAllLines("day-09/input.txt");
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

            result += idx * int.Parse(file[idx]);
        }

        // result: 6340197768906
    }

    internal static void SolutionPartTwo()
    {
        var map = Input.Value;

        int fileIndex = 0;
        var file = new List<Block>();
        for (int idx = 0; idx < map.Length; idx++)
        {
            if (idx % 2 == 0)
            {
                file.Add(new Block(fileIndex.ToString(), map[idx]));
                fileIndex++;
            }
            else
            {
                file.Add(new Block(".", map[idx]));
            }
        }

        for (int backIdx = file.Count - 1; backIdx >= 0; backIdx--)
        {
            if (file[backIdx].Symbol == ".")
            {
                continue;
            }

            for (int frontIdx = 0; frontIdx < backIdx; frontIdx++)
            {
                if (file[frontIdx].Symbol == ".")
                {
                    if (file[frontIdx].Count >= file[backIdx].Count)
                    {
                        var diff = file[frontIdx].Count - file[backIdx].Count;
                        if (diff == 0)
                        {
                            (file[frontIdx], file[backIdx]) = (file[backIdx], file[frontIdx]);
                        }
                        else
                        {
                            var item = file[backIdx];
                            file[backIdx] = new Block(".", file[backIdx].Count);
                            file[frontIdx] = new Block(".", diff);

                            file.Insert(frontIdx, item);
                        }

                        break;
                    }
                }
            }
        }

        decimal result = 0;
        int multiplier = 0;
        for (int idx = 0; idx < file.Count; idx++)
        {
            for (int countIdx = 0; countIdx < file[idx].Count; countIdx++)
            {
                if (file[idx].Symbol != ".")
                {
                    result += multiplier * int.Parse(file[idx].Symbol);
                }

                multiplier++;
            }
        }

        // result: 6363913128533
    }

    private class Block(string symbol, int count)
    {
        internal string Symbol { get; } = symbol;
        internal int Count { get; set; } = count;
    }
}
