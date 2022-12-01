using System.Collections.Immutable;

namespace adventofcode.Day12;

public sealed class PassagePathing : ISolver
{
    private string[] _input = new string[0];

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        _input = (await File.ReadAllLinesAsync(inputFilePath));

        var map = CreateMap(_input);

        int pathCount(string currentCave, ImmutableHashSet<string> visiedCaves)
        {
            if(currentCave.Equals("end")) return 1;
            var res = 0;
            foreach(var cave in map[currentCave])
            {
                var isBigCave = IsUpper(cave);
                var seen = visiedCaves.Contains(cave);
                if(!seen || isBigCave)
                {
                    res += pathCount(cave, visiedCaves.Add(cave));
                }
            }
            return res;
        }
        var numRoutes = pathCount("start", ImmutableHashSet.Create<string>("start"));
        Console.WriteLine(numRoutes);
    }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        var map = CreateMap(_input);

        int pathCount(string currentCave, ImmutableHashSet<string> visiedCaves, bool anySmallCaveWasVisitedTwice)
        {
            if(currentCave.Equals("end")) return 1;
            var res = 0;
            foreach(var cave in map[currentCave])
            {
                var isBigCave = IsUpper(cave);
                var seen = visiedCaves.Contains(cave);
                if(!seen || isBigCave)
                {
                    res += pathCount(cave, visiedCaves.Add(cave), anySmallCaveWasVisitedTwice);
                }
                else if(!isBigCave && cave != "start" && !anySmallCaveWasVisitedTwice)
                {
                    res += pathCount(cave, visiedCaves, true);
                }
            }
            return res;
        }
        var numRoutes = pathCount("start", ImmutableHashSet.Create<string>("start"), false);
        Console.WriteLine(numRoutes);
        return ValueTask.CompletedTask;
    }

    public bool ShouldExecute(int day)
    {
        return day==12;
    }

    private bool IsUpper(string str)
    {
        return str.Any(c => char.IsUpper(c));
    }

    private IDictionary<string,string[]> CreateMap(string[] input)
    {
        var connections =
            from line in input
            let parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries)
            let caveA = parts[0]
            let caveB = parts[1]
            from connection in new[] { (From: caveA, To: caveB), (From: caveB, To: caveA) }
            select connection;

        return (
            from p in connections
            group p by p.From into g
            select g
        ).ToDictionary(g => g.Key, g => g.Select(connnection => connnection.To).ToArray());
    }
}