var inputFilePath = Environment.GetCommandLineArgs()[1];

var day1 = new Day01(inputFilePath);

await day1.Run();