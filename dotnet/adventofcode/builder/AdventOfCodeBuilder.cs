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

            ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) = queueItem.Execute();
            TablePrinter.Row(queueItem.Day, queueItem.Puzzle, partOne, partTwo, itemsCount - 1 == _executionQueue.Count);
        }

        TablePrinter.Footer();
    }

    private class QueueItem(int day, string puzzle, Func<((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo)> execute)
    {
        public int Day { get; private set; } = day;
        public string Puzzle { get; private set; } = puzzle;
        public Func<((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo)> Execute { get; private set; } = execute;
    }

    #region TablePrinter
    private static class TablePrinter
    {
        private static readonly ConsoleColor BorderColor = ConsoleColor.Yellow;

        public static void Header()
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = BorderColor;
            Console.WriteLine();
            Console.WriteLine("adventofcode - 2024");
            Console.WriteLine();

            Console.WriteLine(new string('-', 106));

            Console.Write($"|");
            Console.ForegroundColor = originalColor;
            Console.Write($" {"Day",-6} | {"Puzzle",-25} | {"Result Part One",-20} | {"Time",-8} | {"Result Part Two",-20} | {"Time",-9}");
            Console.ForegroundColor = BorderColor;
            Console.Write($"|");
            Console.WriteLine();

            Console.ForegroundColor = BorderColor;
            Console.WriteLine(new string('-', 106));

            Console.ForegroundColor = originalColor;
        }

        public static void Footer()
        {
            var originalColor = Console.ForegroundColor;

            Console.ForegroundColor = BorderColor;
            Console.WriteLine(new string('-', 106));

            Console.ForegroundColor = originalColor;
        }

        public static void Row(int day, string puzzle, (string result, double milliseconds) partOne, (string result, double milliseconds) partTwo, bool isFirstRow)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = originalColor;

            if (!isFirstRow)
            {
                Console.WriteLine(new string('-', 106));
            }

            Console.ForegroundColor = BorderColor;
            Console.Write($"|");
            Console.ForegroundColor = originalColor;
            Console.Write($" Day {day,-2} | {puzzle,-25} | {partOne.result,-20} | ");

            var (partOneColor, partOneTime) = FormatTime(partOne.milliseconds);
            Console.ForegroundColor = partOneColor;
            Console.Write($"{partOneTime,-9}");

            Console.ForegroundColor = originalColor;
            Console.Write($"| {partTwo.result,-20} | ");

            var (partTwoColor, partTwoTime) = FormatTime(partTwo.milliseconds);
            Console.ForegroundColor = partTwoColor;
            Console.Write($"{partTwoTime,-9}");

            Console.ForegroundColor = BorderColor;
            Console.Write($"|");
            Console.ForegroundColor = originalColor;
            Console.WriteLine();
        }

        public static (ConsoleColor color, string time) FormatTime(double elapsedMilliseconds)
        {
            var message = elapsedMilliseconds switch
            {
                < 0.001 => $"{elapsedMilliseconds * 1_000_000:F} µs",
                < 1 => $"{elapsedMilliseconds:F} ms",
                < 1_000 => $"{Math.Round(elapsedMilliseconds)} ms",
                < 60_000 => $"{0.001 * elapsedMilliseconds:F} s",
                _ => $"{Math.Floor(elapsedMilliseconds / 60_000)} min {Math.Round(0.001 * (elapsedMilliseconds % 60_000))} s",
            };

            var color = elapsedMilliseconds switch
            {
                < 0.001 => ConsoleColor.Blue,
                < 350 => ConsoleColor.Green,
                < 1_000 => ConsoleColor.Yellow,
                _ => ConsoleColor.Red
            };

            return (color, message);
        }
    }
    #endregion
}
