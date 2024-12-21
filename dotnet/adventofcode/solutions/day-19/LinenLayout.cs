using System.Numerics;

namespace adventofcode;

public class LinenLayout : ISolution
{
    public int Day => 19;
    public string Puzzle => "Linen Layout";

    private HashSet<string> TowelPatterns;
    private List<string> Designs;
    
    public (string partOne, string partTwo) Execute()
    {
        var partOne = SolutionPartOne();
        var partTwo = SolutionPartTwo();
        return (partOne.ToString(), partTwo.ToString());
    }

    public void SetInput(string inputSource)
    {
        var lines = File.ReadAllLines(inputSource);

        TowelPatterns = [.. lines[0].Split(", ")];
        Designs = lines.Skip(2).ToList();
    }

    private bool FormDesign(string target, HashSet<string> cache)
    {
        if (cache.Contains(target))
        {
            return true;
        }

        if (string.IsNullOrEmpty(target))
        {
            return true;
        }

        for (int idx = 1; idx <= target.Length; idx++)
        {
            string prefix = target[..idx];
            if (TowelPatterns.Contains(prefix))
            {
                string remaining = target[idx..];
                if (FormDesign(remaining, cache))
                {
                    return true;
                }
            }
        }

        cache.Add(target);
        return false;
    }

    private void FormDesignCombinations(string target, Dictionary<string, BigInteger> cache,  ref BigInteger patterns)
    {
        if (cache.TryGetValue(target, out BigInteger value))
        {
            patterns += value;
            return;
        }

        if (string.IsNullOrEmpty(target))
        {
            patterns++;
            return;
        }

        var currentPatternsCount = patterns;
        for (int idx = 1; idx <= target.Length; idx++)
        {
            string prefix = target[..idx];
            if (TowelPatterns.Contains(prefix))
            {
                string remaining = target[idx..];
                FormDesignCombinations(remaining, cache, ref patterns);
            }
        }

        cache[target] = patterns - currentPatternsCount;
    }

    private int SolutionPartOne()
    {
        var result = 0;
        foreach (var design in Designs)
        {
            if (FormDesign(design, new HashSet<string>()))
            {
                result++;
            }
        }

        return result;
    }

    private BigInteger SolutionPartTwo()
    {
        List<BigInteger> patterns = [];
        foreach (var design in Designs)
        {
            BigInteger designPatterns = 0;
            FormDesignCombinations(design, new Dictionary<string, BigInteger>(), ref designPatterns);

            if (designPatterns > 0)
            {
                patterns.Add(designPatterns);
            }
        }

        BigInteger result = 0;
        foreach (var patternCount in patterns)
        {
            result += patternCount;
        }

        return result;
    }
}
