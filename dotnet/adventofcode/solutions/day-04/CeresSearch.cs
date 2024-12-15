namespace adventofcode;

public class CeresSearch : ISolution
{
    public int Day { get; } = 4;
    public string Puzzle { get; } = "Ceres Search";

    private const string SEARCH_WORD_TERM = "XMAS";
    private const string SEARCH_PATTERN_TERM = "MS";

    private Lazy<Memory<string>> Input;

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
            var input = new string[lines.Length];

            for (int idx = 0; idx < lines.Length; idx++)
            {
                input[idx] = lines[idx];
            }

            return input.AsMemory();
        });
    }

    private bool WordSearch(int row, int col, int rowStep, int colStep) // XMAS
    {
        Span<string> input = Input.Value.Span;

        var index = 1;
        while (index < 4)
        {
            row += rowStep;
            col += colStep;

            if (row < 0 || col < 0 || input.Length <= row || input[row].Length <= col)
                return false;

            if (SEARCH_WORD_TERM[index] != input[row][col])
                return false;
            
            index++;
        }
        return true;
    }

    private bool PatternSearch(int row, int col, int rowStep, int colStep) // X-MAS
    {
        Span<string> input = Input.Value.Span;

        if (row + rowStep < 0 || col + colStep < 0 || input.Length <= row + rowStep || input[row].Length <= col + colStep)
            return false;

        if (input.Length <= row + (rowStep * -1) || input[row].Length <= col + (colStep * -1) || row + (rowStep * -1) < 0 || col + (colStep * -1) < 0)
            return false;
        
        return (input[row + rowStep][col + colStep] == SEARCH_PATTERN_TERM[0] && input[row - rowStep][col - colStep] == SEARCH_PATTERN_TERM[1]) || 
               (input[row + rowStep][col + colStep] == SEARCH_PATTERN_TERM[1] && input[row - rowStep][col - colStep] == SEARCH_PATTERN_TERM[0]);
    }

    internal int SolutionPartOne()
    {
        int result = 0;
        Span<string> input = Input.Value.Span;

        for (int rowIdx = 0; rowIdx < input.Length; rowIdx++)
        {
            for (int colIdx = 0; colIdx < input[rowIdx].Length; colIdx++)
            {
                if (input[rowIdx][colIdx] == 'X')
                {
                    if (WordSearch(rowIdx, colIdx, 0, 1)) result++; // east
                    if (WordSearch(rowIdx, colIdx, 1, 1)) result++; // southeast
                    if (WordSearch(rowIdx, colIdx, 1, 0)) result++; // south
                    if (WordSearch(rowIdx, colIdx, 1, -1)) result++; // southwest
                    if (WordSearch(rowIdx, colIdx, 0, -1)) result++; // west
                    if (WordSearch(rowIdx, colIdx, -1, -1)) result++; // northwest
                    if (WordSearch(rowIdx, colIdx, -1, 0)) result++; // north
                    if (WordSearch(rowIdx, colIdx, -1, 1)) result++; // northeast
                }
            }
        }

        return result;
    }

    internal int SolutionPartTwo()
    {
        int result = 0;
        Span<string> input = Input.Value.Span;

        for (int rowIdx = 0; rowIdx < input.Length; rowIdx++)
        {
            for (int colIdx = 0; colIdx < input[rowIdx].Length; colIdx++)
            {
                if (input[rowIdx][colIdx] == 'A')
                {
                    bool first = PatternSearch(rowIdx, colIdx, -1, -1); // northwest -> southeast | southeast -> northwest;
                    bool second = PatternSearch(rowIdx, colIdx, -1, 1); // northeast -> southwest | southwest -> northeast;
                    if (first && second) result++;
                }
            }
        }

        return result;
    }
}
