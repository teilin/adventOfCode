namespace adventofcode.Day13;

public sealed class TransparentOrigami : ISolver
{
    string[] _input = new string[0];

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        _input = await File.ReadAllLinesAsync(inputFilePath);

        var coordinates = ParseCoordinates(_input);
        var instructions = ParseInstructions(_input);

        var maxX = coordinates.Select(s => s.X).Max();
        var maxY = coordinates.Select(s => s.Y).Max();

        var map = new char[maxY+1,maxX+1];
        foreach(var c in coordinates)
        {
            map[c.Y,c.X] = '#';
        }

        var fi = instructions.First();
        if(fi.DIR == 'y') map = FoldY(fi.VALUE, map);
        if(fi.DIR == 'x') map = FoldX(fi.VALUE, map);

        // Count
        var counter = 0;
        for (int y = 0; y < map.GetLength(0); y++) 
        {
            for (int x = 0; x < map.GetLength(1); x++) 
            {
                if(map[y,x] == '#') counter += 1;
            }
        }
        Console.WriteLine(counter);
    }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        var coordinates = ParseCoordinates(_input);
        var instructions = ParseInstructions(_input);

        var maxX = coordinates.Select(s => s.X).Max();
        var maxY = coordinates.Select(s => s.Y).Max();

        var listPoints = new List<(int X,int Y)>();
        var map = new char[maxY+1,maxX+1];
        foreach(var c in coordinates)
        {
            map[c.Y,c.X] = '#';
        }

        foreach(var fi in instructions)
        {
            if(fi.DIR == 'y') map = FoldY(fi.VALUE, map);
            if(fi.DIR == 'x') map = FoldX(fi.VALUE, map);
            var points = GetPoints(map);
            listPoints.AddRange(points);
        }
        
        // string[] lines = System.IO.File.ReadAllLines(inputFilePath);
        // List<Point> points = new List<Point>();
        // List<Point> folds = new List<Point>();

        // foreach (string line in lines)
        // {
        //     if (line != "" && !line.StartsWith("fold along"))
        //     {
        //         var segments = line.Split(',');
        //         points.Add(new Point() { X = int.Parse(segments[0]), Y = int.Parse(segments[1]) });
        //     }
        //     else if (line.StartsWith("fold along x="))
        //     {
        //         folds.Add(new Point() { X = int.Parse(line.Replace("fold along x=", "")), Y = 0 });
        //     }
        //     else if (line.StartsWith("fold along y="))
        //     {
        //         folds.Add(new Point() { Y = int.Parse(line.Replace("fold along y=", "")), X = 0 });
        //     }
        // }

        // points = points.OrderBy(one => one.Y).ThenBy(one => one.X).ToList();

        // foreach (var fold in folds)
        // {
        //     for (int i = 0; i < points.Count; i++)
        //     {
        //         var p = points[i];
        //         if (p.X > fold.X && fold.X > 0) p.X = p.X - ((p.X - fold.X) * 2);
        //         if (p.Y > fold.Y && fold.Y > 0) p.Y = p.Y - ((p.Y - fold.Y) * 2);
        //         points[i] = p;
        //     }

        //     //part 1
        //     //break;
        // }

        // //part 1
        // //points = points.GroupBy(one => one.ToString()).Select(one => one.First()).ToList();
        // //Console.WriteLine("Number of points = " + points.Count());

        // //print it out for part 2

        for (var y = 0; y <= listPoints.Max(one => one.Y); y++)
        {
            for (var x = 0; x <= listPoints.Max(one => one.X); x++)
            {
                if(listPoints.Contains((x,y)))
                    Console.WriteLine("X");
                else
                    Console.WriteLine(" ");
                // var test = listPoints.Find(one => one.X == x && one.Y == y);
                // if (test != null)
                //     Console.Write("X");
                // else
                //     Console.Write(" ");
            }
            Console.Write("\n");
        }
        return ValueTask.CompletedTask;
    }

    public bool ShouldExecute(int day)
    {
        return day==13;
    }

    private char[,] FoldY(int flipY, char[,] map)
    {
        var y = 0;
        var maxY = map.GetLength(0)-1;
        var newMap = new char[flipY,map.GetLength(1)];
        while(y < flipY && maxY > flipY)
        {
            var x = 0;
            while(x < map.GetLength(1))
            {
                if(map[maxY,x]=='#')
                {
                    newMap[y,x] = '#';
                }
                else if(map[y,x] == '#')
                {
                    newMap[y,x] = '#';
                }
                else
                {
                    newMap[y,x] = '.';
                }
                x += 1;
            }

            y += 1;
            maxY -= 1;
        }
        return newMap;
    }

    private char[,] FoldX(int flipX, char[,] map)
    {
        var x = 0;
        var maxX = map.GetLength(1)-1;
        var newMap = new char[map.GetLength(0),flipX];
        while(x < flipX && maxX > flipX)
        {
            var y = 0;
            while(y < map.GetLength(0))
            {
                if(map[y,maxX]=='#' || map[y,x] == '#')
                {
                    newMap[y,x] = '#';
                }
                else
                {
                    newMap[y,x] = '.';
                }
                y += 1;
            }

            x += 1;
            maxX -= 1;
        }
        return newMap;
    }

    private IList<(int X,int Y)> GetPoints(char[,] map)
    {
        var points = new List<(int X,int Y)>();
        for(var y=0;y<map.GetLength(0);y++)
        {
            for(var x=0;x<map.GetLength(1);x++)
            {
                if(map[y,x]=='#') points.Add((x,y));
            }
        }
        return points;
    }

    private IList<(int X,int Y)> ParseCoordinates(string[] input)
    {
        var coordinates = new List<(int X,int Y)>();
        foreach(var str in input)
        {
            if(!str.StartsWith("fold") && !string.IsNullOrEmpty(str))
            {
                var tmp = str.Split(',',StringSplitOptions.RemoveEmptyEntries);
                coordinates.Add((int.Parse(tmp[0]),int.Parse(tmp[1])));
            }
        }
        return coordinates;
    }

    private IList<(char DIR,int VALUE)> ParseInstructions(string[] input)
    {
        var instructions = new List<(char,int)>();
        foreach(var str in input)
        {
            if(str.StartsWith("fold"))
            {
                var tmp = str.Split('=',StringSplitOptions.RemoveEmptyEntries);
                var tmp2 = tmp[0].Replace("fold along ", "");
                instructions.Add((tmp2.ToCharArray()[0],int.Parse(tmp[1])));
            }
        }
        return instructions;
    }

    // class Point
    // {
    //     public int X;
    //     public int Y;

    //     public override string ToString()
    //     {
    //         return $"({X}:{Y})";
    //     }
    // }
}