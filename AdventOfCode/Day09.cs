namespace AdventOfCode;

public sealed class Day09 : BaseDay
{
    private readonly Instruction[] _instructions;

    public Day09()
    {
        _instructions = File.ReadAllLines(InputFilePath).Select(Instruction.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        var rope = new Rope(1);
        return new ValueTask<string>($"{rope.Simulate(_instructions)}");
    }

    public override ValueTask<string> Solve_2()
    {
        var rope = new Rope(9);
        return new ValueTask<string>($"{rope.Simulate(_instructions)}");
    }

    private class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Point point &&
                   X == point.X &&
                   Y == point.Y;
        }
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    public record Instruction(Direction Direction, int Distance)
    {
        public static Instruction Parse(string instruction)
        {
            var parts = instruction.Split(' ');
            return new Instruction(
                parts[0] switch
                {
                    "U" => Direction.Up,
                    "R" => Direction.Right,
                    "D" => Direction.Down,
                    "L" => Direction.Left,
                    _ => throw new ArgumentException("Invalid direction", nameof(instruction)),
                },
                int.Parse(parts[1]));
        }
    }

    private class Rope
    {
        private readonly List<Point> _line;
        private readonly List<Point> _visitedByTail;

        public Rope(int size)
        {
            _line = Enumerable.Range(0, size + 1).Select(_ => new Point(0, 0)).ToList();
            _visitedByTail = new List<Point>()
            {
                new(0, 0)
            };
        }

        public int Simulate(IEnumerable<Instruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                Move(instruction);
            }

            return _visitedByTail.Count;
        }

        private void Move(Instruction instruction)
        {
            for (var i = 0; i < instruction.Distance; i++)
            {
                switch (instruction.Direction)
                {
                    case Direction.Down:
                        _line[0].Y -= 1;
                        break;
                    case Direction.Up:
                        _line[0].Y += 1;
                        break;
                    case Direction.Left:
                        _line[0].X -= 1;
                        break;
                    case Direction.Right:
                        _line[0].X += 1;
                        break;
                }


                MoveTail();
            }
        }

        private void MoveTail()
        {
            var previousItem = _line[0];
            foreach (var item in _line.Skip(1))
            {
                var xDistance = Math.Abs(previousItem.X - item.X);
                var yDistance = Math.Abs(previousItem.Y - item.Y);

                if (xDistance <= 1 && yDistance <= 1)
                {
                    previousItem = item;
                    continue;
                }

                item.X += GetIncrement(previousItem.X, item.X);
                item.Y += GetIncrement(previousItem.Y, item.Y);

                previousItem = item;

                if (_line.IndexOf(item) != _line.Count - 1) continue;

                if (_visitedByTail.Any(x => x.X == item.X && x.Y == item.Y))
                    continue;

                _visitedByTail.Add(new Point(item.X, item.Y));
            }
        }

        private int GetIncrement(int previous, int current)
        {
            if (previous == current)
                return 0;
            if (previous > current)
                return 1;
            return -1;
        }
    }
}