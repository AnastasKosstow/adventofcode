using adventofcode.builder;

var adventofcode = new AdventOfCodeBuilder();

adventofcode
    .Select(
        selector => selector.Day1,
        selector => selector.Day3,
        selector => selector.Day16,
        selector => selector.Day17,
        selector => selector.Day18)
    .Run();
