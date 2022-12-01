namespace adventofcode.Day25;

public sealed class SeaCucumber : ISolver
{
    private string[] _input;

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        List<string> lines = (await File.ReadAllLinesAsync(inputFilePath)).ToList();
        List<(int y, int x)> eastSeaC = new List<(int y, int x)>();
        List<(int y, int x)> southSeaC = new List<(int y, int x)>();
        int rightEdge = lines[0].Length;
        int bottomEdge = lines.Count;

        for (int y = 0; y < lines.Count; y++)
        {
            for(int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '>')
                    eastSeaC.Add((y, x));
                else if (lines[y][x] == 'v')
                    southSeaC.Add((y, x));
            }
        }

        int steps = 0;
        bool didMove;
        List<(int y, int x)> newEastSeaC = new List<(int y, int x)>();
        List<(int y, int x)> newSouthSeaC = new List<(int y, int x)>();

        do
        {
            didMove = false;
            foreach ((int y, int x) coord in eastSeaC)
            {
                int newX = (coord.x + 1) % rightEdge;
                if (eastSeaC.Contains((coord.y, newX)) || southSeaC.Contains((coord.y, newX)))
                    newEastSeaC.Add(coord);
                else
                {
                    newEastSeaC.Add((coord.y, newX));
                    didMove = true;
                }
                    
            }

            eastSeaC = new List<(int y, int x)>(newEastSeaC);
            newEastSeaC.Clear();

            foreach ((int y, int x) coord in southSeaC)
            {
                int newY = (coord.y + 1) % bottomEdge;
                if (eastSeaC.Contains((newY, coord.x)) || southSeaC.Contains((newY, coord.x)))
                    newSouthSeaC.Add(coord);
                else
                {
                    newSouthSeaC.Add((newY, coord.x));
                    didMove = true;
                }
            }

            southSeaC = new List<(int y, int x)>(newSouthSeaC); ;
            newSouthSeaC.Clear();
            steps++;
        } while (didMove);

        Console.WriteLine(steps);
        // _input = await File.ReadAllLinesAsync(inputFilePath);

        // var map = CreateMap(_input);
        // var hasMoved = true;
        // var numSteps = 0;

        // while(hasMoved)
        // {
        //     map = Move(map, ref hasMoved, ref numSteps);
        // }

        // Console.WriteLine(numSteps);
    }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        return ValueTask.CompletedTask;
    }

    public bool ShouldExecute(int day)
    {
        return day==25;
    }

    private char[,] Move(char[,] map, ref bool hasMoved, ref int numSteps)
    {
        hasMoved = false;
        for(var x=0;x<map.GetLength(0);x++)
        {
            for(var y=0;y<map.GetLength(1);y++)
            {
                var c = map[x,y];
                if(map[x,y]=='>')
                {
                    var next = 0;
                    if(x+1 == map.GetLength(0)) next = 0;
                    else next = x+1;

                    if(map[next,y] == '.')
                    {
                        hasMoved = true;
                        map[next,y] = map[x,y];
                        map[x,y] = '.';
                    }
                }
            }
        }

        for(var x=0;x<map.GetLength(0);x++)
        {
            for(var y=0;y<map.GetLength(1);y++)
            {
                var c = map[x,y];
                if(map[x,y]=='v')
                {
                    var next = 0;
                    if(y+1 == map.GetLength(1)) next = 0;
                    else next = y+1;

                    if(map[x,next] == '.')
                    {
                        hasMoved = true;
                        map[x,next] = map[x,y];
                        map[x,y] = '.';
                    }
                }
            }
        }
        if(hasMoved) numSteps += 1;
        return map;
    }

    private char[,] CreateMap(string[] input)
    {
        var tmpMap = new char[input[0].Length,input.Length];

        for(var x=0;x<input[0].Length;x++)
        {
            for(var y=0;y<input.Length;y++)
            {
                tmpMap[x,y] = input[y][x];
            }
        }

        return tmpMap;
    }
}