namespace adventofcode.Day24;

public sealed class ArithmeticLogicUnit : ISolver
{
    private int[] maxArray = new int[14];
    public async ValueTask ExecutePart1(string inputFilePath)
    {
        string[] lines = await File.ReadAllLinesAsync(inputFilePath);

        string[][] instructions = lines.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

        int n = 14;

        int[] a = new int[n];
        int[] b = new int[n];
        for (int i = 0; i < n; i++) {
            a[i] = int.Parse(instructions[5+18*i][2]);
            b[i] = int.Parse(instructions[15+18*i][2]);
        }

        int[,] relations = new int[7,3];

        Stack<(int,int)> stack = new Stack<(int,int)>();

        int k = 0;

        for (int i = 0; i < n; i++) {
            if (a[i] > 0) {
                stack.Push((b[i],i));
            }
            else {
                (int,int) pop = stack.Pop();
                
                relations[k,0] = i;
                relations[k,1] = pop.Item2;

                int diff = pop.Item1 + a[i];

                relations[k,2] = diff;

                k++;
            }
        }
        for (int i = 0; i < 7; i++) {
            if (relations[i,2] >= 0) {
                maxArray[relations[i,0]] = 9;
                maxArray[relations[i,1]] = 9 - relations[i,2];
            }
            else {
                maxArray[relations[i,1]] = 9;
                maxArray[relations[i,0]] = 9 + relations[i,2];
            }
        }

        long max = ArrayToLong(maxArray);

        Console.WriteLine(max);
    }

    public async ValueTask ExecutePart2(string inputFilePath)
    {
        string[] lines = await File.ReadAllLinesAsync(inputFilePath);

        string[][] instructions = lines.Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

        int n = 14;

        int[] a = new int[n];
        int[] b = new int[n];
        for (int i = 0; i < n; i++) {
            a[i] = int.Parse(instructions[5+18*i][2]);
            b[i] = int.Parse(instructions[15+18*i][2]);
        }

        int[,] relations = new int[7,3];

        Stack<(int,int)> stack = new Stack<(int,int)>();

        int k = 0;

        for (int i = 0; i < n; i++) {
            if (a[i] > 0) {
                stack.Push((b[i],i));
            }
            else {
                (int,int) pop = stack.Pop();
                
                relations[k,0] = i;
                relations[k,1] = pop.Item2;

                int diff = pop.Item1 + a[i];

                relations[k,2] = diff;

                k++;
            }
        }

        int[] minArray = new int[14];

        for (int i = 0; i < 7; i++) {
            if (relations[i,2] >= 0) {
                maxArray[relations[i,1]] = 1;
                maxArray[relations[i,0]] = 1 + relations[i,2];
            }
            else {
                maxArray[relations[i,0]] = 1;
                maxArray[relations[i,1]] = 1 - relations[i,2];
            }
        }

        long min = ArrayToLong(maxArray);

        Console.WriteLine(min);
    }

    public bool ShouldExecute(int day)
    {
        return day==24;
    }

    static long ArrayToLong(int[] array) 
    {
        long res = 0L;
        foreach(int elem in array) {
            res = (long)(res * 10 + (long)elem);
        }
        return res;
    }

}