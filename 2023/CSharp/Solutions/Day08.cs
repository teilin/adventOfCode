using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day08 : ISolver
{
    private char[] _directions;
    private readonly IDictionary<string, Instruction> _map = new Dictionary<string, Instruction>();

    public async Task Solve(Input input)
    {
        await Parse(input.inputs);
        Console.WriteLine($"Part 1 -> {TraverseMapToDestination()}");
        var pathLengths = GetPathLengths();
        Console.WriteLine($"Part 2 -> {LowestCommonMultiple(pathLengths)}");
    }

    private int TraverseMapToDestination()
    {
        var step = 0;
        var currentPositon = "AAA";
        for(var directionIndex=0; directionIndex<_directions.Length; directionIndex++)
        {
            if(currentPositon == "ZZZ") break;
            step++;
            if(_directions[directionIndex] == 'L')
            {
                currentPositon = _map[currentPositon].left;
            }
            if(_directions[directionIndex] == 'R')
            {
                currentPositon = _map[currentPositon].right;
            }
            if(directionIndex == _directions.Length-1) directionIndex = -1;
        }
        return step;
    }

    private List<long> GetPathLengths()
    {
        var nodes = _map.Where(w => w.Key.EndsWith('A')).Select(s => s.Key).ToArray();
        var pathLengths = new List<long>();
        foreach(var node in nodes)
            pathLengths.Add(TraverseMap(node));
        return pathLengths;
    }

    private long TraverseMap(string node)
    {
        var steps = 0;
        var step = 0;        
        while (true)
        {
            steps++;            
            node = _directions[step] == 'L' ? _map[node].left : _map[node].right;
            if (node[2] == 'Z') break;
            step++;
            if (step == _directions.Length) step = 0;
        }
        return steps;
    }

    public static long LowestCommonMultiple(List<long> input)
    {
        var queue = new Queue<long>(input.Count * 2);

        foreach (var item in input)
        {
            queue.Enqueue(item);
        }
        
        while (true)
        {
            long left;
            
            long right;
            
            if (queue.Count == 2)
            {
                left = queue.Dequeue();

                right = queue.Dequeue();

                return left * right / GreatestCommonFactor(left, right);
            }

            left = queue.Dequeue();

            right = queue.Dequeue();

            var lowestCommonMultiple = left * right / GreatestCommonFactor(left, right);

            queue.Enqueue(lowestCommonMultiple);
        }
    }

    private static long GreatestCommonFactor(long left, long right)
    {
        while (left != 0 && right != 0)
        {
            if (left > right)
            {
                left %= right;
            }
            else
            {
                right %= left;
            }
        }
        return left | right;
    }

    private Task Parse(IList<string> input)
    {
        _directions = input.ElementAt(0).ToCharArray();
        for(var i=2;i<input.Count;i++)
        {
            var line = input.ElementAt(i);
            var key = line.Split(" = ", StringSplitOptions.RemoveEmptyEntries)[0];
            var value = line.Split(" = ", StringSplitOptions.RemoveEmptyEntries)[1].Substring(1, line.Split(" = ", StringSplitOptions.RemoveEmptyEntries)[1].Count()-2);
            var coordinate = value.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            if(_map.ContainsKey(key)) throw new Exception("Should not happen...");
            _map.Add(key, new Instruction(coordinate[0], coordinate[1]));
        }
        return Task.CompletedTask;
    }

    record Instruction(string left, string right);
}