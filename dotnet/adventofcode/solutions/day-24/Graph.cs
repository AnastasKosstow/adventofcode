namespace adventofcode;

public class Node()
{
    public string Value { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }
    public string BitwiseOperation { get; set; }
}

public class Graph
{
    public int Count { get; set; }
    public Dictionary<string, Node> Nodes { get; set; }

    public Graph()
    {
        Nodes = [];
    }

    public void AddNode(string value, string leftNodeValue, string rightNodeValue, string bitwiseOperation)
    {
        if (!Nodes.TryGetValue(value, out var node))
        {
            node = new Node()
            {
                Value = value
            };
        }

        node.Left = this.GetOrCreateNode(leftNodeValue);
        node.Right = this.GetOrCreateNode(rightNodeValue);
        node.BitwiseOperation = bitwiseOperation;
        Nodes[value] = node;

        Count++;
    }

    public Node GetNode(string value)
    {
        return Nodes.TryGetValue(value, out var node) ? node : null;
    }

    private Node GetOrCreateNode(string value)
    {
        var node = this.GetNode(value);
        if (node == null)
        {
            node = new Node() { Value = value };
            Nodes[value] = node;

            Count++;
        }

        return node;
    }
}
