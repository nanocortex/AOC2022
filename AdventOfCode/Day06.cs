namespace AdventOfCode;

public sealed class Day06 : BaseDay
{
    private readonly string _buffer;

    public Day06()
    {
        _buffer = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var marker = GetMarker(4);
        return new ValueTask<string>($"{marker}");
    }

    public override ValueTask<string> Solve_2()
    {
        var marker = GetMarker(14);
        return new ValueTask<string>($"{marker}");
    }

    private int GetMarker(int distinctCount)
    {
        for (var i = 0; i < _buffer.Length; i++)
        {
            var markerStream = string.Empty;

            for (var j = i; j < i + distinctCount; j++)
            {
                if (markerStream.Contains(_buffer[j]))
                    break;

                markerStream += _buffer[j];
            }

            if (markerStream.Length != distinctCount) continue;
            return i + distinctCount;
        }

        return 0;
    }
}