public sealed class Day18 : Solver
{
    public Day18(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        return await Task.FromResult(new Lava(Inputs).Sides);
    }

    protected override async Task<object> Part2()
    {
        return await Task.FromResult(new Lava(Inputs).Part2Sides());
    }

    internal class Lava
    {
        private record Cube(int x, int y, int z);
        private HashSet<Cube> Cubes;

        public Lava(IEnumerable<string> input)
        {
            Cubes = new HashSet<Cube>(input.Select(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries)).Select(s => new Cube(int.Parse(s[0]),int.Parse(s[1]),int.Parse(s[2]))).ToList());
        }

        public int Sides => Cubes.Select(x => 6 - CountConnected(x)).Sum();

        public int Part2Sides()
        {
            var maxx = Cubes.OrderByDescending(c => c.x).First().x;
            var maxy = Cubes.OrderByDescending(c => c.y).First().y;
            var maxz = Cubes.OrderByDescending(c => c.z).First().z;
            var offcube = new Cube(maxx+1,maxy+1,maxz+1);

            for(int x=0; x<=maxx;x++)
            {
                for(int y=0;y<=maxy;y++)
                {
                    for(int z=0;z<=maxz;z++)
                    {
                        var c = new Cube(x,y,z);
                        if(!Cubes.Contains(c) && !WayFrom(c, offcube)) Expand(c);
                    }
                }
            }
            return Sides;
        }

        private int CountConnected(Cube cube)
        {
            var count = 0;
            foreach(var step in new[] {-1,1})
            {
                if(Cubes.Any(c => c.x+step == cube.x && c.y==cube.y && c.z==cube.z)) count++;
                if(Cubes.Any(c => c.x==cube.x && c.y+step==cube.y && c.z==cube.z)) count++;
                if(Cubes.Any(c => c.x==cube.x && c.y==cube.y && c.z+step==cube.z)) count++;
            }
            return count;
        }

        private void Expand(Cube start)
        {
            var fringe = new Queue<Cube>();
            var visited = new HashSet<Cube>();
            fringe.Enqueue(start);
            visited.Add(start);
            while(fringe.TryDequeue(out var current))
            {
                Cubes.Add(current);
                foreach(var next in GetAdj(current))
                {
                    if(!visited.Contains(next))
                    {
                        visited.Add(next);
                        if(!Cubes.Contains(next))
                        {
                            Cubes.Add(next);
                            fringe.Enqueue(next);
                        }
                    }
                }
            }
        }

        private bool WayFrom(Cube start, Cube end)
        {
            var fringe = new Queue<Cube>();
            var visited = new HashSet<Cube>();

            fringe.Enqueue(start);
            visited.Add(start);

            var maxx = Cubes.OrderByDescending(c => c.x).First().x;
            var maxy = Cubes.OrderByDescending(c => c.y).First().y;
            var maxz = Cubes.OrderByDescending(c => c.z).First().z;

            while(fringe.TryDequeue(out var current))
            {
                if(current.x < 0 || current.x < 0 || current.z < 0) continue;
                if(current.x > maxx && current.y > maxy && current.z > maxz) continue;

                foreach(var next in GetAdj(current))
                {
                    if(next == end) return true;
                    if(!visited.Contains(next))
                    {
                        visited.Add(next);
                        if(!Cubes.Contains(next)) fringe.Enqueue(next);
                    }
                }
            }
            return false;
        }

        private IEnumerable<Cube> GetAdj(Cube cube)
        {
            foreach (var step in new[] { -1, 1 })
            {
                yield return cube with { x = cube.x + step };
                yield return cube with { y = cube.y + step };
                yield return cube with { z = cube.z + step };
            }
        }
    }
}