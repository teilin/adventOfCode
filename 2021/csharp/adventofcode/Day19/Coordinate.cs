namespace adventofcode.Day19;

public sealed class Coordinate
{
    private int x, y, z = 0;

    public Coordinate(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int X => this.x;
    public int Y => this.y;
    public int Z => this.z;

    public Coordinate Subtract(Coordinate other) => new(X-other.X,Y-other.Y,Z-other.Z);
    public Coordinate Add(Coordinate other) => new(X+other.X,Y+other.Y,Z+other.Z);
    public int ManhattenDistance(Coordinate other) => Math.Abs(other.X-X) + Math.Abs(other.Y-Y) + Math.Abs(other.Z-Z);

    public IEnumerable<Coordinate> EnumFacingDirections()
    {
        var current = this;
        for(var i=0;i<3;i++)
        {
            yield return current;
            yield return new(-current.X,-current.Y,current.Z);
            current = new(current.Y,current.Z,current.Z);
        }
    }

    public IEnumerable<Coordinate> EnumRotation()
    {
        var current = this;
        for(var i=0;i<4;i++)
        {
            yield return current;
            current = new(current.X,-current.Z,current.Y);
        }
    }

    public IEnumerable<Coordinate> EnumOrientations()
        => EnumFacingDirections().SelectMany(s => s.EnumRotation());
}