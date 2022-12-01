public abstract class Solver
{
    protected readonly IEnumerable<string> _inputs;

    public Solver(string inputPath)
    {
        _inputs = System.IO.File.ReadLines(inputPath);
    }

    protected abstract Task Setup();
    protected abstract Task<long> Part1();
    protected abstract Task<long> Part2();

    public async Task Run()
    {
        await Setup();
        Console.WriteLine($"Part 1: {await Part1()}");
        Console.WriteLine($"Part 2: {await Part2()}");
    }
}