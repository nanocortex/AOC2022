namespace AdventOfCode;

public class Map
{
    private readonly List<List<int>> _trees = new();

    public Map(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var row = line.Select(c => c - '0').ToList();
            _trees.Add(row);
        }
    }

    public int GetVisibleCount()
    {
        var count = 0;

        for (var row = 0; row < _trees.Count; row++)
        {
            for (var col = 0; col < _trees[row].Count; col++)
            {
                count += IsVisible(row, col) ? 1 : 0;
            }
        }

        return count;
    }

    private bool IsVisible(int row, int col)
    {
        var value = _trees[row][col];
        var visibleFromLeft = true;
        for (var i = 0; i < col; i++)
        {
            if (_trees[row][i] < value) continue;
            visibleFromLeft = false;
            break;
        }

        if (visibleFromLeft)
            return true;

        var visibleFromRight = true;
        for (var i = _trees[row].Count - 1; i > col; i--)
        {
            if (_trees[row][i] < value) continue;
            visibleFromRight = false;
            break;
        }

        if (visibleFromRight)
            return true;

        var visibleFromTop = true;
        for (var i = 0; i < row; i++)
        {
            if (_trees[i][col] < value) continue;
            visibleFromTop = false;
            break;
        }

        if (visibleFromTop)
            return true;

        var visibleFromBottom = true;
        for (var i = _trees.Count - 1; i > row; i--)
        {
            if (_trees[i][col] < value) continue;
            visibleFromBottom = false;
            break;
        }

        if (visibleFromBottom)
            return true;

        return false;
    }

    public int GetHighestScenicScore()
    {
        var highest = 0;

        for (var row = 0; row < _trees.Count; row++)
        {
            for (var col = 0; col < _trees[row].Count; col++)
            {
                var scenicScore = GetScenicScore(row, col);
                if (scenicScore > highest)
                    highest = scenicScore;
            }
        }

        return highest;
    }

    private int GetScenicScore(int row, int col)
    {
        var value = _trees[row][col];
        var left = 0;
        var top = 0;
        var right = 0;
        var bottom = 0;

        // left
        if (col > 0)
        {
            for (var i = col - 1; i >= 0; i--)
            {
                left++;
                if (_trees[row][i] >= value) break;
            }
        }

        // right
        if (col < _trees[row].Count - 1)
        {
            for (var i = col + 1; i < _trees[row].Count; i++)
            {
                right++;
                if (_trees[row][i] >= value) break;
            }
        }


        // top
        if (row > 0)
        {
            for (var i = row - 1; i >= 0; i--)
            {
                top++;
                if (_trees[i][col] >= value) break;
            }
        }

        // bottom
        if (row < _trees.Count - 1)
        {
            for (var i = row + 1; i < _trees.Count; i++)
            {
                bottom++;
                if (_trees[i][col] >= value) break;
            }
        }


        return top * left * bottom * right;
    }
}

public sealed class Day08 : BaseDay
{
    private readonly Map _map;

    public Day08()
    {
        _map = new Map(File.ReadAllLines(InputFilePath));
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>($"{_map.GetVisibleCount()}");
    }

    public override ValueTask<string> Solve_2()
    {
        return new ValueTask<string>($"{_map.GetHighestScenicScore()}");
    }
}