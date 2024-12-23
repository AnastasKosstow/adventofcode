namespace adventofcode.builder.select;

public interface ISelector
{
    int Day { get; }
    string Puzzle { get; }
    ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute();
}

public interface ISelector<out T> : ISelector where T : ISolution
{
}
