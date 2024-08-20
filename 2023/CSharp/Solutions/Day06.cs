using System.Buffers;
using System.ComponentModel;
using System.Text;
using Aoc.Interfaces;

namespace Aoc.Solutions;

public sealed class Day06 : ISolver
{
    private record Race(int Time = 0, int Distance = 0){};

    public Task Solve(Input input)
    {
        var races = Parse(input.inputArray);
        Console.WriteLine($"Part 1 -> {Part1(races)}");
        Console.WriteLine($"Part 2 -> {Part2(races)}");
        return Task.CompletedTask;
    }

    private int Part1(Race[] races)
    {
        var numWaysToWin = new List<int>();
        foreach(var race in races)
        {
            var posibleTimes = new List<int>();
            foreach(var time in Enumerable.Range(0, race.Time))
            {
                var speed = time*1;
                var distance = (race.Time-time)*speed;
                if(distance > race.Distance) posibleTimes.Add(time);
            }
            if(posibleTimes.Count > 0) numWaysToWin.Add(posibleTimes.Count);
        }
        var product = 1;
        foreach(var p in numWaysToWin) product *= p;
        return product;
    }

    private long Part2(Race[] races)
    {
        var timeBuilder = new StringBuilder();
        var distanceBuilder = new StringBuilder();
        foreach(var race in races)
        {
            timeBuilder.Append(race.Time);
            distanceBuilder.Append(race.Distance);
        }
        var time = long.Parse(timeBuilder.ToString());
        var distance = long.Parse(distanceBuilder.ToString());

        var result = 0L;
        for(var t=0;t<=time;t++)
        {
            var speed = t*1;
            var distance2 = (time-t)*speed;
            if(distance2 > distance) 
                result++;
        }
        return result;
    }

    private Race[] Parse(string[] input)
    {
        var time = input[0].Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var distance = input[1].Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var races = new List<Race>();
        for(var i=0;i<time.Length;i++)
        {
            var r = new Race(time[i], distance[i]);
            races.Add(r);
        }
        return races.ToArray();
    }
}