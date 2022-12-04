// if(Environment.GetCommandLineArgs().Count() != 2)
// {
//     Console.WriteLine("Arguments missing...");
//     Console.WriteLine("First argument need to be path to puzzle input and second should be the day name.");
//     return;
// }

var inputFilePath = Environment.GetCommandLineArgs()[1];
var solution = Environment.GetCommandLineArgs()[2];

var solutions = new Dictionary<string,Solver>()
{
    {"Day01", new Day01(inputFilePath)},
    {"Day02", new Day02(inputFilePath)},
    {"Day03", new Day03(inputFilePath)},
    {"Day04", new Day04(inputFilePath)}
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