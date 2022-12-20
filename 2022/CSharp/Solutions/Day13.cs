using System.Text.Json.Nodes;

public sealed class Day13 : Solver
{
    public Day13(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var sum = GetPackets(RawInput)
            .Chunk(2)
            .Select((pair,index) => Compare(pair[0],pair[1]) < 0 ? index+1 : 0)
            .Sum();
        return await Task.FromResult(sum);
    }

    protected override async Task<object> Part2()
    {
        var divider = GetPackets("[[2]]\n[[6]]").ToList();
        var packets = GetPackets(RawInput).Concat(divider).ToList();
        packets.Sort(Compare);
        return await Task.FromResult((packets.IndexOf(divider[0]) + 1) * (packets.IndexOf(divider[1]) + 1));
    }

    IEnumerable<JsonNode> GetPackets(string input) =>
        from line in input.Split("\n") 
        where !string.IsNullOrEmpty(line) 
        select JsonNode.Parse(line);

    private int Compare(JsonNode left, JsonNode right)
    {
        if(left is JsonValue && right is JsonValue)
        {
            return left.GetValue<int>() - right.GetValue<int>();
        }
        var leftArray = left is JsonArray a ? a : new JsonArray(left.GetValue<int>());
        var rightArray = right is JsonArray b ? b : new JsonArray(right.GetValue<int>());
        foreach(var (leftItem, rightItem) in Enumerable.Zip(leftArray,rightArray))
        {
            var c = Compare(leftItem,rightItem);
            if(c != 0) return c;
        }
        return leftArray.Count - rightArray.Count;
    }
}