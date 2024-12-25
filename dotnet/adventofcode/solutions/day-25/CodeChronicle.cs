namespace adventofcode;

public class CodeChronicle : ExecutionMeasure, ISolution
{
    public int Day => 24;
    public string Puzzle => "Code Chronicle";

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
        throw new NotImplementedException();
    }

    private int SolutionPartOne()
    {
        throw new NotImplementedException();
    }

    private int SolutionPartTwo()
    {
        throw new NotImplementedException();
    }
}
