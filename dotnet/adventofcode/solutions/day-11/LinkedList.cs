namespace adventofcode;

internal class LinkedList
{
    internal class Node(decimal value)
    {
        internal decimal Value = value;
        internal Node Next = null;
        internal Node Prev = null;
    }

    private Node head;
    private Node tail;
    private int length;

    public int Length
    {
        get { return this.length;  }
    }

    public Node Head
    {
        get { return this.head; }
    }

    public LinkedList()
    {
        head = null;
        tail = null;
        length = 0;
    }

    public void Add(decimal value)
    {
        var node = new Node(value);

        if (this.Length == 0)
        {
            head = tail = node;
        }
        else
        {
            node.Prev = tail;
            tail.Next = node;
            tail = node;
        }

        length++;
    }

    public void Split(Node node)
    {
        string nodeValueStr = node.Value.ToString();
        var halfLength = nodeValueStr.Length / 2;

        var newNode = new Node(decimal.Parse(nodeValueStr[halfLength..]))
        {
            Prev = node,
            Next = node.Next
        };
        node.Value = decimal.Parse(nodeValueStr[..halfLength]);
        node.Next = newNode;

        length++;
    }
}
