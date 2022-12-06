public sealed class Day06 : Solver
{
    public Day06(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var rsStr = string.Empty;
        foreach(var line in Inputs)
        {
            var startPosition = 0;
            while(true)
            {
                var chars = line.Substring(startPosition, 4).ToCharArray().Distinct();
                if(chars.Count() == 4)
                {
                    var endPosition = startPosition + 4;
                    rsStr += " " + endPosition;
                    break;
                }
                startPosition++;
            }
        }
        return await Task.FromResult(rsStr);
    }

    protected override async Task<object> Part2()
    {
        var rsStr = string.Empty;
        foreach(var line in Inputs)
        {
            var startPosition = 0;
            while(true)
            {
                var chars = line.Substring(startPosition, 14).ToCharArray().Distinct();
                if(chars.Count() == 14)
                {
                    var endPosition = startPosition + 14;
                    rsStr += " " + endPosition;
                    break;
                }
                startPosition++;
            }
        }
        return await Task.FromResult(rsStr);
    }
}