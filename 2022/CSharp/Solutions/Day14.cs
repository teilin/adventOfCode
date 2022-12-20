using System.Numerics;

public sealed class Day14 : Solver
{
    public Day14(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var t = new Cave(Inputs, false).FillSand(new Complex(500,0));
        return await Task.FromResult(t);
    }

    protected override async Task<object> Part2()
    {
        var sandRest = new Cave(Inputs, true).FillSand(new Complex(500,0));
        return await Task.FromResult(sandRest);
    }

    record Cave
    {
        private bool _hasFloor;
        IDictionary<Complex, char> _map;
        int _maxImaginary;
        public Cave(IEnumerable<string> input, bool hasFloor)
        {
            _hasFloor = hasFloor;
            _map = new Dictionary<Complex,char>();
            foreach(var line in input)
            {
                var steps = (
                    from step in line.Split(" -> ")
                    let parts = step.Split(",")
                    select new Complex(int.Parse(parts[0]), int.Parse(parts[1]))
                ).ToArray();
                for(var i=1;i<steps.Length;i++) FillRocks(steps[i-1], steps[i]);
            }
            _maxImaginary = (int)_map.Keys.Select(p => p.Imaginary).Max();
        }

        public int FillRocks(Complex from, Complex to)
        {
            var direction = new Complex(Math.Sign(to.Real-from.Real), Math.Sign(to.Imaginary-from.Imaginary));
            var steps = 0;
            for(var pos=from;pos!=to+direction;pos+=direction)
            {
                _map[pos] = '#';
                steps++;
            }
            return steps;
        }

        public int FillSand(Complex source)
        {
            while(true)
            {
                var location = SimulateSandFalling(source);
                if(_map.ContainsKey(location)) break;
                if(!_hasFloor && location.Imaginary == _maxImaginary+1) break;
                _map[location] = 'o';
            }
            return _map.Values.Count(x => x=='o');
        }

        private Complex SimulateSandFalling(Complex sand)
        {
            var down = new Complex(0,1);
            var left = new Complex(-1,1);
            var right = new Complex(1,1);
            while(sand.Imaginary < _maxImaginary+1)
            {
                if(!_map.ContainsKey(sand+down)) sand += down;
                else if(!_map.ContainsKey(sand+left)) sand += left;
                else if(!_map.ContainsKey(sand+right)) sand += right;
                else break;
            }
            return sand;
        }
    }
}