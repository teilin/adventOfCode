public sealed class Day07 : Solver
{
    Directory current = new Directory{Name = "/"};
    IList<Directory> directories = new List<Directory>();

    public Day07(string inputPath) : base(inputPath) {}

    protected async override Task Setup()
    {
        directories.Add(current);
        foreach (var line in Inputs.Skip(1))
        {
            var split = line.Split();
            if (split[0]=="$")
            {
                if (split[1] == "cd")
                {
                    if (split[2] == "..")
                    {
                        current = current.Parent;
                    }

                    else
                    {

                        var child = current.Directories.SingleOrDefault(d => d.Name == split[2]);
                        if (child == null)
                        {
                            child = new Directory
                            {
                                Parent = current,
                                Name = split[2]
                            };
                            current.Directories.Add(child);
                            directories.Add(child);
                        }

                        current = child;
                    }
                }
            }
            else
            {
                if (split[0] == "dir")
                {
                    var child = current.Directories.SingleOrDefault(d => d.Name == split[1]);
                    if (child == null)
                    {
                        child = new Directory
                        {
                            Parent = current,
                            Name = split[1]
                        };
                        current.Directories.Add(child);
                        directories.Add(child);
                    }
                }
                else
                {
                    
                    var file = current.Files.SingleOrDefault(f => f.Name == split[1]);
                    if (file == null)
                    {
                        file = new File()
                        {
                            Name = split[1],
                            Size = long.Parse(split[0])
                        };
                        current.Files.Add(file);
                    }
                }
                
            }
        }
    }

    protected override async Task<object> Part1()
    {
        var total = SizeOfDirectories(100000L, directories).Sum();
        return await Task.FromResult(total);
    }

    protected override async Task<object> Part2()
    {
        var total = directories[0].Size;
        var result = directories.Where(d => d.Size >= total - 40000000).Min(d => d.Size).ToString();
        return await Task.FromResult(result);
    }

    private long[] SizeOfDirectories(long atMostSize, IList<Directory> dir)
    {
        var sizes = new List<long>();
        sizes.AddRange(dir.Where(w => w.Size <= atMostSize).Select(s => s.Size));
        foreach(var subDir in dir)
        {
            SizeOfDirectories(atMostSize, subDir.Directories);
        }
        return sizes.ToArray();
    }

    record Directory
    {
        public string Name { get; init; } = "/";
        public Directory? Parent { get; set; }
        public IList<Directory> Directories { get; set; } = new List<Directory>();
        public IList<File> Files { get; set; } = new List<File>();
        public long Size => Files.Sum(s => s.Size) + Directories.Sum(s => s.Size);
    }

    record File
    {
        public string Name { get; init; } = string.Empty;
        public long Size { get; init; }
    }
}