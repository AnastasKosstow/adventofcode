namespace adventofcode;

public class RedNosedReports : ExecutionMeasure, ISolution
{
    public int Day { get; } = 2;
    public string Puzzle { get; } = "Red-Nosed Reports";

    private Lazy<Memory<int[]>> Input;

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
        Input = new(() =>
        {
            var lines = File.ReadAllLines(inputSource);
            var reports = new int[lines.Length][];

            for (int idx = 0; idx < lines.Length; idx++)
            {
                reports[idx] = lines[idx]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
            }

            return reports.AsMemory();
        });
    }

    private bool IsSafe(ReadOnlySpan<int> levels)
    {
        int firstDiff = levels[1] - levels[0];

        bool valid = firstDiff != 0 && (Math.Abs(firstDiff) | 3) == 3; //  | is bitwise OR
        if (!valid) 
            return false;

        /*
           In C#, integers are 32-bit, so bit 31 is the sign bit
           Right shifting by 31 moves the sign bit to the rightmost position
           For positive numbers: gives 0 (all bits become 0)
           For negative numbers: gives - 1 (all bits become 1)
        */
        int directionMask = firstDiff >> 31;
        
        for (int idx = 1; idx < levels.Length - 1; idx++)
        {
            int diff = levels[idx + 1] - levels[idx];

            /*
               If first diff was positive, directionMask is 0, so all diffs must give 0 (positive)
               If first diff was negative, directionMask is -1, so all diffs must give -1 (negative)
            */

            valid &= diff != 0 // Ensures numbers are changing
                 && (Math.Abs(diff) | 3) == 3 // Same check for 1-3 range
                 && ((diff >> 31) == directionMask); // Ensures same direction as first difference

            if (!valid) return false;
        }

        return true;
    }

    internal int SolutionPartOne()
    {
        var reports = Input.Value.Span;
        int safeCount = 0;

        for (int idx = 0; idx < reports.Length; idx++)
        {
            if (IsSafe(reports[idx]))
                safeCount++;
        }

        return safeCount;
    }

    internal int SolutionPartTwo()
    {
        var reports = Input.Value.Span;
        int safeCount = 0;

        Span<int> buffer = stackalloc int[10];

        for (int idx = 0; idx < reports.Length; idx++)
        {
            var report = reports[idx];

            if (IsSafe(report))
            {
                safeCount++;
                continue;
            }

            bool foundSafe = false;
            for (int skipIdx = 0; skipIdx < report.Length && !foundSafe; skipIdx++)
            {
                int bufferIdx = 0;
                for (int i = 0; i < report.Length; i++)
                {
                    if (i != skipIdx)
                        buffer[bufferIdx++] = report[i];
                }

                if (IsSafe(buffer[..bufferIdx]))
                {
                    foundSafe = true;
                    safeCount++;
                }
            }
        }

        return safeCount;
    }
}
