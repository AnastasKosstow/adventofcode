using System.Runtime.CompilerServices;

namespace adventofcode;

public class CrossedWires : ExecutionMeasure, ISolution
{
    public int Day => 24;
    public string Puzzle => "Crossed Wires";

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        var resultPartOne = SolutionPartOne();
        var resultPartTwo = SolutionPartTwo();
        var millisecondsPartOne = MeasureExecutionTime(SolutionPartOne);
        var millisecondsPartTwo = MeasureExecutionTime(SolutionPartTwo);

        return ((resultPartOne.ToString(), millisecondsPartOne), (resultPartTwo.ToString(), millisecondsPartTwo));
    }

    private Dictionary<string, byte> Wires = [];
    private SortedList<int, string> Z_Wires = [];
    private List<string> Instructions = [];
    private Graph Graph = new();

    private static readonly char[] separator = [' ', '-', '>'];

    public void SetInput(string inputSource)
    {
        var lines = File.ReadAllLines(inputSource);

        int separatorIndex = Array.IndexOf(lines, string.Empty);

        Wires = lines
            .Take(separatorIndex)
            .Select(line => line.Split(':', StringSplitOptions.TrimEntries))
            .ToDictionary(parts => parts[0], parts => byte.Parse(parts[1]));

        Instructions = new List<string>(
            lines.Skip(separatorIndex + 1)
                .Where(line => !string.IsNullOrEmpty(line))
        );
    }

    private void BuildGraph()
    {
        for (int idx = 0; idx < Instructions.Count; idx++)
        {
            var instruction = Instructions[idx]
                .Split(separator, StringSplitOptions.RemoveEmptyEntries);

            if (instruction[3].StartsWith('z'))
            {
                int key = int.Parse(instruction[3][1..]);
                Z_Wires.Add(key, instruction[3]);
            }

            Graph.AddNode(
                value: instruction[3],
                leftNodeValue: instruction[0],
                rightNodeValue: instruction[2],
                bitwiseOperation: instruction[1]);
        }
    }

    private long BinaryToInt(List<byte> bits)
    {
        long result = 0;
        for (int idx = bits.Count - 1; idx >= 0; idx--)
        {
            result = (result << 1) | bits[idx];
        }
        return result;
    }

    private long SolutionPartOne()
    {
        if (Graph == null || Graph.Count == 0)
        {
            BuildGraph();
        }

        var bits = new List<byte>();
        for (int idx = 0; idx < Z_Wires.Count; idx++)
        {
            byte value = WireValue(Graph.GetNode(Z_Wires[idx]));
            bits.Add(value);
        }

        long result = BinaryToInt(bits);
        return result;

        byte WireValue(Node node)
        {
            if (!Wires.TryGetValue(node.Left.Value, out var left))
            {
                left = WireValue(node.Left);
            }

            if (!Wires.TryGetValue(node.Right.Value, out var right))
            {
                right = WireValue(node.Right);
            }

            return node.BitwiseOperation switch
            {
                "AND" => (byte)(left & right),
                "OR" => (byte)(left | right),
                "XOR" => (byte)(left ^ right)
            };
        }
    }

    private string SolutionPartTwo()
    {
        if (Graph == null || Graph.Count == 0)
        {
            BuildGraph();
        }

        return "";
    }
}
