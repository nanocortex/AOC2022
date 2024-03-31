namespace AdventOfCode;

public sealed class Day12 : BaseDay
{
    private List<List<int>> _map;
    private Position _startPosition;
    private Position _endPosition;

    public Day12()
    {
        InitializeMap();
    }

    public override ValueTask<string> Solve_1()
    {
        PrintMap();

        DijkstraAlgo(_map, 0, 9);

        // bfs on map
        // var visited = new HashSet<Position>();
        // var queue = new Queue<Position>();
        // queue.Enqueue(_startPosition);
        // var count = 0;
        // while (queue.Count > 0)
        // {
        //     var current = queue.Dequeue();
        //     visited.Add(current);
        //     foreach (var neighbor in GetNeighbors(current))
        //     {
        //         if (visited.Contains(neighbor)) continue;
        //
        //         queue.Enqueue(neighbor);
        //         count++;
        //
        //         if (neighbor == _endPosition)
        //         {
        //             return new ValueTask<string>($"{neighbor} {count}");
        //         }
        //     }
        // }


        return new ValueTask<string>("");
    }

    private IEnumerable<Position> GetNeighbors(Position current)
    {
        // get neighbors
        var neighbors = new List<Position>();
        var x = current.Row;
        var y = current.Col;
        var value = _map[x][y];

        if (x > 0 && _map[x - 1][y] - value <= 1)
        {
            neighbors.Add(new Position(x - 1, y));
        }

        if (x < _map.Count - 1 && _map[x + 1][y] - value <= 1)
        {
            neighbors.Add(new Position(x + 1, y));
        }

        if (y > 0 && _map[x][y - 1] - value <= 1)
        {
            neighbors.Add(new Position(x, y - 1));
        }

        if (y < _map[0].Count - 1 && _map[x][y + 1] - value <= 1)
        {
            neighbors.Add(new Position(x, y + 1));
        }

        return neighbors;
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>("");
    }

    public record Position(int Row, int Col);

    private void PrintMap()
    {
        foreach (var t in _map)
        {
            foreach (var t1 in t)
            {
                Console.Write(t1.ToString().PadLeft(2, '0') + " ");
            }

            Console.WriteLine();
        }
    }

    private void InitializeMap()
    {
        _map = new List<List<int>>();
        var rows = File.ReadAllLines(InputFilePath);
        for (var i = 0; i < rows.Length; i++)
        {
            var row = rows[i];
            var cols = new List<int>();
            for (var j = 0; j < row.Length; j++)
            {
                var col = row[j];
                var value = 0;
                if (col == 'S')
                {
                    _startPosition = new Position(i, j);
                    col = 'a';
                }
                else if (col == 'E')
                {
                    _endPosition = new Position(i, j);
                    col = 'z';
                }

                value = col - 97;

                cols.Add(value);
            }

            _map.Add(cols);
        }
    }

    private static int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
    {
        int min = int.MaxValue;
        int minIndex = 0;

        for (int v = 0; v < verticesCount; ++v)
        {
            if (shortestPathTreeSet[v] == false && distance[v] <= min)
            {
                min = distance[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    private static void Print(int[] distance, int verticesCount)
    {
        Console.WriteLine("Vertex    Distance from source");

        for (int i = 0; i < verticesCount; ++i)
            Console.WriteLine("{0}\t  {1}", i, distance[i]);
    }

    public static void DijkstraAlgo(List<List<int>> graph, int source, int verticesCount)
    {
        var distance = new int[verticesCount];
        var shortestPathTreeSet = new bool[verticesCount];

        for (var i = 0; i < verticesCount; ++i)
        {
            distance[i] = int.MaxValue;
            shortestPathTreeSet[i] = false;
        }

        distance[source] = 0;

        for (var count = 0; count < verticesCount - 1; ++count)
        {
            var u = MinimumDistance(distance, shortestPathTreeSet, verticesCount);
            shortestPathTreeSet[u] = true;

            for (var v = 0; v < verticesCount; ++v)
                if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u][v]) && distance[u] != int.MaxValue && distance[u] + graph[u][v] < distance[v])
                    distance[v] = distance[u] + graph[u][v];
        }

        Print(distance, verticesCount);
    }
}