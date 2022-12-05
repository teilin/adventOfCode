public sealed class Day04 : Solver
{
    private IList<Pair> _pairs = new List<Pair>();

    public Day04(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        foreach(var line in Inputs)
        {
            var pairs = line.Split(',',StringSplitOptions.RemoveEmptyEntries);
            var pair1 = pairs[0].Split('-',StringSplitOptions.RemoveEmptyEntries);
            var pair2 = pairs[1].Split('-',StringSplitOptions.RemoveEmptyEntries);
            var section1 = new Section
            {
                StartPosition = long.Parse(pair1[0]),
                EndPosition = long.Parse(pair1[1])
            };
            var section2 = new Section
            {
                StartPosition = long.Parse(pair2[0]),
                EndPosition = long.Parse(pair2[1])
            };
            _pairs.Add(new Pair
            {
                Section1 = section1,
                Section2 = section2
            });
        }
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var count = 0L;
        foreach(var pair in _pairs)
        {
            if(pair.Section1.IsFullyCovered(pair.Section2) || pair.Section2.IsFullyCovered(pair.Section1))
            {
                count++;
            }
        }
        return await Task.FromResult(count);
    }

    protected override async Task<object> Part2()
    {
        var count = 0L;
        foreach(var pair in _pairs)
        {
            if(pair.Section1.IsOverlapping(pair.Section2) || pair.Section2.IsOverlapping(pair.Section1))
            {
                count++;
            }
        }
        return await Task.FromResult(count);
    }

    internal record Pair
    {
        public Section Section1 { get; init; } = new Section();
        public Section Section2 { get; init; } = new Section();
    }

    internal record Section
    {
        public long StartPosition { get; init; }
        public long EndPosition { get; init; }

        public bool IsFullyCovered(Section otherSection)
        {
            return StartPosition <= otherSection.StartPosition && EndPosition >= otherSection.EndPosition;
        }

        public bool IsOverlapping(Section otherSection)
        {
            return StartPosition <= otherSection.EndPosition && otherSection.StartPosition <= EndPosition;
        }
    }
}