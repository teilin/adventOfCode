namespace adventofcode.Day18;

public sealed class Snailfish : ISolver
{
    private const char Open = '[';
    private const char Close = ']';
    private const char Split = ',';
    private string[] _input;

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        _input = await File.ReadAllLinesAsync(inputFilePath);
        var numbers = _input
            .Select(s => GetSnailNumber(s))
            .ToArray();
        var number = numbers[0];
        for(int step=1;step<numbers.Length;step++)
        {
            number = GetSum(number,numbers[step]);
        }
        Console.WriteLine(number.GetMagnitude());
    }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        Int64 max = 0;
        for(var step=0;step<_input.Length;step++)
        {
            for(var i=0;i<_input.Length;i++)
            {
                if(i==step) continue;
                var first = GetSnailNumber(_input[step]);
                var second = GetSnailNumber(_input[i]);
                var magnitude = GetSum(first,second).GetMagnitude();
                max = Math.Max(max,magnitude);
            }
        }
        Console.WriteLine(max);
        return ValueTask.CompletedTask;
    }

    public bool ShouldExecute(int day)
    {
        return day==18;
    }

    private SnailNumber GetSum(SnailNumber first, SnailNumber second)
    {
        var number = new SnailNumber
        {
            Left = first,
            Right = second
        };

        number.Reduce();

        return number;
    }

    private SnailNumber GetSnailNumber(string line)
    {
        SnailNumber? result = null;
        SnailNumber? current = null;
        var left = true;
        string currentValue = string.Empty;
        foreach(char c in line)
        {
            if (c == Open)
            {
                if (result == null)
                {
                    result = new SnailNumber();
                    current = result;
                }
                else
                {
                    var newNumber = new SnailNumber
                    {
                        Parent = current,
                        IsLeft = left,
                        IsRight = !left,
                        Depth = current!.Depth + 1
                    };

                    if (left)
                    {
                        current.Left = newNumber;
                    }
                    else
                    {
                        current.Right = newNumber;
                    }

                    left = true;
                    current = newNumber;
                }
            }
            else if (c == Close)
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    if (left)
                    {
                        current!.LeftValue = long.Parse(currentValue);
                    }
                    else
                    {
                        current!.RightValue = long.Parse(currentValue);
                    }
                }

                currentValue = string.Empty;
                current = current?.Parent;                    
            }
            else if (c == Split)
            {
                if (!string.IsNullOrEmpty(currentValue))
                {
                    if (left)
                    {
                        current!.LeftValue = long.Parse(currentValue);
                    }
                    else
                    {
                        current!.RightValue = long.Parse(currentValue);
                    }
                }

                currentValue = string.Empty;

                left = false;
            }
            else
            {
                currentValue += c;
            }
        }

        return result!;
    }
}