using System.Text.RegularExpressions;

public sealed class Day11 : Solver
{
    public Day11(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        return Task.CompletedTask;
    }

    protected override async Task<object> Part1()
    {
        var monkeys = ParseMonkeys(RawInput);
        Run(20, monkeys, w => w/3);
        return await Task.FromResult(GetMonkeyBusinessLevel(monkeys));
    }

    protected override async Task<object> Part2()
    {
        var monkeys = ParseMonkeys(RawInput);
        var mod = monkeys.Aggregate(1, (mod,monkey) => mod*monkey.mod);
        Run(10000,monkeys,w=>w%mod);
        return await Task.FromResult(GetMonkeyBusinessLevel(monkeys));
    }

    private Monkey[] ParseMonkeys(string input) =>
        input.Split("\n\n").Select(Parse).ToArray();

    private Monkey Parse(string input)
    {
        var monkey = new Monkey();
        foreach(var line in input.Split("\n"))
        {
            var tryParse = LineParser(line);
            if (tryParse(@"Monkey (\d+)", out var arg)) {
            } else if (tryParse("Starting items: (.*)", out arg)) {
                monkey.items = new Queue<long>(arg.Split(", ").Select(long.Parse));
            } else if (tryParse(@"Operation: new = old \* old", out _)) {
                monkey.operation = old => old * old;
            } else if (tryParse(@"Operation: new = old \* (\d+)", out arg)) {
                monkey.operation = old => old * int.Parse(arg);
            } else if (tryParse(@"Operation: new = old \+ (\d+)", out arg)) {
                monkey.operation = old => old + int.Parse(arg);
            } else if (tryParse(@"Test: divisible by (\d+)", out arg)) {
                monkey.mod = int.Parse(arg);
            } else if (tryParse(@"If true: throw to monkey (\d+)", out arg)) {
                monkey.passToMonkeyIfDicides = int.Parse(arg);
            } else if (tryParse(@"If false: throw to monkey (\d+)", out arg)) {
                monkey.passToMonkeyOtherwise = int.Parse(arg);
            } else {
                throw new ArgumentException(line);
            }
        }
        return monkey;
    }

    long GetMonkeyBusinessLevel(IEnumerable<Monkey> monkeys) => 
        monkeys
            .OrderByDescending(monkey => monkey.inspectedItems)
            .Take(2)
            .Aggregate(1L, (res, monkey) => res * monkey.inspectedItems);

    void Run(int rounds, Monkey[] monkeys, Func<long, long> updateWorryLevel) {
        for (var i = 0; i < rounds; i++) {
            foreach (var monkey in monkeys) {
                while (monkey.items.Any()) {
                    monkey.inspectedItems++;

                    var item = monkey.items.Dequeue();
                    item = monkey.operation(item);
                    item = updateWorryLevel(item);

                    var targetMonkey = item % monkey.mod == 0 ?
                        monkey.passToMonkeyIfDicides :
                        monkey.passToMonkeyOtherwise;

                    monkeys[targetMonkey].items.Enqueue(item);
                }
            }
        }
    }

    internal class Monkey
    {
        public Queue<long> items;
        public Func<long,long> operation;
        public int inspectedItems;
        public int mod;
        public int passToMonkeyIfDicides,passToMonkeyOtherwise;
    }

    private TryParse LineParser(string line)
    {
        bool match(string pattern, out string arg) 
        {
            var m = Regex.Match(line,pattern);
            if(m.Success)
            {
                arg = m.Groups[m.Groups.Count-1].Value;
                return true;
            }
            else
            {
                arg = string.Empty;
                return false;
            }
        }
        return match;
    }

    delegate bool TryParse(string pattern, out string arg);
}