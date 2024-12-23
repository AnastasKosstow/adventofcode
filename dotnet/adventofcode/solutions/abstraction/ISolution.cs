namespace adventofcode;

public interface ISolution
{
    int Day { get; }
    string Puzzle { get; }

    void SetInput(string inputSource);
    ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute();
}
