namespace adventofcode.Day22;
public sealed class ReactorReboot : ISolver
{
    private IList<RebootStep> _rebootSteps = new List<RebootStep>();
    private IDictionary<(int x,int y,int z),bool> _states = new Dictionary<(int x,int y,int z),bool>();
    public async ValueTask ExecutePart1(string inputFilePath)
    {
        // var input = await File.ReadAllLinesAsync(inputFilePath);
        // foreach(var step in input)
        // {
        //     var split = step.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        //     var state = split[0] == "on" ? true : false;
        //     var positions = split[1].Split(',',StringSplitOptions.RemoveEmptyEntries);
        //     var (xstart,xend) = (int.Parse(positions[0].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[0]),int.Parse(positions[0].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[1]));
        //     var (ystart,yend) = (int.Parse(positions[1].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[0]),int.Parse(positions[1].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[1]));
        //     var (zstart,zend) = (int.Parse(positions[2].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[0]),int.Parse(positions[2].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[1]));
        //     _rebootSteps.Add(new RebootStep(state,(xstart,xend),(ystart,yend),(zstart,zend)));
        // }
        // foreach(var rebootStep in _rebootSteps)
        // {
        //     for(var x = rebootStep.X.Start;x <= rebootStep.X.End;x++)
        //     {
        //         if(x >= -50 && x <= 50)
        //         {
        //             for(var y = rebootStep.Y.Start;y <= rebootStep.Y.End;y++)
        //             {
        //                 if(y >= -50 && y <= 50)
        //                 {
        //                     for(var z = rebootStep.Z.Start;z <= rebootStep.Z.End;z++)
        //                     {
        //                         if(z >= -50 && z <= 50)
        //                         {
        //                             if(_states.ContainsKey((x,y,z))) _states[(x,y,z)] = rebootStep.State;
        //                             else _states.Add((x,y,z), rebootStep.State);
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }
        // var cubesWithStateOn = _states.Where(w => w.Value == true).Count();
        // Console.WriteLine(cubesWithStateOn);
        var commands = await Parse(inputFilePath);
        var cubesOn = ActiveCubesInRange(commands,50);
        Console.WriteLine(cubesOn);
    }

    public async ValueTask ExecutePart2(string inputFilePath)
    {
        var commands = await Parse(inputFilePath);
        var cubesOn = ActiveCubesInRange(commands,int.MaxValue);
        Console.WriteLine(cubesOn);
    }

    public bool ShouldExecute(int day)
    {
        return day==22;
    }

    private async Task<Command[]> Parse(string inputFilePath)
    {
        var regions = new List<Command>();
        var input = await File.ReadAllLinesAsync(inputFilePath);
        foreach(var step in input)
        {
            var split = step.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var state = split[0] == "off" ? true : false;
            var positions = split[1].Split(',',StringSplitOptions.RemoveEmptyEntries);
            var (xstart,xend) = (int.Parse(positions[0].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[0]),int.Parse(positions[0].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[1]));
            var (ystart,yend) = (int.Parse(positions[1].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[0]),int.Parse(positions[1].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[1]));
            var (zstart,zend) = (int.Parse(positions[2].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[0]),int.Parse(positions[2].Remove(0,2).Split("..",StringSplitOptions.RemoveEmptyEntries)[1]));
            regions.Add(new Command(state, new Region(new Step(xstart,xend),new Step(ystart,yend),new Step(zstart,zend))));
        }
        return regions.ToArray();
    }

    private long ActiveCubesInRange(Command[] cmds, int range)
    {
        long activeCubesAfterICommand(int iCommand, Region region)
        {
            if(region.IsEmpty || iCommand < 0) return 0;
            else
            {
                var intersection = region.Intersect(cmds[iCommand].Region);
                var activeInRegion = activeCubesAfterICommand(iCommand-1,region);
                var activeInIntersection = activeCubesAfterICommand(iCommand-1,intersection);
                var activeOutsideIntersection = activeInRegion - activeInIntersection;
                return cmds[iCommand].TurnOff ? activeOutsideIntersection : activeOutsideIntersection + intersection.Volume;
            }
        }
        return activeCubesAfterICommand(
            cmds.Length-1,
            new Region(new Step(-range,range),new Step(-range,range),new Step(-range,range))
        );
    }

    internal struct RebootStep
    {
        private bool state;
        private (int start,int end) x;
        private (int start,int end) y;
        private (int start,int end) z;
        public RebootStep(bool state,(int start,int end) x,(int start,int end) y,(int start,int end) z)
        {
            this.state = state;
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public bool State => this.state;
        public (int Start,int End) X => this.x;
        public (int Start,int End) Y => this.y;
        public (int Start,int End) Z => this.z;
    }

    internal struct Step
    {
        private int from;
        private int to;
        public Step(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
        public bool IsEmpty => from > to;
        public long Length => IsEmpty ? 0 : to-from+1;
        public Step Intersect(Step other) => new Step(Math.Max(this.from,other.from),Math.Min(this.to,other.to));
    }

    internal struct Command 
    {
        private bool turnOff;
        private Region region;
        public Command(bool turnOff, Region region)
        {
            this.turnOff = turnOff;
            this.region = region;
        }
        public bool TurnOff => this.turnOff;
        public Region Region => this.region;
    }

    internal struct Region
    {
        private Step x;
        private Step y;
        private Step z;
        public Region(Step x,Step y,Step z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public bool IsEmpty => x.IsEmpty || y.IsEmpty || z.IsEmpty;
        public long Volume => x.Length * y.Length * z.Length;
        public Region Intersect(Region other) => new Region(this.x.Intersect(other.x),this.y.Intersect(other.y),this.z.Intersect(other.z));
    }
}

