using System.Text.RegularExpressions;

namespace adventofcode.Day17;

public sealed class TrickShot : ISolver
{
    private string _input;

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        // target area: x=20..30, y=-10..-5
        _input = (await File.ReadAllTextAsync(inputFilePath));
        //     .Replace("target area: ","")
        //     .Split(", ", StringSplitOptions.RemoveEmptyEntries);
        // var xpos = input[0].Replace("x=","").Split("..",StringSplitOptions.RemoveEmptyEntries);
        // var ypos = input[1].Replace("y=","").Split("..",StringSplitOptions.RemoveEmptyEntries);

        // StartTargetArea = (int.Parse(xpos[0]),int.Parse(ypos[1]));
        // EndTargetArea = (int.Parse(xpos[1]),int.Parse(ypos[0]));

        Console.WriteLine(Solve(_input).Max());
    }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        Console.WriteLine(Solve(_input).Count());
        return ValueTask.CompletedTask;
    }

    public bool ShouldExecute(int day)
    {
        return day==17;
    }

    private IEnumerable<int> Solve(string input)
    {
        var m = Regex.Matches(input, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray();

        // Get the target rectangle
        var (xMin, xMax) = (m[0], m[1]);
        var (yMin, yMax) = (m[2], m[3]);

        // Bounds for the initial horizontal and vertical speeds:
        var vx0Min = 0;     // Because vx is non negative
        var vx0Max = xMax;  // For bigger values we jump too much to the right in the first step
        var vy0Min = yMin;  // For smaller values we jump too deep in the first step
        var vy0Max = -yMin; // 🍎 Newton says that when the falling probe reaches y = 0, it's speed is -vy0.
                            // In the next step we go down to -vy0, which should not be deeper than yMin.
        
        // Run the simulation in the given bounds, maintaining maxY
        for (var vx0 = vx0Min; vx0 <= vx0Max; vx0++) {
            for (var vy0 = vy0Min; vy0 <= vy0Max; vy0++) {

                var (x, y, vx, vy) = (0, 0, vx0, vy0);
                var maxY = 0;

                // as long as there is any chance to reach the target rectangle:
                while (x <= xMax && y >= yMin) {
                   
                    x += vx;
                    y += vy;
                    vy -= 1;
                    vx = Math.Max(0, vx - 1);
                    maxY = Math.Max(y, maxY);

                    // if we are within target, yield maxY:
                    if (x >= xMin && x <= xMax && y >= yMin && y <= yMax) {
                        yield return maxY;
                        break;
                    }
                }
            }
        }
    }
}