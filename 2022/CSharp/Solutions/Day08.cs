public sealed class Day08 : Solver
{
    Dictionary<(int, int), int> trees = new();
    int height = 0;
    int width = 0;

    public Day08(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        int y = 0;
        foreach(var line in Inputs)
        {
            int x = 0;
            for (int i = 0; i < line.Length; i++)
            {
                trees.Add((x, y), int.Parse(line.Substring(i, 1)));
                x++;
                width = Math.Max(width, x);
            }
            y++;
            height = Math.Max(height, y);
        }
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        int visible = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1) visible++;
                else if (y == 0 || y == height - 1) visible++;
                else
                {
                    bool isVisible = true;
                    for (int x2 = x - 1; x2 >= 0; x2--)
                    {
                        if (trees[(x2, y)] >= trees[(x, y)])
                        {
                            isVisible = false; break;
                        }
                    }
                    if (isVisible)
                    {
                        visible++;
                        continue;
                    }

                    isVisible = true;
                    for (int x2 = x + 1; x2 < width; x2++)
                    {
                        if (trees[(x2, y)] >= trees[(x, y)])
                        {
                            isVisible = false; break;
                        }
                    }
                    if (isVisible)
                    {
                        visible++;
                        continue;
                    }

                    isVisible = true;
                    for (int y2 = y - 1; y2 >= 0; y2--)
                    {
                        if (trees[(x, y2)] >= trees[(x, y)])
                        {
                            isVisible = false; break;
                        }
                    }
                    if (isVisible)
                    {
                        visible++;
                        continue;
                    }

                    isVisible = true;
                    for (int y2 = y + 1; y2 < height; y2++)
                    {
                        if (trees[(x, y2)] >= trees[(x, y)])
                        {
                            isVisible = false; break;
                        }
                    }
                    if (isVisible) visible++;
                }
            }
        }
        return await Task.FromResult(visible);
    }

    protected override async Task<object> Part2()
    {
        int bestScore = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int totalScore = 1;
                int score = 0;
                for (int x2 = x - 1; x2 >= 0; x2--)
                {
                    score++;
                    if (trees[(x2, y)] >= trees[(x, y)]) break;
                }
                totalScore *= score;
                score = 0;
                for (int x2 = x + 1; x2 < width; x2++)
                {
                    score++;
                    if (trees[(x2, y)] >= trees[(x, y)]) break;
                }
                totalScore *= score;
                score = 0;
                for (int y2 = y - 1; y2 >= 0; y2--)
                {
                    score++;
                    if (trees[(x, y2)] >= trees[(x, y)]) break;
                }
                totalScore *= score;
                score = 0;
                for (int y2 = y + 1; y2 < height; y2++)
                {
                    score++;
                    if (trees[(x, y2)] >= trees[(x, y)]) break;
                }
                totalScore *= score;
                bestScore = Math.Max(bestScore, totalScore);
            }
        }
        return await Task.FromResult(bestScore);
    }
}