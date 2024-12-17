using adventofcode.builder;

var adventofcode = new AdventOfCodeBuilder();

adventofcode
    .Select(
    selector => selector.Day1,
    selector => selector.Day2,
    selector => selector.Day8,
    selector => selector.Day9)
    .Run();
