using System.Text.RegularExpressions;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day01 : ISolver
{
    public Task Solve(Input input)
    {
        Dictionary<string,int> numbers = new Dictionary<string, int> {{"one",1},{"two",2},{"three",3},{"four",4},{"five",5},{"six",6},{"seven",7},{"eight",8},{"nine",9}};

        var sum = 0;
        foreach(var line in input.inputs)
        {
            var firstNum = string.Empty;
            var currentNum = -1;
            foreach(var ch in line.ToCharArray())
            {
                if(int.TryParse(ch.ToString(), out var test))
                {
                    if(string.IsNullOrEmpty(firstNum)) firstNum = ch.ToString();
                    currentNum = test;
                }
            }
            if(int.TryParse($"{firstNum}{currentNum}", out var c)) sum += c;
        }
        Console.WriteLine($"Part 1 -> {sum}");

        Console.WriteLine($"Part2 -> {input.inputs.Select(CalibrationRegex).Sum()}");

        return Task.CompletedTask;
    }

    private int CalibrationRegex(string line)
    {
        var regexpr = "[1-9]|one|two|three|four|five|six|seven|eight|nine";
        var fsv = new Regex(regexpr);
        var lsv = new Regex(regexpr, RegexOptions.RightToLeft);
        return 
            ToDigit(fsv.Match(line).Value) * 10 + 
            ToDigit(lsv.Match(line).Value);

        static int ToDigit(string value) => value switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => value[0] - '0'
        };
    }
}