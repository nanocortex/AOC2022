namespace AdventOfCode;

public enum Type
{
    File,
    Directory,
}

public class Node
{
    public Node(Type type, Node parent, string name, int size = 0)
    {
        Type = type;
        Parent = parent;
        Children = new List<Node>();
        Name = name;
        Size = size;
    }

    public string Name { get; }

    public int Size { get; set; }


    public Type Type { get; set; }

    public bool IsFile => Type == Type.File;

    public bool IsDirectory => Type == Type.Directory;

    public Node Parent { get; }

    public List<Node> Children { get; }

    public Node Add(string name, int size = 0)
    {
        var type = size > 0 ? Type.File : Type.Directory;
        var node = new Node(type, this, name, size);
        Children.Add(node);
        return node;
    }
}

internal class Tree
{
    public readonly Node Root = new(Type.Directory, null, "/");
    private readonly List<int> _dirSizes = new();

    public Tree(IEnumerable<string> lines)
    {
        var currentNode = Root;
        foreach (var line in lines)
        {
            var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts[0] == "$")
            {
                if (parts[1] == "cd")
                {
                    var directory = parts[2];
                    if (directory == "/")
                    {
                        currentNode = Root;
                        continue;
                    }

                    if (directory == "..")
                    {
                        currentNode = currentNode.Parent;
                        continue;
                    }

                    var child = currentNode.Children.FirstOrDefault(c => c.Name == directory);
                    if (child == null)
                    {
                        child = new Node(Type.Directory, currentNode, directory);
                        currentNode.Children.Add(child);
                    }

                    currentNode = child;
                    continue;
                }

                if (parts[1] == "ls")
                    continue;
            }


            if (parts[0] == "dir")
                currentNode.Add(parts[1]);
            else
                currentNode.Add(parts[1], int.Parse(parts[0]));
        }
    }


    public int CalculateSize(Node node)
    {
        if (node.IsFile)
            return node.Size;

        var size = node.Children.Sum(CalculateSize);
        node.Size = size;
        _dirSizes.Add(size);
        return size;
    }

    public int SumLessOrEqual100000(Node node)
    {
        var sum = 0;
        if (node.IsDirectory && node.Size <= 100000) sum += node.Size;
        sum += node.Children.Where(x => x.IsDirectory).Sum(SumLessOrEqual100000);
        return sum;
    }

    public int FindSmallestToDelete()
    {
        const int availableSpace = 70000000;
        const int requiredSpace = 30000000;

        var currentUsedSpace = Root.Size;
        var currentFreeSpace = availableSpace - currentUsedSpace;

        var smallest = int.MaxValue;

        foreach (var size in _dirSizes)
        {
            if (currentFreeSpace + size < requiredSpace) continue;

            if (size < smallest)
                smallest = size;
        }

        return smallest;
    }
}

public sealed class Day07 : BaseDay
{
    private readonly Tree _tree;

    public Day07()
    {
        _tree = new Tree(File.ReadAllLines(InputFilePath));
        _tree.CalculateSize(_tree.Root);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = _tree.SumLessOrEqual100000(_tree.Root);
        return new ValueTask<string>($"{result}");
    }

    public override ValueTask<string> Solve_2()
    {
        var result = _tree.FindSmallestToDelete();
        return new ValueTask<string>($"{result}");
    }
}