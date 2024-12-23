namespace adventofcode.builder.select;

public class Selector<T> : ISelector<T> where T : ISolution, new()
{
    private readonly string _inputPath = "builder/inputs/day-{0}-input.txt";
    private readonly T _solution;

    public int Day => _solution.Day;
    public string Puzzle => _solution.Puzzle;

    public Selector()
    {
        _solution = new T();

        var path = string.Format(_inputPath, Day);
        _solution.SetInput(path);
    }

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        return _solution.Execute();
    }
}
