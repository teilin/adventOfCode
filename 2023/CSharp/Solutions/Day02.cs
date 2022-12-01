public sealed class Day02 : Solver
{
    private int elfCount = 1;
    private IDictionary<int,long> calories = new Dictionary<int,long>();
    private int caloriesCurrentElv = 0;

    public Day02(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<long> Part1()
    {
        return await Task.FromResult(0L);
    }

    protected override async Task<long> Part2()
    {
        return await Task.FromResult(0L);
    }
}