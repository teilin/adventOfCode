using System.Text.RegularExpressions;

public sealed class Day15 : Solver
{
    public Day15(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var pairs = Parse().ToArray();
        var rects = pairs.Select(s => s.ToRect()).ToArray();
        var left = rects.Select(s => s.Left).Min();
        var right = rects.Select(s => s.Right).Max();
        var y = 2000000;
        var res = 0;
        for(var x=left;x<=right;x++)
        {
            var pos = new Position(x,y);
            if(pairs.Any(p => p.beacon != pos && p.InRange(pos))) res++;
        }
        return await Task.FromResult(res);
    }

    protected override async Task<object> Part2()
    {
        var pairs = Parse().ToArray();
        var area = GetUncoveredAreas(pairs, new Rect(0,0,4000001,4000001)).First();
        return await Task.FromResult(area.X*4000000L+area.Y);
    }

    private IEnumerable<Pair> Parse()
    {
        foreach(var line in Inputs)
        {
            var numbers = Regex.Matches(line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray();
            yield return new Pair(
                sensor: new Position(numbers[0],numbers[1]),
                beacon: new Position(numbers[2],numbers[3])
            );
        }
    }

    private IEnumerable<Rect> GetUncoveredAreas(Pair[] pairs, Rect rect)
    {
        if(rect.Width == 0 || rect.Height == 0) yield break;
        foreach(var pair in pairs)
        {
            if(rect.Corners.All(corner => pair.InRange(corner))) yield break;
        }
        if(rect.Width == 1 && rect.Height == 1)
        {
            yield return rect;
            yield break;
        }
        foreach(var rectT in rect.Split())
        {
            foreach(var area in GetUncoveredAreas(pairs, rectT)) yield return area;
        }
    }

    private record Position(int X, int Y);

    private record struct Pair(Position sensor, Position beacon)
    {
        public int _radius = Mangatten(sensor,beacon);
        
        public bool InRange(Position pos)
        {
            return Mangatten(pos, sensor) <= _radius;
        }

        public Rect ToRect()
        {
            return new Rect(sensor.X-_radius,sensor.Y-_radius,2*_radius+1,2*_radius+1);
        }

        private static int Mangatten(Position pos1, Position pos2)
        {
            return Math.Abs(pos1.X-pos2.X)+Math.Abs(pos1.Y-pos2.Y);
        }
    }

    record struct Rect(int X, int Y, int Width, int Height) 
    {
        public int Left => X;
        public int Right => X + Width - 1;
        public int Top => Y;
        public int Bottom => Y + Height - 1;

        public IEnumerable<Position> Corners {
            get {
                yield return new Position(Left, Top);
                yield return new Position(Right, Top);
                yield return new Position(Right, Bottom);
                yield return new Position(Left, Bottom);
            }
        }

        public IEnumerable<Rect> Split() 
        {
            var w0 = Width / 2;
            var w1 = Width - w0;
            var h0 = Height / 2;
            var h1 = Height - h0;
            yield return new Rect(Left, Top, w0, h0);
            yield return new Rect(Left + w0, Top, w1, h0);
            yield return new Rect(Left, Top + h0, w0, h1);
            yield return new Rect(Left + w0, Top + h0, w1, h1);
        }
    }
}