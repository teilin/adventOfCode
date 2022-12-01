string inputFile = Environment.GetCommandLineArgs()[1];

var input = System.IO.File.ReadLines(inputFile);
var elfCount = 1;
var calories = new Dictionary<int,int>();
var caloriesCurrentElv = 0;

foreach(var line in input)
{
    if(string.IsNullOrEmpty(line)) 
    {   
        calories.Add(elfCount, caloriesCurrentElv);
        ++elfCount;
        caloriesCurrentElv = 0;
    }
    else caloriesCurrentElv += int.Parse(line);
}
calories.Add(elfCount, caloriesCurrentElv);

Console.WriteLine($"Total amount of calories of the elf carring the most: {calories.Values.Max()}");

Console.WriteLine($"Total amount of calories of top tree elves: {calories.OrderByDescending(o => o.Value).Take(3).Sum(s => s.Value)}");