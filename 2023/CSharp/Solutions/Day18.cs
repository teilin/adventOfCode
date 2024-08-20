using System.Dynamic;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day18 : ISolver
{
    public async Task Solve(Input input)
    {
        Console.WriteLine($"Part 1 -> {Part1(input.inputs)}");
        Console.WriteLine($"Part 2 -> {Part2(input.inputs)}");
    }

    private int Part1(IList<string> input)
    {
        var maxX = 0;
        var maxY = 0;
        var minX = 0;
        var minY = 0;
        var curX = 0;
        var curY = 0;

        foreach (var line in input)
        {
            var parts = line.Split();
            var direction = parts[0];
            var length = int.Parse(parts[1]);

            (curX, curY) = direction switch
            {
                "U" => (curX, curY - length),
                "D" => (curX, curY + length),
                "L" => (curX - length, curY),
                "R" => (curX + length, curY),
                _ => throw new NotImplementedException(),
            };

            maxX = Math.Max(curX, maxX);
            minX = Math.Min(curX, minX);
            maxY = Math.Max(curY, maxY);
            minY = Math.Min(curY, minY);
        }

        var offsetX = Math.Abs(minX);
        var offsetY = Math.Abs(minY);

        var grid = new bool[maxY + offsetY + 1, maxX + offsetX + 1];

        curX += offsetX;
        curY += offsetY;

        foreach (var line in input)
        {
            var parts = line.Split();
            var direction = parts[0];
            var length = int.Parse(parts[1]);

            for (var i = 0; i < length; i++)
            {
                grid[curY, curX] = true;

                if (direction == "U")
                {
                    curY -= 1;
                }
                else if (direction == "D")
                {
                    curY += 1;
                }
                else if (direction == "L")
                {
                    curX -= 1;
                }
                else if (direction == "R")
                {
                    curX += 1;
                }
            }
        }

        var currentPos = new Coord(offsetX, offsetY);

        var polygon = new List<Coord> {
            currentPos
        };

        var boundary = 0;

        foreach (var line in input)
        {
            var parts = line.Split();
            var direction = parts[0];
            var length = int.Parse(parts[1]);
            boundary += length;

            if (direction == "U")
            {
                currentPos = currentPos with { Y = currentPos.Y - length };
            }
            else if (direction == "D")
            {
                currentPos = currentPos with { Y = currentPos.Y + length };
            }
            else if (direction == "L")
            {
                currentPos = currentPos with { X = currentPos.X - length };
            }
            else if (direction == "R")
            {
                currentPos = currentPos with { X = currentPos.X + length };
            }

            polygon.Add(currentPos);
        }

        var area = 0;

        for (var i = 0; i < polygon.Count; i++)
        {
            area += polygon[i].Y * (polygon[(i + polygon.Count - 1) % polygon.Count].X - polygon[(i + 1) % polygon.Count].X);
        }

        area = Math.Abs(area) / 2;
        var iPoints = area - (boundary / 2) + 1;

        return iPoints + boundary;
    }

    private long Part2(IEnumerable<string> input)
    {
        var maxX = 0L;
        var maxY = 0L;
        var minX = 0L;
        var minY = 0L;
        var curX = 0L;
        var curY = 0L;

        foreach (var line in input)
        {
            var parts = line.Split().Last();
            var direction = parts[7..8];
            var length = Convert.ToInt64(parts[2..7], 16);

            (curX, curY) = direction switch
            {
                "3" => (curX, curY - length),
                "1" => (curX, curY + length),
                "2" => (curX - length, curY),
                "0" => (curX + length, curY),
                _ => throw new NotImplementedException(),
            };

            maxX = Math.Max(curX, maxX);
            minX = Math.Min(curX, minX);
            maxY = Math.Max(curY, maxY);
            minY = Math.Min(curY, minY);
        }

        var offsetX = Math.Abs(minX);
        var offsetY = Math.Abs(minY);

        var currentPos = new LongCoord(offsetX, offsetY);

        var polygon = new List<LongCoord> {
            currentPos
        };

        var boundary = 0L;

        foreach (var line in input)
        {
            var parts = line.Split().Last();
            var direction = parts[7..8];
            var length = Convert.ToInt64(parts[2..7], 16);

            boundary += length;

            if (direction == "3")
            {
                currentPos = currentPos with { Y = currentPos.Y - length };
            }
            else if (direction == "1")
            {
                currentPos = currentPos with { Y = currentPos.Y + length };
            }
            else if (direction == "2")
            {
                currentPos = currentPos with { X = currentPos.X - length };
            }
            else if (direction == "0")
            {
                currentPos = currentPos with { X = currentPos.X + length };
            }

            polygon.Add(currentPos);
        }

        var area = 0L;

        for (var i = 0; i < polygon.Count; i++)
        {
            area += polygon[i].Y * (polygon[(i + polygon.Count - 1) % polygon.Count].X - polygon[(i + 1) % polygon.Count].X);
        }

        area = Math.Abs(area) / 2;
        var iPoints = area - (boundary / 2) + 1;

        return iPoints + boundary;
    }

    record Coord(int X, int Y);
    record LongCoord(long X, long Y);
}