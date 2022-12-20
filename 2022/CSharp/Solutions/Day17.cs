using System.Numerics;

public sealed class Day17 : Solver
{
    private List<Shape> Shapes = new()
    {
        new() { Blocks = new() { new(0,0), new(1,0),  new(2,0),   new (3,0) }             },
        new() { Blocks = new() { new(0,0), new(1,0),  new(2,0),   new (1,1), new(1, -1) } },
        new() { Blocks = new() { new(0,0), new(1,0),  new(2,0),   new(2,1),  new(2,2) }   },
        new() { Blocks = new() { new(0,0), new(0,-1), new(0,-2),  new(0,-3) }             },
        new() { Blocks = new() { new(0,0), new(1,0),  new (0,-1), new (1, -1) }           }
    };

    public Day17(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        throw new NotImplementedException();
    }

    protected override Task<object> Part1()
    {
        throw new NotImplementedException();
    }

    protected override Task<object> Part2()
    {
        throw new NotImplementedException();
    }

    private enum Direction
    {
        Left, Right, Down
    }

    private class Shape
    {
        public List<Vector2> Blocks { get; set; }

        public bool Overlaps(Shape s) => Blocks.Any(b => s.Blocks.Contains(b));
        
        public void Spawn(Vector2 pos) => Blocks = Blocks.Select(b => b + pos).ToList();

        public void Move(Direction direction)
        {
            Blocks = direction switch
            {
                Direction.Left => Blocks.Select(b => b - Vector2.UnitX).ToList(),
                Direction.Right => Blocks.Select(b => b + Vector2.UnitX).ToList(),
                Direction.Down => Blocks.Select(b => b - Vector2.UnitY).ToList(),
                _ => Blocks
            };
        }
    }
}