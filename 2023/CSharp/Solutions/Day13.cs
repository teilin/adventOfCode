using Aoc.Interfaces;
using Microsoft.VisualBasic;

namespace Aoc.Solutions;

public sealed class Day13 : ISolver
{
    public Task Solve(Input input)
    {
        Console.WriteLine($"Part 1 -> {Part1(input.inputs)}");
        Console.WriteLine($"Part 2 -> {Part2(input.inputs)}");
        return Task.CompletedTask;
    }

    private int Part1(IList<string> input)
    {
        var allGroups = new List<List<string>>();
        var group = new List<string>();
        foreach(var item in input)
        {
            if(string.IsNullOrEmpty(item))
            {
                allGroups.Add(group);
                group = [];
            }
            else
            {
                group.Add(item);
            }
        }
        allGroups.Add(group);

        var colSum = 0;
        var rowSum = 0;

        foreach(var current in allGroups)
        {
            var numCols = current[0].Length;
            var foundVertical = false;
            for(var i=1;i<numCols;i++)
            {
                var prev = new string(current.Select(s => s[i-1]).ToArray());
                var next = new string(current.Select(s => s[i]).ToArray());
                if(prev == next)
                {
                    foundVertical = IsColumnReflection(current, i);
                    colSum += foundVertical ? i : 0;
                    if(foundVertical) break;
                }
            }
            if(!foundVertical)
            {
                for(var i=1;i<current.Count;i++)
                {
                    var prev = current[i-1];
                    var next = current[i];
                    if(prev == next)
                    {
                        var foundHorizontal = IsRowReflection(current, i);
                        rowSum += foundHorizontal ? i : 0;
                        if(foundHorizontal) break;
                    }
                }
            }
        }
        return colSum + 100 * rowSum;
    }

    private int Part2(IList<string> input)
    {
        var allGroups = new List<List<string>>();
        var group = new List<string>();
        foreach (var item in input)
        {
            if (string.IsNullOrEmpty(item))
            {
                allGroups.Add(group);
                group = new List<string>();
            }
            else
            {
                group.Add(item);
            }
        }
        allGroups.Add(group);

        var colSum = 0;
        var rowSum = 0;

        foreach(var current in allGroups)
        {
            var numColumns = current[0].Length;
            var numRows = current.Count;
            for(var y=0;y<numRows;y++)
            {
                for(var x=0;x<numColumns;x++)
                {
                    var matrix = CreateCharMatrix(current);
                    matrix[y][x] = matrix[y][x] == '#' ? '.' : '#';
                    var foundVertical = false;
                    for(var i=1;i<numColumns;i++)
                    {
                        var prev = new string(matrix.Select(s => s[i - 1]).ToArray());
                        var next = new string(matrix.Select(s => s[i]).ToArray());

                        if (prev == next)
                        {
                            var originalReflection = IsColumnReflection(current, i);
                            foundVertical = IsColumnReflection(matrix, i) && !originalReflection;
                            colSum += foundVertical ? i : 0;
                            if (foundVertical)
                            {
                                goto EndOfLoop;
                            }
                        }
                    }
                    if (!foundVertical)
                    {
                        for (int i = 1; i < current.Count; i++)
                        {
                            var prev = new string(matrix[i - 1]);
                            var next = new string(matrix[i]);
                            if (prev == next)
                            {
                                var originalReflection = IsRowReflection(current, i);
                                var foundHorizontal = IsRowReflection(matrix, i) && !originalReflection;
                                rowSum += foundHorizontal ? i : 0;
                                if (foundHorizontal)
                                {
                                    goto EndOfLoop;
                                }
                            }
                        }
                    }
                }
            }
            EndOfLoop:
                continue;
        }
        return colSum + 100 * rowSum;
    }

    private static bool IsRowReflection(List<string> current, int index)
    {
        var aboveIndex = index - 1;
        var belowIndex = index;
        while(aboveIndex >= 0 && belowIndex < current.Count)
        {
            var prev = current[aboveIndex];
            var next = current[belowIndex];
            if(prev != next) return false;
            aboveIndex--;
            belowIndex++;
        }
        return true;
    }

    private static bool IsColumnReflection(List<string> current, int index)
    {
        var leftIndex = index - 1;
        var rightInxex = index;
        while(leftIndex >= 0 && rightInxex < current[0].Length)
        {
            var prev = new string(current.Select(s => s[leftIndex]).ToArray());
            var next = new string(current.Select(s => s[rightInxex]).ToArray());
            if(prev != next) return false;
            leftIndex--;
            rightInxex++;
        }
        return true;
    }

    private static bool IsRowReflection(char[][] current, int i)
    {
        var aboveIndex = i - 1;
        var belowIndex = i;
        while (aboveIndex >= 0 && belowIndex < current.Length)
        {
            var prev = new string(current[aboveIndex]);
            var next = new string(current[belowIndex]);
            if (prev != next)
            {
                return false;
            }
            aboveIndex--;
            belowIndex++;
        }
        return true;
    }

    private static bool IsColumnReflection(char[][] current, int i)
    {
        var leftIndex = i - 1;
        var rightIndex = i;
        while (leftIndex >= 0 && rightIndex < current[0].Length)
        {
            var prev = new string(current.Select(s => s[leftIndex]).ToArray());
            var next = new string(current.Select(s => s[rightIndex]).ToArray());
            if (prev != next)
            {
                return false;
            }
            leftIndex--;
            rightIndex++;
        }
        return true;
    }

    private static char[][] CreateCharMatrix(List<string> current)
    {
        var numColumns = current[0].Length;
        var numRows = current.Count;
        var result = new char[numRows][];
        for(var y=0;y<numRows;y++)
        {
            result[y] = new char[numColumns];
            for(var x=0;x<numColumns;x++)
            {
                result[y][x] = current[y][x];
            }
        }
        return result;
    }
}