using adventofcode.builder;

var adventofcode = new AdventOfCodeBuilder();

adventofcode
    .Select(
        selector => selector.HistorianHysteria,
        selector => selector.RedNosedReports,
        selector => selector.MullItOver,
        selector => selector.CeresSearch,
        selector => selector.PrintQueue,
        selector => selector.RestroomRedoubt
    )
    .Run();
