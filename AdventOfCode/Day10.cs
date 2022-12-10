namespace AdventOfCode;

public sealed class Day10 : BaseDay
{
    private readonly List<Instruction> _instructions;

    public Day10()
    {
        _instructions = File.ReadAllLines(InputFilePath).Select(Instruction.Parse).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var cycle = 0;
        var registerValue = 1;
        var registerCycle = 0;
        var sum = 0;
        var instructionIndex = 0;

        while (true)
        {
            var instruction = _instructions[instructionIndex];

            if (IsInterestingCycle(cycle))
            {
                sum += cycle * registerValue;

                if (cycle == 220)
                    break;
            }

            if (instruction.Type != InstructionType.Noop)
            {
                if (registerCycle == 2)
                {
                    registerCycle = 0;
                    registerValue += instruction.Value;
                    instructionIndex++;
                }

                registerCycle++;
            }
            else
                instructionIndex++;

            cycle++;
        }


        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        var cycle = 0;
        var registerValue = 1;
        var registerCycle = 0;
        var instructionIndex = 0;
        var row = 0;

        while (true)
        {
            if (row == 6)
                break;

            var instruction = _instructions[instructionIndex];

            if (instruction.Type != InstructionType.Noop)
            {
                if (registerCycle == 2)
                {
                    registerCycle = 0;
                    registerValue += instruction.Value;
                    instructionIndex++;
                }

                registerCycle++;
            }
            else
                instructionIndex++;

            if (registerValue == cycle - 1 || registerValue == cycle + 1 || registerValue == cycle)
                Console.Write("#");
            else
                Console.Write(".");

            cycle++;

            if (cycle % 40 != 0) continue;

            Console.Write("\n");
            cycle = 0;
            row++;
        }


        return new ValueTask<string>($"look up ^");
    }


    private bool IsInterestingCycle(int cycle)
    {
        return new[] { 20, 60, 100, 140, 180, 220 }.Contains(cycle);
    }

    private record Instruction(InstructionType Type, int Value)
    {
        public static Instruction Parse(string line)
        {
            line = line.Trim();
            return line == "noop" ? new Instruction(InstructionType.Noop, 0) : new Instruction(InstructionType.AddX, int.Parse(line.Split(" ")[1]));
        }
    }

    private enum InstructionType
    {
        Noop,
        AddX
    }
}