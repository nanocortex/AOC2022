using System.Text;

namespace AdventOfCode;

public sealed class Day03 : BaseDay
{
    private readonly string[] _input;

    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = 0;
        foreach (var line in _input)
        {
            var length = line.Length;
            var division = length / 2;
            var comp1 = line[..division];
            var comp2 = line[division..];
            var commonItem = comp1.Intersect(comp2).FirstOrDefault();
            var priority = GetPriority(commonItem);
            sum += priority;
        }

        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        var sum = 0;

        for (var i = 0; i < _input.Length; i += 3)
        {
            var line1 = _input[i];
            var line2 = _input[i + 1];
            var line3 = _input[i + 2];

            var aggregate = new List<List<char>> { line1.ToList(), line2.ToList(), line3.ToList(), };

            var commonChar = aggregate
                .Aggregate((previousList, nextList)
                    => previousList.Intersect(nextList).ToList())
                .FirstOrDefault();

            var priority = GetPriority(commonChar);
            sum += priority;
        }

        return new ValueTask<string>($"{sum}");
    }

    private int GetPriority(char c)
    {
        var code = Encoding.ASCII.GetBytes(c.ToString())[0];
        return char.IsLower(c) ? code - 96 : code - 38;
    }
}