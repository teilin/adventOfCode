public sealed class Day20 : Solver
{
    private int[] _initialLIst;

    public Day20(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        var tmp = new List<int>();
        foreach(var number in Inputs)
        {
            tmp.Add(int.Parse(number));
        }
        _initialLIst = tmp.ToArray();
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var dict = new Dictionary<int, int>();
        var position = 1;
        while(dict.Count<=3)
        {

        }
        return await Task.FromResult(dict[1000]+dict[2000]+dict[3000]);
    }

    protected override async Task<object> Part2()
    {
        return await Task.FromResult(0);
    }

    private int GetNthNumber(int[] list, int number)
    {
        return list[number%list.Length];
    }
}