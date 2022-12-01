public sealed class Day01 : Solver
{
    private int elfCount = 1;
    private IDictionary<int,long> calories = new Dictionary<int,long>();
    private int caloriesCurrentElv = 0;

    public Day01(string inputPath) : base(inputPath)
    {
        foreach(var line in _inputs)
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
    }

    protected override async Task<long> Part1()
    {
        return await Task.FromResult(calories.Values.Max());
    }

    protected override async Task<long> Part2()
    {
        return await Task.FromResult(calories.OrderByDescending(o => o.Value).Take(3).Sum(s => s.Value));
    }
}