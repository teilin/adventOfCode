namespace adventofcode.Day14;

public sealed class ExtendedPolymerization : ISolver
{
    private IDictionary<string,long> _pairs = new Dictionary<string,long>();
    private IDictionary<string,char> _pairInsertion = new Dictionary<string,char>();
    private char[] _list = new char[0];
    private string[] _input = new string[0];

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        // _input = (await File.ReadAllTextAsync(inputFilePath))
        //     .Split('\n', StringSplitOptions.RemoveEmptyEntries)
        //     .Select(s => s).ToArray();

        // for(var i=1;i<_input.Length;i++)
        // {
        //     var pair = _input[i].Split(" -> ", StringSplitOptions.RemoveEmptyEntries);
        //     _pairInsertion.Add(pair[0],pair[1][0]);
        // }
        // var template = _input[0].ToCharArray();
        // for(var i=0;i<template.Length-1;i++)
        // {
        //     var pair = $"{template[i]}{template[i+1]}";
        //     if(!_pairs.ContainsKey(pair)) _pairs.Add(pair, 1);
        //     else _pairs[pair]++;
        // }
        // for (var i = 0; i < 10; i++) await CalculateSteps();
        // Console.WriteLine(MaxMinDiff());
        var data = await File.ReadAllLinesAsync(inputFilePath);

        var poly = data[0].Select(c => c.ToString()).ToList();

        var instr = new Dictionary<string, string>();

        foreach (var e in data.Skip(2))
        {
            instr.Add(e.Substring(0, 2), e.Substring(6, 1));
        }

        for (int i = 0; i < 10; i++)
        {
            var last = poly.Last();

            poly = poly.Take(poly.Count - 1)
                .SelectMany((c, n) => new[] { c, instr[c + poly[n + 1]] }.ToList()).ToList();
            poly.Add(last);
        }

        var largestSum = -1;
        var leastSum = 99999999;

        while (poly.Count > 0)
        {
            var ch = poly.First();
            var count = poly.Count(s => s == ch);
            poly = poly.Where(c => c != ch).ToList();
            Console.WriteLine($"Char: {ch} Count: {count}");

            if (count > largestSum)
            {
                largestSum = count;
            }

            if (count < leastSum)
            {
                leastSum = count;
            }
        }

        Console.WriteLine($"Difference: {largestSum - leastSum}");
    }

    public async ValueTask ExecutePart2(string inputFilePath)
    {
        //for (var i = 0; i < 30; i++) await CalculateSteps();
        //Console.WriteLine(MaxMinDiff());
        var data = await File.ReadAllLinesAsync(inputFilePath);

        var poly = data[0].Select(c => c.ToString()).ToList();

        var lastChar = poly.Last();

        var instr = new Dictionary<string, string>();

        foreach (var e in data.Skip(2))
        {
            instr.Add(e.Substring(0, 2), e.Substring(6, 1));
        }

        var pairs = new Dictionary<string, Poly14>();

        foreach (var p in instr)
        {
            pairs.Add(p.Key, new Poly14(p.Key, p.Key.Substring(0, 1) + p.Value, p.Value + p.Key.Substring(1, 1)));
        }

        for (int i = 0; i < poly.Count - 1; i++)
        {
            pairs[poly[i] + poly[i+1]].AddCount(1);
        }

        foreach (var p in pairs)
        {
            p.Value.TransferCount();
        }

        for (var i = 0; i < 40; i++)
        {
            foreach (var p in pairs)
            {
                p.Value.AddCount(-p.Value.Count); // Remove this one and replace with the two others
                pairs[p.Value.Pair1].AddCount(p.Value.Count);
                pairs[p.Value.Pair2].AddCount(p.Value.Count);
            }

            foreach (var p in pairs)
            {
                p.Value.TransferCount();
            }
        }

        var counts = new long[40];

        foreach (var p in pairs)
        {
            counts[p.Key[0] - 'A'] += p.Value.Count;
        }

        counts[lastChar[0] - 'A'] += 1;

        long largestSum = -1;
        long leastSum = 9999999999999999;

        for (int i = 0; i < 40; i++)
        {
            if (counts[i] > 0)
            {
                var ch = ((char)('A' + i)).ToString();
                Console.WriteLine($"{ch}: {counts[i]}");

                if (counts[i] > largestSum)
                {
                    largestSum = counts[i];
                }

                if (counts[i] < leastSum)
                {
                    leastSum = counts[i];
                }
            }
        }

        Console.WriteLine($"Difference: {largestSum}-{leastSum} = {largestSum - leastSum}");
    }

    public bool ShouldExecute(int day)
    {
        return day==14;
    }

    private ValueTask CalculateSteps()
    {
        var tmp = new Dictionary<string,long>();
        foreach(var str in _pairs.Keys)
        {
            var newPairs = new string[2] { $"{str.Substring(0,1)}{_pairInsertion[str]}", $"{_pairInsertion[str]}{str.Substring(1,1)}" };
            foreach(var p in newPairs)
            {
                if(!tmp.ContainsKey(p)) tmp.Add(p, 0);
                tmp[p] += _pairs[str];
            }
        }
        _pairs = new Dictionary<string,long>(tmp);
        return ValueTask.CompletedTask;
    }

    private long MaxMinDiff()
    {
        var count = new Dictionary<string,long>();
        foreach(var pair in _pairs.Keys)
        {
            if(!count.ContainsKey(pair.Substring(0,1))) count.Add(pair.Substring(0,1),0);
            count[pair.Substring(0,1)] += _pairs[pair];
        }
        return (count.Max(c => c.Value)+1 - count.Min(c => c.Value));
    }

    /////

    internal class Poly14
    {
        public string Key { get; }

        public string Pair1 { get; }

        public string Pair2 { get; }

        public long Count { get; private set; } = 0;

        private long addCount = 0;

        public Poly14(string key, string pair1, string pair2)
        {
            Key = key;
            Pair1 = pair1;
            Pair2 = pair2;
        }

        public void AddCount(long count)
        {
            addCount += count;
        }

        public void TransferCount()
        {
            Count += addCount;
            addCount = 0;
        }
    }
}