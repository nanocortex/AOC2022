namespace AdventOfCode;

public class Cargo
{
    private readonly List<Stack<string>> _stacks = new();

    public Cargo(IReadOnlyList<string> input)
    {
        for (var i = 0; i < 9; ++i)
            _stacks.Add(new Stack<string>());

        for (var i = input.Count - 2; i >= 0; --i)
        {
            var itemIndex = 1;
            for (var j = 1; j < input[i].Length; j += 4)
            {
                var item = input[i][j].ToString();
                if (string.IsNullOrWhiteSpace(item))
                {
                    itemIndex++;
                    continue;
                }

                _stacks[itemIndex - 1].Push(item);
                itemIndex++;
            }
        }
    }

    public void ApplyInstructions(List<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            for (var i = 0; i < instruction.Count; i++)
            {
                var value = _stacks[instruction.From - 1].Pop();
                _stacks[instruction.To - 1].Push(value);
            }
        }
    }

    public void ApplyInstructions2(List<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            var values = new List<string>();
            for (var i = 0; i < instruction.Count; i++)
            {
                values.Add(_stacks[instruction.From - 1].Pop());
            }

            values.Reverse();

            foreach (var value in values)
            {
                _stacks[instruction.To - 1].Push(value);
            }
        }
    }

    public string GetTopCrates()
    {
        return _stacks.Aggregate(string.Empty, (current, stack) => current + stack.Peek());
    }
}

public record Instruction(int Count, int From, int To)
{
    public static Instruction FromString(string input)
    {
        var parts = input.Split(' ');
        return new Instruction(int.Parse(parts[1]), int.Parse(parts[3]), int.Parse(parts[5]));
    }
}

public sealed class Day05 : BaseDay
{
    private readonly List<Instruction> _instructions;
    private readonly string[] _input;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
        _instructions = _input.Skip(10).Select(Instruction.FromString).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var cargo = new Cargo(_input.Take(9).ToArray());
        cargo.ApplyInstructions(_instructions);
        return new ValueTask<string>($"{cargo.GetTopCrates()}");
    }

    public override ValueTask<string> Solve_2()
    {
        var cargo = new Cargo(_input.Take(9).ToArray());
        cargo.ApplyInstructions2(_instructions);
        return new ValueTask<string>($"{cargo.GetTopCrates()}");
    }
}