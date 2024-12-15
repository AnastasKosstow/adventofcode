using adventofcode.builder.select;

namespace adventofcode.builder;

public interface IAdventOfCodeBuilder
{
    IAdventOfCodeBuilder Select(params Func<SolutionSelector, ISelector>[] selectors);
    void Run();
}
