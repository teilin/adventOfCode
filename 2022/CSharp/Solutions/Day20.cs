public sealed class Day20 : Solver
{
    public Day20(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        return await Task.FromResult(GetCoordinates(1,1));
    }

    protected override async Task<object> Part2()
    {
        return await Task.FromResult(GetCoordinates(10,811589153L));
    }

    private long GetCoordinates(int iterations, long multiplier) 
    {
        var nums = Inputs.Select(line => multiplier * long.Parse(line)).ToArray();
        var perm = Enumerable.Range(0, nums.Length).ToArray();      
        for (var iter = 0; iter < iterations; iter++) 
        {          
            for (var inum = 0; inum < nums.Length; inum++) 
            {
                var iperm = Array.IndexOf(perm, inum); 
                var steps = nums[inum] % (nums.Length - 1);
                var dir = Math.Sign(steps);

                for (var i = 0; i != steps; i += dir) 
                {
                    var ipermNext = (iperm + dir + nums.Length) % nums.Length;
                    (perm[ipermNext], perm[iperm]) = (perm[iperm], perm[ipermNext]);
                    iperm = ipermNext;
                }
            }
        }

        nums = perm.Select(inum => nums[inum]).ToArray();
        var idx0 = Array.IndexOf(nums, 0);
        return (
            nums[(idx0 + 1000) % nums.Length] +
            nums[(idx0 + 2000) % nums.Length] +
            nums[(idx0 + 3000) % nums.Length]
        );
    }
}