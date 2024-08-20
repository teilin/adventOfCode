using System.Text;
using System.Text.RegularExpressions;
using Aoc;
using Aoc.Interfaces;
using Aoc.Solutions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<Solver>()
    .AddScoped<ISolver, Day01>()
    .AddScoped<ISolver, Day02>()
    .AddScoped<ISolver, Day03>()
    .AddScoped<ISolver, Day04>()
    .AddScoped<ISolver, Day05>()
    .AddScoped<ISolver, Day06>()
    .AddScoped<ISolver, Day07>()
    .AddScoped<ISolver, Day08>()
    .AddScoped<ISolver, Day09>()
    .AddScoped<ISolver, Day10>()
    .AddScoped<ISolver, Day12>()
    .AddScoped<ISolver, Day13>()
    .AddScoped<ISolver, Day14>()
    .AddScoped<ISolver, Day15>()
    .AddScoped<ISolver, Day16>()
    .AddScoped<ISolver, Day17>()
    .AddScoped<ISolver, Day18>()
    .AddScoped<ISolver, Day19>()
    .BuildServiceProvider();

// serviceProvider
//     .GetService<ILoggerFactory>()
//     .AddConsole(LogLevel.Debug);

var logger = serviceProvider.GetService<ILoggerFactory>()
    .CreateLogger<Program>();
logger.LogDebug("Starting application");

var solver = serviceProvider.GetService<Solver>();

if(solver != null)
{
    //var args = System.Environment.GetCommandLineArgs();
    await solver.Run(args);
}

logger.LogDebug("All done!");

        

        

    

// namespace Aoc
// {
//     public static class StringExtensions
//     {
//         public static string ReverseSubString(this string str, int startPos, int length)
//         {
//             var tmp = str.ToCharArray();
//             var ret = tmp.Take(startPos).Skip(startPos-length).ToArray();
//             return new string(ret);
//         }

//         static public IEnumerable<List<string>> GroupLines(this string [] lines)
//         {
//             var group = new List<string>();

//             foreach(var line in lines)
//             {
//                 if (line == "")
//                 {
//                     yield return group;
//                     group = new List<string>();
//                 }
//                 else
//                 {
//                     group.Add(line);
//                 }
//             }

//             yield return group;
//         }

//         static public string[] ToLines(this string input)
//         {
//             return input.Split("\r\n");
//         }

//         static public int[] ToInts(this string input)
//         {
//             return input.ToInts("\r\n");
//         }

//         static public int[] ToInts(this string input, string splitter)
//         {
//             return input.Split(splitter, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
//         }

//         static public long[] ToLongs(this string input, string splitter)
//         {
//             return input.Split(splitter, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
//         }
//         static public ulong[] ToULongs(this string input, string splitter)
//         {
//             return input.Split(splitter, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray();
//         }

//         static public int[] ToInts(this string[] input)
//         {
//             return input.Select(int.Parse).ToArray();
//         }

//         static public string Sort(this string input)
//         {
//             return string.Concat(input.OrderBy(c => c));
//         }

//         static public int ToInt(this string input)
//         {
//             return int.Parse(input);
//         }

//         static public string ToBits(this int input)
//         {
//             return Convert.ToString(input, 2);
//         }

//         static public string ToBits(this byte input)
//         {
//             return Convert.ToString(input, 2);
//         }

//         static public int IntFromBits(this string input) => input.AsSpan().IntFromBits();

//         static public int IntFromBits(this ReadOnlySpan<char> input)
//         {
//             var v = 0;
//             foreach (var c in input)
//             {
//                 v <<= 1;
//                 if (c == '1')
//                 {
//                     v |= 1;
//                 }
//             }

//             return v;
//         }

//         public static string ReverseString(this string s) => new(s.Reverse().ToArray());
//     }
// }
