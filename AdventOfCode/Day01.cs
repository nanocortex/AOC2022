namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var largest = 0;
        var elfCalories = 0;

        foreach (var line in _input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (elfCalories > largest)
                {
                    largest = elfCalories;
                }

                elfCalories = 0;
                continue;
            }

            elfCalories += int.Parse(line);
        }

        return new($"{largest}");
    }

    public override ValueTask<string> Solve_2()
    {
        var elfCalories = new List<int>();
        var sum = 0;

        foreach (var line in _input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                elfCalories.Add(sum);
                sum = 0;
                continue;
            }

            sum += int.Parse(line);
        }

        var total = elfCalories.OrderDescending().Take(3).Sum();

        return new($"{total}");
    }
}