namespace adventofcode.builder.select;

public interface ISelector
{
    int Day { get; }
    string Puzzle { get; }
    (string partOne, string partTwo) Execute();
}

public interface ISelector<out T> : ISelector where T : ISolution
{
}
