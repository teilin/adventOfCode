namespace adventofcode.Day21;

public sealed class DiracDice : ISolver
{
    private int _dice = 0;
    private int _numDieRolled = 0;
    private IDictionary<int,int> _spaces = new Dictionary<int,int>();
    private IDictionary<int,int> _score = new Dictionary<int,int>();

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        var input = await File.ReadAllLinesAsync(inputFilePath);
        var numPlayers = 0;
        foreach(var player in input)
        {
            numPlayers += 1;
            var p = int.Parse(player.Split(": ", StringSplitOptions.RemoveEmptyEntries)[1]);
            _spaces.Add(numPlayers,p);
            _score.Add(numPlayers,0);
        }

        var rounds = 1;
        var isPlayerTurn = 1;
        while(_score.Values.Max() < 1000)
        {
            var moves = ThriughDice()+ThriughDice()+ThriughDice();
            var spaces = 0;
            if(isPlayerTurn==2)
            {
                spaces = _spaces[numPlayers];
                spaces = (spaces+moves-1)%10+1;
                _spaces[isPlayerTurn] = spaces;
                _score[isPlayerTurn] += spaces;
            }
            else
            {
                spaces = _spaces[rounds%numPlayers];
                spaces = (spaces+moves-1)%10+1;
                _spaces[isPlayerTurn] = spaces;
                _score[isPlayerTurn] += spaces;
            }
            rounds += 1;
            isPlayerTurn = isPlayerTurn==1 ? 2 : 1;
        }
        var scoreLoosing = _score.Values.Min();
        Console.WriteLine(scoreLoosing*_numDieRolled);
    }

    public async ValueTask ExecutePart2(string inputFilePath)
    {
        var input = await File.ReadAllLinesAsync(inputFilePath);
        var (pos1,pos2) = (int.Parse(input[0].Split(": ")[1]),int.Parse(input[1].Split(": ")[1]));
        var cache = new Dictionary<string, (long, long)>();
        var (wins1, wins2) = RollDirac(true, pos1, pos2, 0, 0, 1, cache);
        Console.WriteLine($"Player 1 wins {wins1} times.");
        Console.WriteLine($"Player 2 wins {wins2} times.");
    }

    public bool ShouldExecute(int day)
    {
        return day==21;
    }

    private int ThriughDice()
    {
        _numDieRolled += 1;
        _dice += 1;
        if(_dice > 100)
        {
            _dice = 1;
        }
        return _dice;
    }

    static (long wins1, long wins2) RollDirac(bool player1, int pos1, int pos2, int score1, int score2, int roll, Dictionary<string, (long, long)> cache)
    {
        var (w1a, w2a) = SplitUniverse(player1, pos1, pos2, score1, score2, roll, 1, cache);
        var (w1b, w2b) = SplitUniverse(player1, pos1, pos2, score1, score2, roll, 2, cache);
        var (w1c, w2c) = SplitUniverse(player1, pos1, pos2, score1, score2, roll, 3, cache);
        return (w1a + w1b + w1c, w2a + w2b + w2c);
    }

    private static (long wins1, long wins2) SplitUniverse(bool player1, int pos1, int pos2, int score1, int score2, int roll, int dieValue, Dictionary<string, (long, long)> cache)
    {
        var fingerPrint = $"{player1}:{pos1}:{pos2}:{score1}:{score2}:{roll}:{dieValue}";
        if (cache.ContainsKey(fingerPrint))
        {
            return cache[fingerPrint];
        }
        var wins1 = 0L;
        var wins2 = 0L;
        if (roll == 3)
        {
            if (player1)
            {
                pos1 = (pos1 + dieValue - 1) % 10 + 1;
                score1 += pos1;
                if (score1 >= 21)
                {
                    return (1, 0);
                }
            }
            else
            {
                pos2 = (pos2 + dieValue - 1) % 10 + 1;
                score2 += pos2;
                if (score2 >= 21)
                {
                    return (0, 1);
                }
            }
            // switch players
            var (w1, w2) = RollDirac(!player1, pos1, pos2, score1, score2, 1, cache);
            wins1 += w1;
            wins2 += w2;
        }
        else
        {
            var (w1a, w2a) = SplitUniverse(player1, pos1, pos2, score1, score2, roll + 1, dieValue + 1, cache);
            var (w1b, w2b) = SplitUniverse(player1, pos1, pos2, score1, score2, roll + 1, dieValue + 2, cache);
            var (w1c, w2c) = SplitUniverse(player1, pos1, pos2, score1, score2, roll + 1, dieValue + 3, cache);
            wins1 += w1a + w1b + w1c;
            wins2 += w2a + w2b + w2c;
        }	

        cache[fingerPrint] = (wins1, wins2);
        return (wins1, wins2);
    }
}