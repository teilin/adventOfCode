namespace adventofcode.Day15;

public sealed class Chiton : ISolver
{
    private Dictionary<(int x,int y),CaveNode> _nodes = new Dictionary<(int x, int y), CaveNode>();
    // private readonly IList<(int x,int y)> _directions = new List<(int x,int y)> { (0,1),(1,0),(-1,0),(0,-1) };
    // private string[] _input = new string[0];
    static readonly (int y, int x)[] adjacents =
    {
        (-1,  0),
        ( 0, -1),
        ( 0, +1),
        (+1,  0)
    };

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        List<string> lines = (await File.ReadAllLinesAsync(inputFilePath)).ToList();
        Dictionary<(int y, int x), CaveNode> nodes = new Dictionary<(int y, int x), CaveNode>();
        (int X, int Y) target = (lines[0].Length - 1, lines.Count - 1);
        for(int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                nodes.Add((y, x), new CaveNode(x, y, Int32.Parse(lines[y][x].ToString())));
            }
        }

        nodes[(0, 0)].DistanceFromStart = 0;
        Console.WriteLine(ShortestPath(nodes, target));
    }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        List<string> lines = File.ReadAllLines(inputFilePath).ToList();
        Dictionary<(int y, int x), CaveNode> nodes = new Dictionary<(int y, int x), CaveNode>();
        int lineY = lines.Count;
        int lineX = lines[0].Length;
        (int X, int Y) target = (lineX * 5 - 1, lineY * 5 - 1);

        for (int y = 0; y <= target.Y; y++)
        {
            for (int x = 0; x <= target.X; x++)
            {
                int yMultiplier = y / lineY;
                int xMultiplier = x / lineX;
                int riskLevel = Int32.Parse(lines[y % (lineY)][x % (lineX)].ToString());
                riskLevel = (riskLevel + yMultiplier + xMultiplier) % 9;
                riskLevel = (riskLevel > 0) ? riskLevel : 9;
                nodes.Add((y, x), new CaveNode(x, y, riskLevel));
            }
        }
        
        nodes[(0, 0)].DistanceFromStart = 0;
        Console.WriteLine(ShortestPath(nodes, target));
        return ValueTask.CompletedTask;
    }

    public bool ShouldExecute(int day)
    {
        return day==15;
    }

    private Int64 ShortestPath(Dictionary<(int x, int y), CaveNode> nodes, (int X,int Y) target)
    {
        PriorityQueue<CaveNode, int> queue = new PriorityQueue<CaveNode, int>();
        HashSet<CaveNode> visited = new HashSet<CaveNode>();
        queue.Enqueue(nodes[(0, 0)], nodes[(0,0)].DistanceFromStart );
        visited.Add(nodes[(0, 0)]);

        while (queue.Count > 0)
        {
            CaveNode currentNode = queue.Dequeue();

            if (currentNode.X == target.X && currentNode.Y == target.Y)
                return currentNode.DistanceFromStart;

            for (int i = 0; i < adjacents.Length; i++)
            {
                int newY = currentNode.Y + adjacents[i].y;
                int newX = currentNode.X + adjacents[i].x;

                if (newY < 0 || newX < 0 || newY > target.Y || newX > target.X) continue;

                CaveNode nextNode = nodes[(newY, newX)];
                int distanceToNext = currentNode.DistanceFromStart + nextNode.RiskLevel;

                if ( nextNode.DistanceFromStart > distanceToNext)
                {
                    if (visited.Contains(nextNode)) visited.Remove(nextNode);

                    nextNode.DistanceFromStart = distanceToNext;
                    queue.Enqueue(nextNode, distanceToNext);
                }
            }
        }

        return nodes[(target.Y, target.X)].DistanceFromStart;
    }

    // private IList<(int x,int y)> ValidDirections((int x,int y) position)
    // {
    //     return _directions.Where(w => position.x+w.x >= 0 && position.x+w.x < _input[0].Length
    //         && position.y+w.y >= 0 && position.y+w.y < _input.Length).ToList();
    // }

    internal class CaveNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int RiskLevel { get; set; }
        public int DistanceFromStart { get; set; }

        public CaveNode(int x, int y, int riskLevel)
        {
            X = x; 
            Y = y; 
            RiskLevel = riskLevel; 
            DistanceFromStart = int.MaxValue;
        }
    }
}