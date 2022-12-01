namespace adventofcode.Day11;

public sealed class DumboOctopus : ISolver
{
    private int[][] _octopuses = new int[10][];

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        _octopuses = (await File.ReadAllLinesAsync(inputFilePath))
            .Select(s => s.ToCharArray().Select(s => int.Parse(s.ToString())).ToArray()).ToArray();

        var flashes = new List<int>();
        var steps = 0;

        while(steps < 100)
        {
            var seen = new HashSet<(int X, int Y)>();
            var toCheck = new Queue<(int x,int y)>();

            for(var y=0;y<_octopuses.Length;y++)
            {
                for(var x=0;x<_octopuses[y].Length;x++)
                {
                    _octopuses[y][x] += 1;
                    if(_octopuses[y][x] > 9)
                    {
                        toCheck.Enqueue((x,y));
                    }
                }
            }

            while(toCheck.Any())
            {
                var (x,y) = toCheck.Dequeue();
                if(seen.Contains((x,y))) continue;
                
                _octopuses[y][x] = 0;
                foreach(var (x1,y1) in ValidNeighbours(x,y))
                {
                    var current = _octopuses[y1][x1];
                    if(current > 0)
                    {
                        _octopuses[y1][x1] += 1;
                    }
                    var newEntry = _octopuses[y1][x1];
                    if(newEntry > 9 && !seen.Contains((x1,y1)))
                    {
                        toCheck.Enqueue((x1,y1));
                    }
                }

                seen.Add((x,y));
            }
            flashes.Add(seen.Count);
            steps += 1;
        }

        Console.WriteLine(flashes.Sum());
    }

    public async ValueTask ExecutePart2(string inputFilePath)
    {
        _octopuses = (await File.ReadAllLinesAsync(inputFilePath))
            .Select(s => s.ToCharArray().Select(s => int.Parse(s.ToString())).ToArray()).ToArray();

        var steps = 0;

        while(true)
        {
            var seen = new HashSet<(int X, int Y)>();
            var toCheck = new Queue<(int x,int y)>();

            for(var y=0;y<_octopuses.Length;y++)
            {
                for(var x=0;x<_octopuses[y].Length;x++)
                {
                    _octopuses[y][x] += 1;
                    if(_octopuses[y][x] > 9)
                    {
                        toCheck.Enqueue((x,y));
                    }
                }
            }

            while(toCheck.Any())
            {
                var (x,y) = toCheck.Dequeue();
                if(seen.Contains((x,y))) continue;
                
                _octopuses[y][x] = 0;
                foreach(var (x1,y1) in ValidNeighbours(x,y))
                {
                    var current = _octopuses[y1][x1];
                    if(current > 0)
                    {
                        _octopuses[y1][x1] += 1;
                    }
                    var newEntry = _octopuses[y1][x1];
                    if(newEntry > 9 && !seen.Contains((x1,y1)))
                    {
                        toCheck.Enqueue((x1,y1));
                    }
                }

                seen.Add((x,y));
            }
            steps += 1;

            var allFlashes = true;
            for(var a=0;a<_octopuses.Length;a++)
            {
                for(var b=0;b<_octopuses[a].Length;b++)
                {
                    if(_octopuses[a][b] != 0) allFlashes = false;
                }
            }
            if(allFlashes) break;
        }

        Console.WriteLine(steps);
    }

    public bool ShouldExecute(int day)
    {
        return day==11;
    }

    private Dictionary<(int x,int y),int> Flash(int x, int y, Dictionary<(int x,int y),int> state)
    {
        var value = ++_octopuses[y][x];
        if(value > 9) value = 0;
        if(state.ContainsKey((x,y)))
        {
            state[(x,y)] = value;
        }
        else
        {
            state.Add((x,y),value);
        }
        return state;
    }

    private IEnumerable<(int x, int y)> ValidNeighbours(int x, int y)
        => Neighbours(x,y).Where(pos => pos.x >= 0 && pos.y >= 0 && pos.x < 10 && pos.y < 10);

    private IEnumerable<(int x, int y)> Neighbours(int x, int y)
    {
        yield return new ValueTuple<int,int>(x + -1,y + -1);
        yield return new ValueTuple<int,int>(x + 0,y + -1);
        yield return new ValueTuple<int,int>(x + 1,y + -1);

        yield return new ValueTuple<int,int>(x + -1,y + 0);
        yield return new ValueTuple<int,int>(x + 0,y + 0);
        yield return new ValueTuple<int,int>(x + 1,y + 0);

        yield return new ValueTuple<int,int>(x + -1,y + 1);
        yield return new ValueTuple<int,int>(x + 0,y + 1);
        yield return new ValueTuple<int,int>(x + 1,y + 1);
    }
}