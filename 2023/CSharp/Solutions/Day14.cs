using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day14 : ISolver
{
    public Task Solve(Input input)
    {
        Console.WriteLine($"Part 1 -> {CalculateLoad(Cicle(Parse(input.inputs), [(0,-1)]))}");
        Console.WriteLine($"Part 2 -> {CalculateLoad(TiltCycles(1_000_000_000, Parse(input.inputs)))}");
        return Task.CompletedTask;
    }

    private static long CalculateLoad(char[][] matrix)
    {
        var totalLoad = 0L;
        var numRows = matrix.Length;
        for(var y=0;y<matrix.Length;y++)
        {
            var countSlideRocks = 0;
            for(var x=0;x<matrix[y].Length;x++)
            {
                if(matrix[y][x]=='O') countSlideRocks++;
            }
            totalLoad += countSlideRocks*(numRows-y);
        }
        return totalLoad;
    }

    private static char[][] Cicle(char[][] matrix, (int x, int y)[] directions)
    {
        var canMove = true;
        foreach(var direction in directions)
        {
            while(canMove)
            {
                var moves = new Dictionary<(int x, int y), bool>();
                for(var y=0;y<matrix.Length;y++)
                {
                    for(var x=0;x<matrix[y].Length;x++)
                    {
                        moves.Add((x,y), false);
                        if(x+direction.x >= 0 && x+direction.x < matrix[y].Length && y+direction.y >= 0 && y+direction.y < matrix.Length)
                        {
                            var isSlideRock = matrix[y][x] == 'O' ? true : false;
                            var move = matrix[y+direction.y][x+direction.x] == '.' ? true : false;
                            if(isSlideRock && move)
                            {
                                moves[(x,y)] = true;
                                matrix[y+direction.y][x+direction.x] = matrix[y][x];
                                matrix[y][x] = '.';
                            }
                        }
                    }
                }
                if(moves.Values.All(w => !w)) canMove = false;
            }
        }
        return matrix;
    }

    private static char[][] TiltCycles(int totalCycles, char[][] platform)
    {
        var seen = new Dictionary<long, int>();

        for (var cycle = 1; cycle <= totalCycles; cycle++)
        {
            DoCycle();

            var key = Hash(platform);
            if (seen.TryGetValue(key, out int cycleStart))
            {
                var cycleLength = cycle - cycleStart;
                var remainingCycles = (totalCycles - cycleStart) % cycleLength;
                for (var i = 0; i < remainingCycles; i++) DoCycle();
                return platform;
            }
            else
            {
                seen.Add(key, cycle);
            }
        }

        return platform;

        void DoCycle()
        {
            Tilt(platform, Direction.N);
            Tilt(platform, Direction.W);
            Tilt(platform, Direction.S);
            Tilt(platform, Direction.E);
        }
    }

    private static long Hash(char[][] platform) 
        => platform.Select((row, y) => row.Select((c, x) => c * (long)x).Sum() * y).Sum();

    private static char[][] Tilt(char[][] platform, Direction direction)
    {
        var (dy, dx, ystart, yend, ystep, xstart, xend, xstep) = direction switch
        {
            Direction.N => (-1, +0,                  +1,  platform.Length, +1, 0, platform[0].Length, +1),
            Direction.S => (+1, +0, platform.Length - 2, -1,               -1, 0, platform[0].Length, +1),

            Direction.W => (+0, -1, 0, platform.Length, +1, 1,                       platform[0].Length, +1),
            Direction.E => (+0, +1, 0, platform.Length, +1, platform[0].Length - 2, -1,                  -1),

            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        for (var y = ystart; y != yend; y += ystep)
            for (var x = xstart; x != xend; x += xstep)
            {
                if (platform[y][x] != 'O') continue;

                var y2 = y + dy;
                var x2 = x + dx;
                while (y2 >= 0 && y2 < platform.Length
                    && x2 >= 0 && x2 < platform[0].Length
                    && platform[y2][x2] == '.')
                {
                    platform[y2][x2] = 'O';
                    platform[y2-dy][x2-dx] = '.';
                    y2 += dy;
                    x2 += dx;
                }
            }

        return platform;
    }

    private static char[][] Parse(IList<string> input)
    {
        var matrix = new char[input.Count][];
        for(var y=0;y<input.Count;y++)
        {
            matrix[y] = input.ElementAt(y).ToCharArray();
        }
        return matrix;
    }

    enum Direction
    {
        N,W,S,E
    }
}