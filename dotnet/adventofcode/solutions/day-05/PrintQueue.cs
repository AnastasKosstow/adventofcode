namespace adventofcode;

public class PrintQueue : ISolution
{
    public int Day { get; } = 5;
    public string Puzzle { get; } = "Print Queue";

    private Lazy<(Memory<string> rules, Memory<string> updates)> Input;

    public (string partOne, string partTwo) Execute()
    {
        var partOne = SolutionPartOne();
        var partTwo = SolutionPartTwo();
        return (partOne.ToString(), partTwo.ToString());
    }

    public void SetInput(string inputSource)
    {
        Input = new(() =>
        {
            var input = File.ReadAllText(inputSource);

            string[] parts = input.Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);

            return (
                rules: parts[0].Split(Environment.NewLine, StringSplitOptions.None).AsMemory(),
                updates: parts[1].Split(Environment.NewLine, StringSplitOptions.None).AsMemory()
            );
        });
    }

    private IEnumerable<int> TopologicalSort(int[] numbers, IDictionary<int, HashSet<int>> graph)
    {
        var result = new List<int>();
        var visited = new HashSet<int>();
        var numberSet = new HashSet<int>(numbers);

        for (int idx = 0; idx < numbers.Length; idx++)
        {
            if (!visited.Contains(numbers[idx]))
                DFS(numbers[idx]);
        }

        void DFS(int node)
        {
            if (visited.Contains(node))
                return;

            if (graph.TryGetValue(node, out var afterNodes))
            {
                foreach (var after in afterNodes)
                {
                    if (numberSet.Contains(after))
                    {
                        DFS(after);
                    }
                }
            }

            visited.Add(node);
            result.Insert(0, node);
        }

        return result;
    }

    private Dictionary<int, HashSet<int>> BuildGraph(ReadOnlySpan<string> rules)
    {
        var graph = new Dictionary<int, HashSet<int>>();

        for (var idx = 0; idx < rules.Length; idx++)
        {
            var (before, after) = rules[idx]
                .Split('|')
                .Select(int.Parse) switch { var x => (x.First(), x.Last()) };

            if (!graph.TryGetValue(before, out _))
            {
                graph.Add(before, []);
            }

            graph[before].Add(after);
        }

        return graph;
    }

    private bool IsValid(int[] update, Dictionary<int, HashSet<int>> graph)
    {
        for (var updateIdx = 0; updateIdx < update.Length; updateIdx++)
        {
            for (var beforeIdx = 0; beforeIdx < updateIdx; beforeIdx++)
            {
                if (graph.TryGetValue(update[beforeIdx], out var values) && !values.Contains(update[updateIdx]))
                {
                    return false;
                }
            }

            for (var afterIdx = updateIdx + 1; afterIdx < update.Length; afterIdx++)
            {
                if (graph.TryGetValue(update[updateIdx], out var values) && !values.Contains(update[afterIdx]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    internal int SolutionPartOne()
    {
        int result = 0;
        var graph = BuildGraph(Input.Value.rules.Span);

        var updates = Input.Value.updates.Span;
        for (var idx = 0; idx < updates.Length; idx++)
        {
            var line = updates[idx].Split(',').Select(int.Parse).ToArray();

            bool valid = IsValid(line, graph);

            if (valid) result += line[line.Length / 2];
        }

        return result;
    }

    internal int SolutionPartTwo()
    {
        int result = 0;
        var graph = BuildGraph(Input.Value.rules.Span);

        var updates = Input.Value.updates.Span;
        for (var idx = 0; idx < updates.Length; idx++)
        {
            var line = updates[idx].Split(',').Select(int.Parse).ToArray();

            bool valid = IsValid(line, graph);

            if (!valid)
            {
                var sortedLine = TopologicalSort(line, graph).ToArray();
                result += sortedLine[sortedLine.Length / 2];
            }
        }

        return result;
    }
}
