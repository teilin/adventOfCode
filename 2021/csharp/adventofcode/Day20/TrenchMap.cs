using System.Text;

namespace adventofcode.Day20;

public sealed class TrenchMap : ISolver
{
    // private string[] _input;
    // private char[] _imageAlgoritm;
    // private char[,] _image = new char[0,0];
    // private int maxX, maxY = 0;

    public async ValueTask ExecutePart1(string inputFilePath)
    {
        string[] input = await System.IO.File
            .ReadAllLinesAsync(inputFilePath);

        Solution s = new Solution();
        s.Part1(input);
        s.Part2(input);
        // var index = 0;
        // _input = await File.ReadAllLinesAsync(inputFilePath);

        // var tmpAlgorithm = string.Empty;
        // foreach(var str in _input)
        // {
        //     index += 1;
        //     if(string.IsNullOrEmpty(str)) break;
        //     tmpAlgorithm += str;
        // }
        // _imageAlgoritm = tmpAlgorithm.ToCharArray();
        // var map = new Dictionary<(int X,int Y),char>();
        // _image = new char[_input[index].ToCharArray().Length, _input.Length-index];
        // for(int i=0;i<_image.GetLength(0);i++)
        // {
        //     var str = _input[i+index].ToCharArray();
        //     for(var j=0;j<str.Length;j++)
        //     {
        //         _image[j,i] = str[j];
        //     }
        // }
        // var outputImage = Simulate(_image,2);
        // Console.WriteLine(CountCharsInArray(outputImage,'#'));
    }

    // private char[,] Simulate(char[,] input, int numSimulations)
    // {
    //     if(numSimulations == 0) return input;
    //     else
    //     {
    //         var outputImage = new char[input.GetLength(0)+2,input.GetLength(1)+2];
    //         for(var x=-1;x<=input.GetLength(0);x++)
    //         {
    //             for(var y=-1;y<=input.GetLength(1);y++)
    //             {
    //                 var binary = string.Empty;
    //                 // Top left
    //                 binary += ToBinary(input,x-1,y-1);
    //                 // Top middle
    //                 binary += ToBinary(input,x,y-1);
    //                 // Top right
    //                 binary += ToBinary(input,x+1,y-1);
    //                 // Midle left
    //                 binary += ToBinary(input,x-1,y);
    //                 // Middle middle
    //                 binary += ToBinary(input,x,y);
    //                 // Middle right
    //                 binary += ToBinary(input,x+1,y);
    //                 // Bottom left
    //                 binary += ToBinary(input,x-1,y+1);
    //                 // Bottom middle
    //                 binary += ToBinary(input,x,y+1);
    //                 // Bottom right
    //                 binary += ToBinary(input,x+1,y+1);
    //                 // Binary to decimal
    //                 var tmp = Convert.ToInt32(binary,2);
    //                 var fromAlgoritm = _imageAlgoritm[tmp];
    //                 outputImage[x+1,y+1] = fromAlgoritm;
    //             }
    //         }
    //         return Simulate(outputImage,numSimulations-1);
    //     }
    // }

    // private int CountCharsInArray(char[,] input, char c)
    // {
    //     var counter = 0;
    //     for(var x=0;x<input.GetLength(0);x++)
    //     {
    //         for(var y=0;y<input.GetLength(1);y++)
    //         {
    //             if(input[x,y]==c) counter += 1;
    //         }
    //     }
    //     return counter;
    // }

    // private static string ToBinary(char c)
    // {
    //     if(c == '.') return "0";
    //     if(c == '#') return "1";
    //     throw new InvalidOperationException("Invalid chararcter");
    // }

    // private static string ToBinary(char[,] input, int x, int y)
    // {
    //     try
    //     {
    //         return ToBinary(input[x,y]);
    //     }
    //     catch
    //     {
    //         return ToBinary('.');
    //     }
    // }

    public ValueTask ExecutePart2(string inputFilePath)
    {
        return ValueTask.CompletedTask;
    }

    public bool ShouldExecute(int day)
    {
        return day==20;
    }
}

internal class Solution
{
    private char[,] _image;
    private int Rows { get; set; }
    private int Cols { get; set; }
    private string _algorithm;
    public bool IsEvenIteration;

    private void ParseInput(string[] input)
    {
        _algorithm = input[0];

        Cols = input[2].Length;
        Rows = input.Length - 2;

        _image = new char[Rows, Cols];

        for (int i = 2; i < input.Length; i++)
        {
            for (int j = 0; j < Cols; j++)
                _image[i - 2, j] = input[i][j];
        }
    }
    public int CountLitPixels()
    {
        int count = 0;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
                if (_image[i, j] == '#')
                    count++;
        }

        return count;
    }

    public char GetOutputPixel(int r, int c)
    {
        char borderChar = '.';
        if (r == 0 || c == 0 || r == Rows - 1 || c == Cols - 1)
        {
            if (!IsEvenIteration)
                borderChar = '#';

            return borderChar;
        }

        StringBuilder binaryCode = new StringBuilder();

        for (int i = r - 1; i <= r + 1; i++)
        {
            for (int j = c - 1; j <= c + 1; j++)
            {
                char x = _image[i, j] == '#' ? '1' : '0';
                binaryCode.Append(x);
            }
        }
        
        int decimalCode = Convert.ToInt32(binaryCode.ToString(), 2);
        return _algorithm[decimalCode];
    }

    public void EnhanceImage()
    {
        char[,] enhancedImage = new char[Rows, Cols];
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
                enhancedImage[i, j] = GetOutputPixel(i, j);
        }

        _image = enhancedImage;
    }
    public void ExtendImage(int margin)
    {
        int newRows = Rows + margin * 2;
        int newCols = Cols + margin * 2;
        char[,] extendedImage = new char[Rows + margin * 2, Cols + margin * 2];

        char borderChar = IsEvenIteration ? '#' : '.';
        
        for (int k = 0; k < newRows; k++)
        {
            for (int m = 0; m < margin; m++)
            {
                extendedImage[k, m] = borderChar;
                extendedImage[k, newCols - m-1] = borderChar;
            }
        }
        
        for (int k = 0; k < newCols; k++)
        {
            for (int m = 0; m < margin; m++)
            {
                extendedImage[m, k] = borderChar;
                extendedImage[newRows - m-1, k] = borderChar;
            }
        }
        
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
                extendedImage[i + margin, j + margin] = _image[i, j];
        }

        _image = extendedImage;
        Rows = newRows;
        Cols = newCols;
    }

    private void MultipleEnhance(int iter)
    {
        IsEvenIteration = false;
        for (int i = 0; i < iter; i++)
        {
            ExtendImage(2);
            EnhanceImage();
            IsEvenIteration = !IsEvenIteration;
        }
    }
    public void Part1(string[] input)
    {
        ParseInput(input);
        MultipleEnhance(2);
        Console.WriteLine(CountLitPixels());
    }
    
    public void Part2(string[] input)
    {
        ParseInput(input);
        MultipleEnhance(50);
        Console.WriteLine(CountLitPixels());
    }
}