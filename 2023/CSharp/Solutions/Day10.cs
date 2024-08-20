using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day10 : ISolver
{
    private char[,] _map;

    public Task Solve(Input input)
    {
        return Task.CompletedTask;
    }

    private Task Parse(IList<string> input)
    {
        _map = new char[input.ElementAt(0).Length, input.Count];
        return Task.CompletedTask;
    }
}