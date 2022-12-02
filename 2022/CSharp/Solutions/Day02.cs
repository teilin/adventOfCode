public sealed class Day02 : Solver
{
    private IList<(Card opponent,char me)> _rounds = new List<(Card opponent,char me)>();

    public Day02(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        foreach(var line in _inputs)
        {
            var l = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var o = Transform(l[0].ToCharArray()[0]);
            _rounds.Add((o,l[1].ToCharArray()[0]));
        }
        return Task.CompletedTask;
    }

    protected override async Task<long> Part1()
    {
        var totalScore = 0L;

        foreach(var round in _rounds)
        {
            totalScore += RockPaperScissors(round.opponent,Transform(round.me));
        }

        return await Task.FromResult(totalScore);
    }

    protected override async Task<long> Part2()
    {
        var totalScore = 0L;
        foreach(var round in _rounds)
        {
            var neededoutCome = TransformOutcome(round.me);
            switch(neededoutCome)
            {
                case Outcome.WIN:
                    totalScore += RockPaperScissorOutcome(round.opponent, neededoutCome);
                    break;
                case Outcome.LOSE:
                    totalScore += RockPaperScissorOutcome(round.opponent, neededoutCome);
                    break;
                default:
                    totalScore += RockPaperScissorOutcome(round.opponent, neededoutCome);
                    break;
            }
        }
        return await Task.FromResult(totalScore);
    }

    private long RockPaperScissors(Card opponent, Card me)
    {
        if(opponent == Card.ROCK && me == Card.ROCK) return CalculateScore(me, Outcome.DRAW);
        if(opponent == Card.ROCK && me == Card.PAPER) return CalculateScore(me, Outcome.WIN);
        if(opponent == Card.ROCK && me == Card.SCISSORS) return CalculateScore(me, Outcome.LOSE);

        if(opponent == Card.PAPER && me == Card.ROCK) return CalculateScore(me, Outcome.LOSE);
        if(opponent == Card.PAPER && me == Card.PAPER) return CalculateScore(me, Outcome.DRAW);
        if(opponent == Card.PAPER && me == Card.SCISSORS) return CalculateScore(me, Outcome.WIN);

        if(opponent == Card.SCISSORS && me == Card.ROCK) return CalculateScore(me, Outcome.WIN);
        if(opponent == Card.SCISSORS && me == Card.PAPER) return CalculateScore(me, Outcome.LOSE);
        if(opponent == Card.SCISSORS && me == Card.SCISSORS) return CalculateScore(me, Outcome.DRAW);

        throw new Exception("I will never end here");
    }

    private long RockPaperScissorOutcome(Card opponent, Outcome outcome)
    {
        if(opponent == Card.ROCK && outcome == Outcome.WIN) return CalculateScore(Card.PAPER, outcome);
        if(opponent == Card.ROCK && outcome == Outcome.DRAW) return CalculateScore(Card.ROCK, outcome);
        if(opponent == Card.ROCK && outcome == Outcome.LOSE) return CalculateScore(Card.SCISSORS, outcome);

        if(opponent == Card.PAPER && outcome == Outcome.WIN) return CalculateScore(Card.SCISSORS, outcome);
        if(opponent == Card.PAPER && outcome == Outcome.DRAW) return CalculateScore(Card.PAPER, outcome);
        if(opponent == Card.PAPER && outcome == Outcome.LOSE) return CalculateScore(Card.ROCK, outcome);

        if(opponent == Card.SCISSORS && outcome == Outcome.WIN) return CalculateScore(Card.ROCK, outcome);
        if(opponent == Card.SCISSORS && outcome == Outcome.DRAW) return CalculateScore(Card.SCISSORS, outcome);
        if(opponent == Card.SCISSORS && outcome == Outcome.LOSE) return CalculateScore(Card.PAPER, outcome);

        throw new Exception("I will never end here");
    }

    private long CalculateScore(Card card, Outcome outcome)
    {
        long score = 0L;
        switch(card)
        {
            case Card.ROCK:
                score = 1;
                break;
            case Card.PAPER:
                score = 2;
                break;
            case Card.SCISSORS:
                score = 3;
                break;
        }
        switch(outcome)
        {
            case Outcome.WIN:
                return score+6;
            case Outcome.DRAW:
                return score+3;
            default:
                return score;
        }
    }

    private Card Transform(char c)
    {
        switch(c)
        {
            case 'A':
            case 'X':
                return Card.ROCK;
            case 'B':
            case 'Y':
                return Card.PAPER;
            case 'C':
            case 'Z':
                return Card.SCISSORS;
            default:
                throw new Exception("Unknown");
        }
    }

    private Outcome TransformOutcome(char c)
    {
        switch(c)
        {
            case 'X':
                return Outcome.LOSE;
            case 'Y':
                return Outcome.DRAW;
            default:
                return Outcome.WIN;
        }
    }

    private enum Card
    {
        ROCK,
        PAPER,
        SCISSORS
    }

    private enum Outcome
    {
        WIN,
        LOSE,
        DRAW
    }
}