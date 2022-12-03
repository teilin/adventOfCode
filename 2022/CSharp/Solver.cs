public abstract class Solver
{
    private readonly string _inputPath;
    private IEnumerable<string> _inputs;
    private string _inputRaw;

    public Solver(string inputPath)
    {
        _inputPath = inputPath;
    }

    private async Task Init()
    {
        _inputs = await File.ReadAllLinesAsync(_inputPath);
        _inputRaw = await File.ReadAllTextAsync(_inputPath);
    }

    public IEnumerable<string> Inputs => _inputs;
    public string RawInput => _inputRaw;

    protected abstract Task Setup();
    protected abstract Task<long> Part1();
    protected abstract Task<long> Part2();

    public async Task Run()
    {
        await Init();
        await Setup();
        Console.WriteLine($"Part 1: {await Part1()}");
        Console.WriteLine($"Part 2: {await Part2()}");
    }
}