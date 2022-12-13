using System.Numerics;

namespace AdventOfCode;

public sealed class Day11 : BaseDay
{
    private readonly string[] _lines;
    private List<Monkey> _monkeys;

    private static BigInteger _divisorsMult = 1;

    public Day11()
    {
        _lines = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        InitializeMonkeys();
        SimulateRounds(20);

        var result = _monkeys
            .OrderByDescending(x => x.InspectedCount)
            .Take(2)
            .Select(x => x.InspectedCount)
            .ToList();

        return new ValueTask<string>($"{result[0] * result[1]}");
    }

    public override ValueTask<string> Solve_2()
    {
        InitializeMonkeys();
        SimulateRounds(10000, true);

        var result = _monkeys
            .OrderByDescending(x => x.InspectedCount)
            .Take(2)
            .Select(x => x.InspectedCount)
            .ToList();

        return new ValueTask<string>($"{result[0] * result[1]}");
    }

    private void InitializeMonkeys()
    {
        _monkeys = new List<Monkey>();
        for (var i = 0; i < (_lines.Length + 1) / 7; i++)
        {
            _monkeys.Add(Monkey.Parse(_lines.Skip(i * 7).Take(7).ToArray()));
        }

        _divisorsMult = _monkeys.Select(x => x.DivisionTest.DivisibleBy).Aggregate((x, y) => x * y);
    }

    private void SimulateRounds(int count, bool part2 = false)
    {
        for (var i = 1; i <= count; i++)
        {
            SimulateRound(part2);
        }
    }

    private void SimulateRound(bool part2)
    {
        foreach (var monkey in _monkeys)
        {
            var itemsCount = monkey.Items.Count;
            for (var index = 0; index < itemsCount; index++)
            {
                var result = monkey.Inspect(0, part2);
                var m = _monkeys.FirstOrDefault(x => x.Index == result.MonkeyIndex);
                m?.Items.Add(result.Item);
            }
        }
    }

    public class Monkey
    {
        public int Index { get; }
        public List<BigInteger> Items { get; }

        private Operation Operation { get; }

        public DivisionTest DivisionTest { get; }

        public long InspectedCount { get; private set; }

        private Monkey(int index, List<BigInteger> startingItems, Operation operation, DivisionTest divisionTest)
        {
            Index = index;
            Items = startingItems;
            Operation = operation;
            DivisionTest = divisionTest;
        }

        public static Monkey Parse(string[] lines)
        {
            var index = lines[0].Split(' ')[1][0] - '0';
            var startingItemsPart = lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            var startingItems = startingItemsPart.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => BigInteger.Parse(x.Trim()))
                .ToList();
            var operationPart = lines[2].Split('=', StringSplitOptions.RemoveEmptyEntries)[1];
            var operationParts = operationPart.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var operationType = operationParts[2] == "old" ? OperationType.OldTimesOld : (operationParts[1] == "*" ? OperationType.Multiply : OperationType.Add);
            var operationValue = operationParts[2] != "old" ? BigInteger.Parse(operationParts[2]) : 0;
            var divisibleBy = BigInteger.Parse(lines[3].Split('y', StringSplitOptions.RemoveEmptyEntries)[1].Trim());
            var trueMonkey = int.Parse(lines[4].Split(' ', StringSplitOptions.RemoveEmptyEntries)[5]);
            var falseMonkey = int.Parse(lines[5].Split(' ', StringSplitOptions.RemoveEmptyEntries)[5]);

            return new Monkey(index, startingItems, new Operation(operationType, operationValue), new DivisionTest(divisibleBy, trueMonkey, falseMonkey));
        }

        public InspectResult Inspect(int index, bool part2)
        {
            var item = Items[index];

            switch (Operation.Type)
            {
                case OperationType.Add:
                    item += Operation.Value;
                    break;
                case OperationType.Multiply:
                    item *= Operation.Value;
                    break;
                case OperationType.OldTimesOld:
                    item *= item;
                    break;
            }

            if (!part2)
                item /= 3;


            // idk why it works, but it does
            item %= _divisorsMult;


            var test = item % DivisionTest.DivisibleBy == 0 ? DivisionTest.TrueMonkey : DivisionTest.FalseMonkey;

            Items.RemoveAt(index);
            InspectedCount++;
            return new InspectResult(test, item);
        }
    }

    public record InspectResult(int MonkeyIndex, BigInteger Item);

    public record Operation(OperationType Type, BigInteger Value);

    public record DivisionTest(BigInteger DivisibleBy, int TrueMonkey, int FalseMonkey);

    public enum OperationType
    {
        Add,
        Multiply,
        OldTimesOld,
    };
}