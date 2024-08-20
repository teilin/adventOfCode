using System.Collections;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed partial class Day12 : ISolver
{
    public Task Solve(Input input)
    {
        var springs = Parse([.. input.inputs]);

        var count = springs.Select(s => Process(s.Condition, s.DamagedCounts)).Sum();
        Console.WriteLine($"Part 1 -> {count}");

        var unfolded = springs.Select(s => Unfold(s, 5));
        var completed = 0;
        var total = 0L;
        Parallel.ForEach(unfolded, spr =>
        {
            var count2 = Process(spr.Condition, spr.DamagedCounts);
            var localCompleted = Interlocked.Increment(ref completed);
            var localTotal = Interlocked.Add(ref total, count2);

            Console.WriteLine($"{localTotal,8} ({localCompleted,4}/{springs.Length, 4})");
        });

        return Task.CompletedTask;
    }

    private static Springs Unfold(Springs input, int count)
    {
        var condition = string.Join('?', Enumerable.Range(0, count).Select(_ => input.Condition));

        var damagedCounts =  new List<int>();

        for (var i = 0; i < count; i++)
        {
            damagedCounts.AddRange(input.DamagedCounts);
        }

        return new Springs(condition, [.. damagedCounts]);
    }

    private static long Process(string springs, ImmutableArray<int> damagedCounts)
    {
        return Process(springs, [.. damagedCounts], []);
    }

    private static long Process(string springs, List<int> damagedCounts, Dictionary<string, long> cache)
    {
        var key = $"{springs}-{string.Join(',', damagedCounts.Select(x => x.ToString()))}";
        if (cache.TryGetValue(key, out var value))
        {
            return value;
        }

        value = Count(springs, damagedCounts, cache);
        cache[key] = value;
        return value;
    }

    private static long Count(string springs, List<int> damagedCounts, Dictionary<string, long> cache)
    {
        return springs.FirstOrDefault() switch {
            '.' => HandleWorking(springs, damagedCounts, cache),
            '#' => HandleDamaged(springs, damagedCounts, cache),
            '?' => HandleUnknown(springs, damagedCounts, cache),
            _ => HandleEnd(damagedCounts)
        };
    }

    private static long HandleWorking(string springs, List<int> damagedCounts, Dictionary<string, long> cache) =>
        Process(springs[1..], damagedCounts, cache);

    private static long HandleDamaged(string springs, List<int> damagedCounts, Dictionary<string, long> cache)
    {
        if (damagedCounts.Count == 0)
        {
            return 0;
        }

        var c = damagedCounts[0];
        damagedCounts = damagedCounts[1..];

        var leadingDamagedOrUnknown = springs.TakeWhile(x => x is '#' or '?').Count();

        if (leadingDamagedOrUnknown < c)
        {
            return 0;
        }
        if (springs.Length == c)
        {
            return Process("", damagedCounts, cache);
        }
        if (springs[c] == '#')
        {
            return 0;
        }
        return Process(springs[(c+1)..], damagedCounts, cache);
    }

    private static long HandleUnknown(string springs, List<int> damagedCounts, Dictionary<string, long> cache) =>
        Process("." + springs[1..], damagedCounts, cache) + Process("#" + springs[1..], damagedCounts, cache);

    private static long HandleEnd(List<int> damagedCounts) => damagedCounts.Count == 0 ? 1 : 0;

    private static bool Match(string conditions, ImmutableArray<int> damagedCounts)
    {
        var regex = SplitRegex();
        var damaged = regex.Split(conditions).Where(s => s != string.Empty);
        return damaged.Select(x => x.Length).SequenceEqual(damagedCounts);
    }

    private static IEnumerable<string> PossibleConditions(string conditions)
    {
        var unknownPositions = conditions.Select((c, i) => (Char: c, Index: i))
            .Where(w => w.Char == '?')
            .Select(s => s.Index)
            .ToArray();
        var numCombinations = Math.Pow(2, unknownPositions.Length);
        for(var i=0;i<numCombinations;i++)
        {
            var copy = conditions.ToArray();
            for(var j=0;j<unknownPositions.Length;j++)
            {
                var pos = unknownPositions[j];
                copy[pos] = (i&(1<<j)) == 0 ? '#' : '.';
            }
            yield return new string(copy);
        }
    }

    private static ImmutableArray<Springs> Parse(List<string> input)
    {
        return input
            .Select(line => line.Split(' '))
            .Select(parts => new Springs(
                parts[0], 
                parts[1].Split(',').Select(x => Convert.ToInt32(x)).ToImmutableArray()
            ))
            .ToImmutableArray();
    }

    [GeneratedRegex(@"\.+")]
    private static partial Regex SplitRegex();

    record Springs(string Condition, ImmutableArray<int> DamagedCounts);
}