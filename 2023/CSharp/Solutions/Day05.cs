using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Aoc.Interfaces;
using Microsoft.Extensions.Primitives;

namespace Aoc.Solutions;

public sealed class Day05 : ISolver
{
    public async Task Solve(Input input)
    {
        var mappers = await Parse(input.inputs);

        Console.WriteLine($"Part 1 -> {GetMinSeed(mappers.Seeds, mappers.Mappers)}");

        Console.WriteLine($"Part 2 -> {Part2(input.input)}");
    }

    public static long calc(long seed, List<List<long>> rules)
    {
        foreach (var rule in rules)
        {
            if (seed >= rule[1] && seed < rule[1] + rule[2])
            {
                seed = rule[0] + (seed - rule[1]);
                break;
            }
        }

        return seed;
    }

    public static long find(long seed, List<List<List<long>>> rules)
    {
        var end = long.MaxValue;
        var seedA = seed;
        foreach (var rule in rules) { 
            seedA = calc(seedA, rule);
        }

        end = Math.Min(end, seedA);

        return end;
    }

    private long Part2(string input)
    {
        string[] inputSplit = input.Replace("\r", "").Split(new string[] { "\n\n" }, StringSplitOptions.None);
        
        string[] seeds = inputSplit[0]
            .Split(new string[] { ": " }, StringSplitOptions.None)[1].Trim()
            .Split(' ');

        List<List<long>> seedsRange = new List<List<long>>();

        for (int i = 0; i < seeds.Length; i+=2)
        {
            List<long> range = new List<long>();
            range.Add(long.Parse(seeds[i]));
            range.Add(long.Parse(seeds[i]) + long.Parse(seeds[i+1]));
            seedsRange.Add(range);
        }
        
        List<List<List<long>>> rules = new  List<List<List<long>>>();
        
        for (int i = 1; i < 8; i++)
        {
            var rule = inputSplit[i]
                .Split(new string[] { ":\n" }, StringSplitOptions.None)[1].Trim()
                .Split(new string[] { "\n" }, StringSplitOptions.None);;
            
            List<List<long>> ruleList = new List<List<long>>();
            foreach (var r in rule)
            {
                var rSplit = r.Split(' ');
                List<long> arr = new List<long>();
                arr.Add(long.Parse(rSplit[1]));
                arr.Add(long.Parse(rSplit[0]));
                arr.Add(long.Parse(rSplit[2]));

                ruleList.Add(arr);
            }
            rules.Add(ruleList);
        }

        rules.Reverse();
        long sl = 0;

        while (true)
        {
            long seed = find(sl, rules);
            if (seedsRange.Any(longs => longs[0] <= seed && seed < longs[1]))
            {
                return sl;
            }
            
            sl++;
        }
    }

    private long GetMinSeed(IEnumerable<long> seeds, IEnumerable<Mapper> mappers)
    {
        var min = long.MaxValue;
        foreach(var seed in seeds)
        {
            var result = seed;
            foreach(var map in mappers)
                result = map.Map(result);
            if(result < min)
                min = result;
        }
        return min;
    }

    private async Task<(IEnumerable<long> Seeds,IEnumerable<Mapper> Mappers)> Parse(IList<string> input)
    {
        var seeds = input.ElementAt(0)[6..]
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse);
        var mappers = await ParseMapper(input);
        return (seeds,mappers);
    }

    private async Task<IEnumerable<Mapper>> ParseMapper(IList<string> lines)
    {
        var mappers = new List<Mapper>();
        var current = new Mapper();
        for(var i=2;i<lines.Count;i++)
        {
            var line = lines[i];
            if(string.IsNullOrEmpty(line)) continue;
            if(char.IsDigit(line[0]))
            {
                var mapperNums = line
                    .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(long.Parse).ToArray();
                if(mapperNums.Length == 3)
                    current.AddEntry(new MapperEntity(mapperNums[1], mapperNums[0], mapperNums[2]));
            }
            else
            {
                mappers.Add(current);
                current = new Mapper();
            }
        }
        mappers.Add(current);
        return await Task.FromResult(mappers);
    }

    private sealed class Mapper
    {
        private readonly List<MapperEntity> _entries = [];

        public void AddEntry(MapperEntity entry)
        {
            _entries.Add(entry);
        }

        public long Map(long num)
        {
            var mapEntry = _entries.FirstOrDefault(n => n.InRange(num));
            return mapEntry?.Map(num) ?? num;
        }
    }

    private sealed class MapperEntity
    {
        private readonly MapDescription _map;

        public MapperEntity(long src, long dest, long len)
        {
            _map = new MapDescription(dest, src, len);
        }

        public MapDescription Entity => _map;

        public bool InRange(long num)
        {
            return num >= _map.sourceStart && num <= _map.sourceStart + _map.rangeLength -1;
        }

        public long Map(long num)
        {
            return InRange(num) ? _map.destStart + (num - _map.sourceStart) : num;
        }

        public record MapDescription(long destStart, long sourceStart, long rangeLength);
    }
}