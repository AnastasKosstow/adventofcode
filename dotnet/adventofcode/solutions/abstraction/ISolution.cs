namespace adventofcode;

public interface ISolution
{
    int Day { get; }
    string Puzzle { get; }

    void SetInput(string inputSource);
    (string partOne, string partTwo) Execute();
}
