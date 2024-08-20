using Aoc.Interfaces;
using MathNet.Numerics;

namespace Aoc.Solutions;

public sealed class Day09 : ISolver
{
    public Task Solve(Input input)
    {
        Console.WriteLine($"Part 1 -> {SolvePuzle(false, input.inputs)}");
        Console.WriteLine($"Part 2 -> {SolvePuzle(true, input.inputs)}");
        return Task.CompletedTask;
    }

    private long SolvePuzle(bool isPart2, IList<string> inputs)
    {
        var totalResult = 0L;
        foreach(var line in inputs)
        {
            var seeds = new string(line.Where(w => char.IsNumber(w) || w == ' ' || w == '-').ToArray())
                .Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            if(isPart2) seeds = seeds.Reverse().ToArray();

            var result = seeds[^1];
            var i = 1;
            var diff = seeds[^i] - seeds[^(i+1)];
            while(i<seeds.Length-1)
            {
                result += diff;
                i++;
                diff = seeds[^1];
                var add = -1;
                for(var j=1; j<=i;j++)
                {
                    var leftTurnCombinations = (long)SpecialFunctions.Binomial(i,j);
                    diff += add * leftTurnCombinations * seeds[^(j+1)];
                    add *= -1;
                }
            }
            totalResult += result;
        }
        return totalResult;
    }
}