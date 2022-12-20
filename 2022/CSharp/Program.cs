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
    {"Day04", new Day04(inputFilePath)},
    {"Day05", new Day05(inputFilePath)},
    {"Day06", new Day06(inputFilePath)},
    {"Day07", new Day07(inputFilePath)},
    {"Day08", new Day08(inputFilePath)},
    {"Day09", new Day09(inputFilePath)},
    {"Day10", new Day10(inputFilePath)},
    {"Day11", new Day11(inputFilePath)},
    {"Day12", new Day12(inputFilePath)},
    {"Day13", new Day13(inputFilePath)},
    {"Day14", new Day14(inputFilePath)},
    {"Day15", new Day15(inputFilePath)},
    {"Day16", new Day16(inputFilePath)},
    {"Day19", new Day19(inputFilePath)},
    {"Day20", new Day20(inputFilePath)}
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