using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day15 : ISolver
{
    public Task Solve(Input input)
    {
        var initializationSequence = Parse(input.input);

        Console.WriteLine($"Part 1 -> {Part1(initializationSequence)}");

        Console.WriteLine($"Part 2 -> {Part2(input.inputs.ElementAt(0))}");

        return Task.CompletedTask;
    }

    private IList<char[]> Parse(string input)
    {
        return input
            .Split(',',StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.ToCharArray())
            .ToList();
    }

    private long Part1(IList<char[]> sequence)
    {
        var sum = 0L;
        foreach(var seq in sequence)
        {
            var hash = new AocHashAlg();
            foreach(var c in seq)
            {
                hash.AddCharacter(c);
            }
            sum += hash.CurrentValue;
        }
        return sum;
    }

    private long Part2(string input)
    {
        var instructions = input.Split(',');
        var boxes = Enumerable.Range(0, 256)
            .Select(x => new List<(string label, int focus)>())
            .ToList();

        foreach (var instruction in instructions)
        {
            var m = Regex.Match(instruction, @"^(?<l>.*)(?<op>[=-])(?<f>\d*)$");

            var box = m.Groups["l"].Value.Aggregate((byte)0, (a, c) => (byte)((a + (byte)c) * 17));
            var lens = boxes[box].FirstOrDefault(l => l.label == m.Groups["l"].Value);
            if (m.Groups["op"].Value == "-")
                boxes[box].Remove(lens);
            else
            {
                var f = int.Parse(m.Groups["f"].ValueSpan);
                var newLens = (m.Groups["l"].Value, f);

                if (lens.label is null)
                    boxes[box].Add(newLens);
                else
                {
                    var i = boxes[box].IndexOf(lens);
                    boxes[box].RemoveAt(i);
                    boxes[box].Insert(i, newLens);
                }
            }
        }

        return boxes
            .SelectMany((b, bi) => b.Select((l, li) => (bi + 1) * (li + 1) * l.focus))
            .Sum();
    }

    private sealed class AocHashAlg
    {
        private int _currentValue;

        public int CurrentValue => _currentValue;

        public void AddCharacter(char character) => Add(character);

        public int Add(string str)
        {
            foreach(var c in str.ToCharArray())
            {
                Add(c);
            }
            return _currentValue;
        }

        private void Add(char character)
        {
            var asciiValue = (int)character;
            _currentValue += (int)asciiValue;
            _currentValue *= 17;
            _currentValue %= 256;
        }
    }
}