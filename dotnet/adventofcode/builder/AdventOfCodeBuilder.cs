using adventofcode.builder.select;

namespace adventofcode.builder;

public class AdventOfCodeBuilder : IAdventOfCodeBuilder
{
    private readonly Queue<QueueItem> _executionQueue = new();

    public IAdventOfCodeBuilder Select(params Func<SolutionSelector, ISelector>[] selectors)
    {
        var solutionSelector = new SolutionSelector();

        foreach (var selector in selectors)
        {
            var taskSelector = selector(solutionSelector);
            _executionQueue.Enqueue(new QueueItem(
                taskSelector.Day, 
                taskSelector.Puzzle, 
                taskSelector.Execute));
        }

        return this;
    }

    public void Run()
    {
        TablePrinter.Header();

        var itemsCount = _executionQueue.Count;
        while (_executionQueue.Count > 0)
        {
            var queueItem = _executionQueue.Dequeue();

            (string partOne, string partTwo) = queueItem.Execute();

            TablePrinter.Row(queueItem.Day, queueItem.Puzzle, partOne, partTwo, itemsCount - 1 == _executionQueue.Count ? true : false);
        }

        TablePrinter.Footer();
    }

    private class QueueItem(int day, string puzzle, Func<(string partOne, string partTwo)> execute)
    {
        public int Day { get; private set; } = day;
        public string Puzzle { get; private set; } = puzzle;
        public Func<(string, string)> Execute { get; private set; } = execute;
    }

    private static class TablePrinter
    {
        private static readonly ConsoleColor BorderColor = ConsoleColor.Yellow;

        public static void Header()
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = BorderColor;
            Console.WriteLine(new string('-', 85));

            Console.Write($"|");
            Console.ForegroundColor = originalColor;
            Console.Write($" {"Day",-12} | {"Puzzle",-20} | {"Result Part One",-20} | {"Result Part Two",-20} ");
            Console.ForegroundColor = BorderColor;
            Console.Write($"|");
            Console.WriteLine();

            Console.ForegroundColor = BorderColor;
            Console.WriteLine(new string('-', 85));

            Console.ForegroundColor = originalColor;
        }

        public static void Footer()
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = BorderColor;
            Console.WriteLine(new string('-', 85));

            Console.ForegroundColor = originalColor;
        }

        public static void Row(int day, string puzzle, string solutionPartOne, string solutionPartTwo, bool isFirstRow)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = originalColor;

            if (!isFirstRow)
            {
                Console.WriteLine(new string('-', 85));
            }

            Console.ForegroundColor = BorderColor;
            Console.Write($"|");
            Console.ForegroundColor = originalColor;
            Console.Write($" Day {day,-8} | {puzzle,-20} | {solutionPartOne,-20} | {solutionPartTwo,-20} ");
            Console.ForegroundColor = BorderColor;
            Console.Write($"|");
            Console.ForegroundColor = originalColor;
            Console.WriteLine();
        }
    }
}
