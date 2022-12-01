var inputFilePath = Environment.GetCommandLineArgs()[1];
var solution = Environment.GetCommandLineArgs()[2];

var solutions = new Dictionary<string,Solver>()
{
    {"Day01", new Day01(inputFilePath)},
    {"Day02", new Day02(inputFilePath)}
};

if(solutions.ContainsKey(solution))
{
    var executingSolution = solutions[solution];
    await executingSolution.Run();
}
else
{
    Console.WriteLine("No solution found");
}