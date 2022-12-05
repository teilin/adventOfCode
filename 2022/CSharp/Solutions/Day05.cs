public sealed class Day05 : Solver
{
    private IList<Instruction> _instructions = new List<Instruction>();
    private IDictionary<int, Stack<char>> _stacks = new Dictionary<int, Stack<char>>();

    public Day05(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        var index = 0;
        var spacerLine = 0;
        foreach(var line in Inputs)
        {
            if(spacerLine != 0) break;
            if(string.IsNullOrEmpty(line))
            {
                spacerLine = index;
            }
            index++;
        }
        // Read instructions
        for(var i=spacerLine+1;i<Inputs.Count();i++)
        {
            var line = Inputs.ElementAt(i);
            line = line.Replace("move ", "").Replace("from ", "").Replace("to ", "");
            var inputs = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            _instructions.Add(new Instruction
            {
                Quantity = int.Parse(inputs[0]),
                From = int.Parse(inputs[1]),
                To = int.Parse(inputs[2])
            });
        }
        //2 6 10
        var stacs = new[] { 1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41, 45, 49 };
        // Read stacks
        for(var i=spacerLine-2;i>=0;i--)
        {
            var line = Inputs.ElementAt(i);
            for(var s=1;s<line.Length;s+=4)
            {
                if(line[s]!=' ')
                {
                    var element = line[s];
                    var stack = Array.IndexOf(stacs, s);
                    if(_stacks.ContainsKey(stack+1))
                    {
                        var tmp = _stacks[stack+1];
                        tmp.Push(element);
                        _stacks[stack+1] = tmp;
                    }
                    else
                    {
                        var tmp = new Stack<char>();
                        tmp.Push(element);
                        _stacks.Add(stack+1, tmp);
                    }
                }
            }
        }
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var tmpStack = new Dictionary<int, Stack<char>>(_stacks);
        foreach(var instruction in _instructions)
        {
            var q = new Queue<char>();
            var moveCount = 0;
            while(moveCount < instruction.Quantity)
            {
                var t = tmpStack[instruction.From].Pop();
                q.Enqueue(t);
                moveCount++;
            }
            while(q.Count() > 0)
            {
                var t = q.Dequeue();
                tmpStack[instruction.To].Push(t);
            }
        }
        IEnumerable<char> tmpStr = new List<char>();
        var tmp = 0;
        foreach(var s in tmpStack)
        {
            tmpStr = tmpStr.Append(s.Value.Peek());
            tmp++;
            if(tmp == tmpStack.Count()) break;
        }
        return await Task.FromResult(new string(tmpStr.ToArray()));
    }

    protected override async Task<object> Part2()
    {
        var tmpStack = new Dictionary<int, Stack<char>>(_stacks);
        foreach(var instruction in _instructions)
        {
            var q = new Stack<char>();
            var moveCount = 0;
            while(moveCount < instruction.Quantity)
            {
                var t = tmpStack[instruction.From].Pop();
                q.Push(t);
                moveCount++;
            }
            while(q.Count() > 0)
            {
                var t = q.Pop();
                tmpStack[instruction.To].Push(t);
            }
        }
        IEnumerable<char> tmpStr = new List<char>();
        var tmp = 0;
        foreach(var s in _stacks)
        {
            tmpStr = tmpStr.Append(s.Value.Peek());
            tmp++;
            if(tmp == tmpStack.Count()) break;
        }
        return await Task.FromResult(new string(tmpStr.ToArray()));
    }

    record Instruction
    {
        public int Quantity { get; init; }
        public int From { get; init; }
        public int To { get; init; }
    }
}