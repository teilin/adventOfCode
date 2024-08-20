using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day02 : ISolver
{
    private readonly IDictionary<int, HashSet<Set>> _game = new Dictionary<int, HashSet<Set>>();

    public async Task Solve(Input input)
    {
        await ParseInput(input.inputs);

        // Part 1
        var unvalidGames = new List<int>();
        foreach(var game in _game)
        {
            var isValid = new Dictionary<string, bool>
            {
                { "red", true },
                { "green", true },
                { "blue", true }
            };
            foreach(var set in game.Value)
            {
                if(set != null)
                {
                    foreach(var pick in set.pics)
                    {
                        if(pick.color == "red" && pick.number > 12) isValid["red"] = false;
                        if(pick.color == "green" && pick.number > 13) isValid["green"] = false;
                        if(pick.color == "blue" && pick.number > 14) isValid["blue"] = false;
                    }
                }
            }

            if(isValid.Any(c => c.Value == false)) unvalidGames.Add(game.Key);
        }
        var validGames = _game.Keys.Except(unvalidGames);

        Console.WriteLine($"Part 1 -> {validGames.Sum()}");

        // Part 2
        var pows = new List<int>();
        foreach(var game in _game)
        {
            var minNumberCubes = new Dictionary<string, int>
            {
                {"red",0},
                {"green",0},
                {"blue",0}
            };
            foreach(var set in game.Value)
            {
                if(set != null)
                {
                    foreach(var pick in set.pics)
                    {
                        if(minNumberCubes[pick.color] < pick.number) minNumberCubes[pick.color] = pick.number;
                    }
                }
            }
            pows.Add(minNumberCubes["red"]*minNumberCubes["blue"]*minNumberCubes["green"]);
        }
        Console.WriteLine($"Part 2 -> {pows.Sum()}");
    }

    private Task ParseInput(IList<string> input)
    {
        foreach(var game in input)
        {
            var tmp  = game.Split(": ");

            var gameId = int.Parse(tmp[0].Split(' ')[1]);
            var sets = tmp[1].Split("; ");
            foreach(var set in sets)
            {
                var picks = set.Split(", ");
                var t = new Pick[picks.Length];
                for(var i=0;i<picks.Length;i++)
                {
                    var p = picks[i].Split(' ');
                    t[i] = new Pick(p[1], int.Parse(p[0]));
                }
                if(_game.ContainsKey(gameId))
                {
                    _game[gameId].Add(new Set(t));
                }
                else
                {
                    var tmpHash = new HashSet<Set>();
                    tmpHash.Add(new Set(t));
                    _game.Add(gameId, tmpHash);
                }
            }
        }
        return Task.CompletedTask;
    }

    private record Set(Pick[] pics);
    private record Pick(string color, int number);
}