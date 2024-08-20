using System.Text;
using Aoc.Interfaces;

namespace Aoc;

public record Input(string input, IList<string> inputs, string[] inputArray);

public sealed class Solver
{
    private IEnumerable<ISolver> _solvers;

    public Solver(IEnumerable<ISolver> solvers)
    {
        _solvers = solvers;
    }

    public async Task Run(string[] args)
    {
        if(args.Length != 2) throw new Exception("Need two arguments");
        var filePath = args[0];
        if(filePath == null) throw new NullReferenceException("Filepath need to first argument");
        var puzzelInput = ReadInput(filePath);
        var solver = _solvers.FirstOrDefault(w => w.GetType().Name == args[1]) ?? throw new NullReferenceException("Solver could not be found");
        await solver.Solve(puzzelInput);
    }

    private static Input ReadInput(string puzleInputPath)
    {
        return new Input(File.ReadAllText(puzleInputPath, Encoding.UTF8), File.ReadAllLines(puzleInputPath, Encoding.UTF8), File.ReadAllLines(puzleInputPath, Encoding.UTF8));
    }
}