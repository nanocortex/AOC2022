namespace AdventOfCode;

public record Pair(int From, int To)
{
    public static Pair FromString(string s)
    {
        var parts = s.Split('-');
        var p1 = int.Parse(parts[0]);
        var p2 = int.Parse(parts[1]);
        return new Pair(p1, p2);
    }

    public bool OverlapFull(Pair other)
    {
        return other.From >= From && other.To <= To;
    }

    public bool Overlap(Pair other)
    {
        return (From >= other.From && From <= other.To) || (To >= other.From && To <= other.To);
    }
}

public sealed class Day04 : BaseDay
{
    private readonly string[] _input;

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = 0;
        foreach (var line in _input)
        {
            var pairs = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var pair1 = Pair.FromString(pairs[0]);
            var pair2 = Pair.FromString(pairs[1]);

            if (pair1.OverlapFull(pair2) || pair2.OverlapFull(pair1))
                sum++;
        }

        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        var sum = 0;
        foreach (var line in _input)
        {
            var pairs = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var pair1 = Pair.FromString(pairs[0]);
            var pair2 = Pair.FromString(pairs[1]);

            if (pair1.Overlap(pair2) || pair2.Overlap(pair1))
                sum++;
        }

        return new ValueTask<string>($"{sum}");
    }
}