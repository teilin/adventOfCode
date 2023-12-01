public sealed class Day25 : Solver
{
    public Day25(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var sum = Inputs
            .Select(line => SnafuToLong(line))
            .Sum();
        return await Task.FromResult(LongToSnafu(sum));
    }

    protected override async Task<object> Part2()
    {
        return await Task.FromResult(0);
    }

    private long Snafu(string number)
    {
        var numbers = number.ToCharArray()
            .Select(s => Snafu(s)).ToArray();

        var retNumber = 0;
        var powerOf = Convert.ToUInt32(numbers.Length-1);
        for(var i=0;i<numbers.Length;i++)
        {
            retNumber += (numbers[i]*IntPow(5,powerOf));
            powerOf--;
        }
        return retNumber;
    }

    private int Snafu(char c)
    {
        if(c == '-') return -1;
        else if(c == '=') return -2;
        else return Convert.ToInt32(c.ToString());
    }

    private int IntPow(int x, uint pow)
    {
        int ret = 1;
        while ( pow != 0 )
        {
            if ( (pow & 1) == 1 )
                ret *= x;
            x *= x;
            pow >>= 1;
        }
        return ret;
    }

    long SnafuToLong(string snafu) 
    {
        long res = 0L;
        foreach (var digit in snafu) 
        {
            res = res * 5;
            switch (digit) 
            {
                case '=': res += -2; break;
                case '-': res += -1; break;
                case '0': res += 0; break;
                case '1': res += 1; break;
                case '2': res += 2; break;
            }
        }
        return res;
    }

    string LongToSnafu(long d) 
    {
        var res = "";
        while (d > 0) {
            switch (d % 5) {
                case 0: res = '0' + res; break;
                case 1: res = '1' + res; break;
                case 2: res = '2' + res; break;
                case 3: d+=5; res = '=' + res; break; 
                case 4: d+=5; res = '-' + res; break;
            }
            d /= 5;
        }
        return res;
    }
}