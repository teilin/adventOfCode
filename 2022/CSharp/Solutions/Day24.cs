public sealed class Day24 : Solver
{
    private readonly IList<Blizzard> _blizzards = new List<Blizzard>();
    private char[][] _valley;

    public Day24(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        _valley = new char[Inputs.Count()][];
        var lineNum = 0;
        foreach(var line in Inputs)
        {
            _valley[lineNum] = line.ToCharArray();
            lineNum++;
        }
        for(var y=0;y<_valley.Length;y++)
        {
            for(var x=0;x<_valley[y].Length;x++)
            {
                if(_valley[y][x]=='>' || _valley[y][x]=='<' || _valley[y][x]=='^' || _valley[y][x]=='v')
                {
                    _blizzards.Add(new Blizzard(x,y,_valley[y][x]));
                }
            }
        }
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        (int X, int Y) startPos = (Array.IndexOf(_valley[0],'.'),0);
        return await Task.FromResult(0);
    }

    protected override async Task<object> Part2()
    {
        return await Task.FromResult(0);
    }

    private record Blizzard
    {
        private int _x, _y;
        private (int x,int y) _direction;

        public Blizzard(int x, int y, char dir)
        {
            _x = x;
            _y = y;

            switch(dir)
            {
                case '>':
                    _direction = (1,0);
                    break;
                case '<':
                    _direction = (-1,0);
                    break;
                case '^':
                    _direction = (0,1);
                    break;
                case 'v':
                    _direction = (0,-1);
                    break;
                default:
                    break;
            }
        }

        public (int X, int Y) Current => (_x,_y);
        public (int X, int Y) NextPositition(char[][] map)
        {
            (int X, int Y) tmp = (_x+_direction.x,_y+_direction.y);
            if(map[tmp.Y][tmp.X] == '#')
            {
                if(_direction.y == -1) _y = map.Length - 2;
                if(_direction.y == 1) _y = 1;
                if(_direction.x == -1) _x = map[0].Length - 2;
                if(_direction.x == 1) _x = 1;
            }
            {
                _x += _direction.x;
                _y += _direction.y;
            }
            return Current;
        }
    }
}