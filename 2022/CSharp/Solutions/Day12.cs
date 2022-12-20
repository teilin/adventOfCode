public sealed class Day12 : Solver
{
    private Dictionary<(int x, int y), Node> allNodes = new();
    private Node? part1StartNode;
    private Node? highPoint;

    public Day12(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        foreach (var (line, y) in Inputs.Select((value, y) => (value, y))) 
        {
            foreach (var (character, x) in line.Select((value, x) => (value, x))) 
            {
                if (character == 'S') {
                    part1StartNode = new Node((x, y), 0);
                    allNodes[(x, y)] = (part1StartNode);
                } else if (character == 'E') {
                    highPoint = new Node((x, y), 25);
                    allNodes[(x, y)] = (highPoint);
                } else {
                    allNodes[(x, y)] = (new Node((x, y), character - 'a'));
                }
            }
        }
        return Task.CompletedTask;
    }

    protected async override Task<object> Part1()
    {
        var shortestPath = ShortestPath((h1,h2) => h1+1 >= h2, part1StartNode, highPoint).ToString();
        return await Task.FromResult(shortestPath);
    }

    protected async override Task<object> Part2()
    {
        var shortestPath = int.MaxValue;
        var lowPoints = allNodes.Values.Where(w => w.height == 0);
        ShortestPath((h1, h2) => h1 <= h2 + 1, highPoint);
        foreach(var node in lowPoints)
        {
            if(shortestPath>node.distFromStart)
            {
                shortestPath = node.distFromStart;
            }
        }
        return await Task.FromResult(shortestPath);
    }

    // Djikstra with optional destination node, validNeighbour funcion takes heights and tells us if 2 nodes can be neighbours
    private int ShortestPath(Func<int, int, bool> validNeighbour, Node startNode, Node ?destNode = null) 
    {
        ResetNodes(validNeighbour);
        startNode.distFromStart = 0;

        PriorityQueue<Node, int> nodeQueue = new PriorityQueue<Node, int>();
        nodeQueue.Enqueue(startNode, 0);

        while (nodeQueue.Count > 0) 
        {
            Node curr = nodeQueue.Dequeue();

            if (curr.visited) 
            { // skip - already been here with a better dist (because p queue has no adjust priority method :( )
                continue;
            }

            if (destNode != null && curr == destNode) 
            {
                return curr.distFromStart;
            }
            
            curr.visited = true;
            int dist = curr.distFromStart + 1;
            foreach (Node neighbour in curr.neighbours.Where(x => !x.visited)) 
            {
                if (dist <= neighbour.distFromStart) 
                {
                    neighbour.distFromStart = dist;
                    nodeQueue.Enqueue(neighbour, dist);
                }
            }
        }

        return 0;
    }

    private void ResetNodes(Func<int, int, bool> validNeighbour)
    {
        foreach (Node n in allNodes.Values) 
        {
            n.distFromStart = int.MaxValue;
            n.visited = false;
            n.FindNeighbours(allNodes, validNeighbour);
        }
    }

    class Node
    {
        public List<Node> neighbours = new();
        public (int x, int y) position;
        public int height;

        public int distFromStart = int.MaxValue;
        public bool visited = false;

        public Node((int x, int y) position, int height) 
        {
            this.position = position;
            this.height = height;
        }

        public void FindNeighbours(Dictionary<(int x, int y), Node> allNodes, Func<int, int, bool> validNeighbour) 
        {
            if (allNodes.ContainsKey((position.x - 1, position.y))) {
                neighbours.Add(allNodes[(position.x - 1, position.y)]);
            }
            if (allNodes.ContainsKey((position.x + 1, position.y))) {
                neighbours.Add(allNodes[(position.x + 1, position.y)]);
            }
            if (allNodes.ContainsKey((position.x, position.y - 1))) {
                neighbours.Add(allNodes[(position.x, position.y - 1)]);
            }
            if (allNodes.ContainsKey((position.x, position.y + 1))) {
                neighbours.Add(allNodes[(position.x, position.y + 1)]);
            }

            neighbours = neighbours.Where(x => validNeighbour(height, x.height)).ToList();
        }
    }
}