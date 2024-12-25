namespace adventofcode;

public class LANParty : ExecutionMeasure, ISolution
{
    public int Day => 23;
    public string Puzzle => "LAN Party";

    public ((string result, double milliseconds) partOne, (string result, double milliseconds) partTwo) Execute()
    {
        var resultPartOne = SolutionPartOne();
        var resultPartTwo = SolutionPartTwo();
        var millisecondsPartOne = MeasureExecutionTime(SolutionPartOne);
        var millisecondsPartTwo = MeasureExecutionTime(SolutionPartTwo);

        return ((resultPartOne.ToString(), millisecondsPartOne), (resultPartTwo, millisecondsPartTwo));
    }

    public void SetInput(string inputSource)
    {
        var lines = File.ReadAllLines(inputSource);

        foreach (string line in lines)
        {
            string[] computers = line.Split('-');

            if (!Graph.ContainsKey(computers[0]))
            {
                Graph[computers[0]] = [];
                Degree[computers[0]] = 0;
            }
            if (!Graph.ContainsKey(computers[1]))
            {
                Graph[computers[1]] = [];
                Degree[computers[1]] = 0;
            }

            Graph[computers[0]].Add(computers[1]);
            Degree[computers[0]]++;
            Graph[computers[1]].Add(computers[0]);
            Degree[computers[1]]++;

            NodesSet.Add(computers[0]);
            NodesSet.Add(computers[1]);
        }
    }

    private Dictionary<string, HashSet<string>> Graph = [];
    private Dictionary<string, int> Degree = [];
    private HashSet<string> NodesSet = [];

    private void FindCliques(HashSet<HashSet<string>> networkGroups, List<string> nodes, int startIndex, HashSet<string> currentNetworkGroup, int targetSize)
    {
        if (currentNetworkGroup.Count == targetSize)
        {
            networkGroups.Add(new HashSet<string>(currentNetworkGroup));
            return;
        }

        for (int idx = startIndex; idx < nodes.Count; idx++)
        {
            string candidateNode = nodes[idx];
            if (Degree[candidateNode] < targetSize - 1)
            {
                continue;
            }

            currentNetworkGroup.Add(candidateNode);

            if (IsNetworkGroup(currentNetworkGroup))
            {
                FindCliques(networkGroups, nodes, idx + 1, currentNetworkGroup, targetSize);
            }
            currentNetworkGroup.Remove(candidateNode);
        }
    }

    private bool IsNetworkGroup(HashSet<string> networkGroup)
    {
        List<string> nodes = [.. networkGroup];
        for (int sourceNodeIndex = 0; sourceNodeIndex < nodes.Count; sourceNodeIndex++)
        {
            for (int targetNodeIndex = 0; targetNodeIndex < nodes.Count; targetNodeIndex++)
            {
                if (sourceNodeIndex == targetNodeIndex)
                {
                    continue;
                }
                if (!Graph[nodes[sourceNodeIndex]].Contains(nodes[targetNodeIndex])) return false;
                if (!Graph[nodes[targetNodeIndex]].Contains(nodes[sourceNodeIndex])) return false;
            }
        }

        return true;
    }

    public int SolutionPartOne()
    {
        HashSet<HashSet<string>> networkGroups = [];
        List<string> nodes = [.. NodesSet];

        FindCliques(networkGroups, nodes, 0, [], 3);

        int result = 0;
        foreach (var networkGroup in networkGroups)
        {
            if (networkGroup.Any(node => node.StartsWith('t'))) result++;
        }

        return result;
    }

    public string SolutionPartTwo()
    {
        HashSet<string> maxNetworkGroup = [];
        List<string> nodes = [.. NodesSet];

        MaximalNetworkGroup(0, []);

        var result = string.Join(',', maxNetworkGroup.ToList().OrderBy(x => x));
        return result;

        void MaximalNetworkGroup(int startIdx, HashSet<string> currentNetworkGroup)
        {
            if (currentNetworkGroup.Count >= maxNetworkGroup.Count)
            {
                maxNetworkGroup = new HashSet<string>(currentNetworkGroup);
            }

            if (startIdx >= nodes.Count)
            {
                return;
            }

            for (int idx = startIdx; idx < nodes.Count; idx++)
            {
                var candidateNode = nodes[idx];

                currentNetworkGroup.Add(candidateNode);
                if (IsNetworkGroup(currentNetworkGroup))
                {
                    MaximalNetworkGroup(idx + 1, currentNetworkGroup);
                }
                currentNetworkGroup.Remove(candidateNode);
            }
        }
    }
}
