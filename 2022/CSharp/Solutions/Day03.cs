public sealed class Day03 : Solver
{
    private IDictionary<char,long> _prirority = new Dictionary<char,long>();

    public Day03(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        var priCount = 1L;
        foreach(var c in alphabeth) 
        {
            _prirority.Add(Char.ToLower(c), priCount); 
            ++priCount;
        }
        foreach(var c in alphabeth) 
        {
            _prirority.Add(Char.ToUpper(c), priCount); 
            ++priCount;
        }
        return Task.CompletedTask;
    }

    protected override async Task<long> Part1()
    {
        var priSum = 0L;
        foreach(var line in Inputs)
        {
            var chunks = line.Chunk(line.Length/2);
            var c1 = chunks.ElementAt(0).AsEnumerable();
            var c2 = chunks.ElementAt(1).AsEnumerable();
            var intersect = c1.Intersect(c2);
            if(intersect.Any())
            {
                priSum += _prirority[intersect.ElementAt(0)];
            }
        }
        return await Task.FromResult(priSum);
    }

    protected override async Task<long> Part2()
    {
        var priSum = 0L;
        for(var i=0;i<Inputs.Count();i+=3)
        {
            var r1 = Inputs.ElementAt(i).ToCharArray();
            var r2 = Inputs.ElementAt(i+1).ToCharArray();
            var r3 = Inputs.ElementAt(i+2).ToCharArray();
            var intersect = r1.Intersect(r2).Intersect(r3);
            if(intersect.Any())
            {
                priSum += _prirority[intersect.ElementAt(0)];
            }
        }
        return await Task.FromResult(priSum);
    }

    internal char[] alphabeth = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
}