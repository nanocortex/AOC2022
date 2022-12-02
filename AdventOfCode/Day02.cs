namespace AdventOfCode;

public enum Move
{
    Rock,
    Paper,
    Scissors
}

public enum Outcome
{
    Win,
    Lose,
    Draw
}

public sealed class Day02 : BaseDay
{
    private readonly string[] _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = 0;
        foreach (var line in _input)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var op = GetOpMove(parts[0]);

            var me = parts[1] switch
            {
                "X" => Move.Rock,
                "Y" => Move.Paper,
                "Z" => Move.Scissors,
                _ => throw new ArgumentOutOfRangeException()
            };

            var score = GetScore(op, me);
            var moveValue = GetMoveValue(me);

            sum += score + moveValue;
        }

        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        var sum = 0;
        foreach (var line in _input)
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var op = GetOpMove(parts[0]);
            var result = GetOutcome(parts[1]);
            var me = GetOutcomeMove(op, result);
            var score = GetScore(op, me);
            var moveValue = GetMoveValue(me);
            sum += score + moveValue;
        }

        return new ValueTask<string>($"{sum}");
    }

    private static Outcome GetOutcome(string outcome)
    {
        return outcome switch
        {
            "X" => Outcome.Lose,
            "Y" => Outcome.Draw,
            "Z" => Outcome.Win,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Move GetOutcomeMove(Move opMove, Outcome outcome)
    {
        if (outcome == Outcome.Draw)
            return opMove;

        if (outcome == Outcome.Lose)
        {
            switch (opMove)
            {
                case Move.Rock:
                    return Move.Scissors;
                case Move.Paper:
                    return Move.Rock;
                case Move.Scissors:
                    return Move.Paper;
            }
        }

        return opMove switch
        {
            Move.Rock => Move.Paper,
            Move.Paper => Move.Scissors,
            _ => Move.Rock
        };
    }

    private static Move GetOpMove(string move)
    {
        return move switch
        {
            "A" => Move.Rock,
            "B" => Move.Paper,
            "C" => Move.Scissors,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private int GetMoveValue(Move move) =>
        move switch
        {
            Move.Rock => 1,
            Move.Paper => 2,
            Move.Scissors => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(move), move, null)
        };

    private int GetScore(Move op, Move me)
    {
        if (op == me)
            return 3;

        if (op == Move.Rock && me == Move.Paper || op == Move.Paper && me == Move.Scissors || op == Move.Scissors && me == Move.Rock)
            return 6;

        return 0;
    }
}