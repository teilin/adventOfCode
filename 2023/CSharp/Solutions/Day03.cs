using System.Text;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day03 : ISolver
{
    private (int dx, int dy)[] deltas = { (-1,1), (0,1), (1,1), (-1,0), (1,0), (-1,-1), (0,-1), (1,-1) };
    private char[,] _matrix;
    private readonly char dot = '.';
    private readonly char asterisk = '*';
    private readonly int noAdjacentNumbers = 2;

    public async Task Solve(Input input)
    {
        await ReadMatrix(input.inputs);
        var adjacentNumbers = GetNumberAdjacentToSymbol(_matrix);
        var foundNumbers = GetNumbers(_matrix, adjacentNumbers);
        Console.WriteLine($"Part 1 -> {foundNumbers.Sum()}");

        var adjacentNumbersCoordinates = GetNumbersAdjacentToAsteriskPairs(_matrix);
        var filteredCoordinates = adjacentNumbersCoordinates
            .Where(w => w.Value.Count == noAdjacentNumbers)
            .ToDictionary(w => w.Key, x => x.Value);
        Console.WriteLine($"Part 2 -> {GetResult(filteredCoordinates, _matrix)}");
    }

    private long GetResult(Dictionary<Point, List<Point>> data, char[,] grid)
    {
        long sum = 0;
        foreach (var item in data)
        {
            var numbers = GetNumbers(grid, item.Value);
            sum += numbers.Aggregate(1, (current, value) => current * value);
        }
        return sum;
    }

    private IList<Point> GetNumberAdjacentToSymbol(char[,] matrix)
    {
        var numRows = matrix.GetLength(0);
        var numColumns = matrix.GetLength(1);
        var adjacentNumbers = new List<Point>();
        for(var i=0;i<numRows;i++)
        {
            for(var j=0;j<numColumns;j++)
            {
                if(char.IsDigit(matrix[i,j]) || matrix[i,j] == dot) continue;
                for(var k=i-1;k<i+2;k++)
                {
                    for(var l=j-1;l<j+2;l++)
                    {
                        if(k<0 || l<0 || k>=numRows || l>=numColumns) continue;
                        if(char.IsDigit(matrix[k,l]))
                        {
                            var column = l;
                            while(column>0 && char.IsDigit(matrix[k,column-1])) column--;
                            if(!adjacentNumbers.Exists(x => x.X==k && x.Y==column))
                            {
                                adjacentNumbers.Add(new Point(k, column));
                            }
                        }
                    }
                }
            }
        }
        return adjacentNumbers;
    }

    private IList<int> GetNumbers(char[,] matrix, IList<Point> coordinates)
    {
        var numColumns = matrix.GetLength(1);
        var foundNumbers = new List<int>();
        foreach(var point in coordinates)
        {
            var number = new StringBuilder();
            while(point.Y < numColumns && char.IsDigit(matrix[point.X,point.Y]))
            {
                number.Append(matrix[point.X,point.Y]);
                point.Y++;
            }
            foundNumbers.Add(Convert.ToInt32(number.ToString()));
        }
        return foundNumbers;
    }

    private Dictionary<Point, List<Point>> GetNumbersAdjacentToAsteriskPairs(char[,] grid)
    {
        int noRows = grid.GetLength(0);
        int noColumns = grid.GetLength(1);

        Dictionary<Point, List<Point>> asteriskAdjacentNumbersCoordPairs = new Dictionary<Point, List<Point>>();

        for (int i = 0; i < noRows; i++)
        {
            for (int j = 0; j < noColumns; j++)
            {
                if (grid[i, j] != asterisk)
                {
                    continue;
                }

                List<Point> adjacentNumbersCoordinates = new List<Point>();

                for (int k = i - 1; k < i + 2; k++)
                {
                    for (int l = j - 1; l < j + 2; l++)
                    {
                        if (k < 0 || l < 0 || k >= noRows || l >= noColumns)
                        {
                            continue;
                        }

                        if (char.IsDigit(grid[k, l]))
                        {
                            int column = l;
                            while (column > 0 && char.IsDigit(grid[k, column - 1]))
                            {
                                column--;
                            }

                            if (!adjacentNumbersCoordinates.Any(x => x.X == k && x.Y == column))
                            {
                                adjacentNumbersCoordinates.Add(new Point { X = k, Y = column });
                            }
                        }
                    }
                }

                asteriskAdjacentNumbersCoordPairs.Add(new Point { X = i, Y = j }, adjacentNumbersCoordinates);
            }
        }

        return asteriskAdjacentNumbersCoordPairs;
    }

    private Task ReadMatrix(IList<string> inputs)
    {
        var numRows = inputs.Count;
        var numColumns = inputs.ElementAt(0).Length;
        _matrix = new char[numRows,numColumns];
        for(var i=0; i<numRows; i++)
        {
            var row = inputs[i];
            for(var j=0;j<numColumns;j++)
            {
                _matrix[i,j] = row[j];
            }
        }
        return Task.CompletedTask;
    }

    private class Point
    {
        public Point() {}
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}