using System.Collections;
using System.Text.RegularExpressions;

public sealed class Day16 : Solver
{
    private Map _map;

    record Map(int[,] distances, Valve[] valves);
    record Valve(int id, string name, int flowRate, string[] tunnels);
    record Player(Valve valve, int distance);

    public Day16(string inputPath) : base(inputPath) {}

    protected override Task Setup()
    {
        var valves = new List<Valve>();
        foreach(var line in Inputs)
        {
            var name = Regex.Match(line, "Valve (.*) has").Groups[1].Value;
            var flow = int.Parse(Regex.Match(line, @"\d+").Groups[0].Value);
            var tunnels = Regex.Match(line, "to valves? (.*)").Groups[1].Value.Split(", ").ToArray();
            valves.Add(new Valve(0, name, flow, tunnels));
        }
        var v = valves
            .OrderByDescending(valve => valve.flowRate)
            .Select((v, i) => v with { id = i })
            .ToArray();
        _map = new Map(ComputeDistances(v), v);
        return Task.CompletedTask;
    }

    int[,] ComputeDistances(Valve[] valves) {
        // Bellman-Ford style distance calculation for every pair of valves
        var distances = new int[valves.Length, valves.Length];
        for (var i = 0; i < valves.Length; i++) {
            for (var j = 0; j < valves.Length; j++) {
                distances[i, j] = int.MaxValue;
            }
        }
        foreach (var valve in valves) {
            foreach (var target in valve.tunnels) {
                var targetNode = valves.Single(x => x.name == target);
                distances[valve.id, targetNode.id] = 1;
                distances[targetNode.id, valve.id] = 1;
            }
        }

        var n = distances.GetLength(0);
        var done = false;
        while (!done) {
            done = true;
            for (var source = 0; source < n; source++) {
                for (var target = 0; target < n; target++) {
                    if (source != target) {
                        for (var through = 0; through < n; through++) {
                            if (distances[source, through] == int.MaxValue || distances[through, target] == int.MaxValue) {
                                continue;
                            }
                            var cost = distances[source, through] + distances[through, target];
                            if (cost < distances[source, target]) {
                                done = false;
                                distances[source, target] = cost;
                                distances[target, source] = cost;
                            }
                        }
                    }
                }
            }
        }
        return distances;
    }

    protected override async Task<object> Part1()
    {
        return await Task.FromResult(Solve(true,30));
    }

    protected override async Task<object> Part2()
    {
        return await Task.FromResult(Solve(false, 26));
    }

    int Solve(bool singlePlayer, int time) {
        var start = _map.valves.Single(x => x.name == "AA");

        var valvesToOpen = new BitArray(_map.valves.Length);
        for (var i = 0; i < _map.valves.Length; i++) {
            if (_map.valves[i].flowRate > 0) {
                valvesToOpen[i] = true;
            }
        }

        if (singlePlayer) {
            // int.MaxValue is used here as a dummy player that doesn't really do anything, it just
            // walks towards the start node
            return MaxFlow(_map, 0, 0, new Player(start, 0), new Player(start, int.MaxValue), valvesToOpen, time);
        } else {
            return MaxFlow(_map, 0, 0, new Player(start, 0), new Player(start, 0), valvesToOpen, time);
        }
    }

    int MaxFlow(
        Map map,               // this is our map as per task input.
        int maxFlow,           // is the current maximum we found (call with 0), this is used internally to shortcut
        int currentFlow,       // the flow produced by the currently investigated steps (on the stack)
        Player player0,        // this is the 'human' player
        Player player1,        // this can be a second player, use distance = int.MaxValue to make it inactive
        BitArray valvesToOpen, // these valves can still be open
        int remainingTime      // and the remaining time
    ) {

        // briefly: we advance the simulation and collect what states the players can go -> recurse
        // use lots of early exits to make this approach practical (Residue is an important concept)

        // One of the players is standing next to a valve:
        if (player0.distance != 0 && player1.distance != 0) {
            throw new ArgumentException();
        }

        // Compute the next states for each player:
        var nextStatesByPlayer = new Player[2][];

        for (var iplayer = 0; iplayer < 2; iplayer++) {

            var player = iplayer == 0 ? player0 : player1;

            if (player.distance > 0) {
                // this player just steps forward towards the valve
                nextStatesByPlayer[iplayer] = new[] { player with { distance = player.distance - 1 } };

            } else if (valvesToOpen[player.valve.id]) {
                // the player is next to the valve, the valve is still closed, let's open:
                // (this takes 1 time, so multiply with remainingTime -1)
                currentFlow += player.valve.flowRate * (remainingTime - 1);

                if (currentFlow > maxFlow) {
                    maxFlow = currentFlow;
                }

                valvesToOpen = new BitArray(valvesToOpen); // copy on write
                valvesToOpen[player.valve.id] = false;

                // in the next round this player will take some new target, 
                // but it already used up it's 1 second this round for opening the valve
                nextStatesByPlayer[iplayer] = new[] { player };

            } else {
                // the valve is already open, let's try each valves that are still closed:
                // this is where brancing happens

                var nextStates = new List<Player>();

                for (var i = 0; i < valvesToOpen.Length; i++) {
                    if (valvesToOpen[i]) {
                        var nextValve = map.valves[i];
                        var distance = map.distances[player.valve.id, nextValve.id];
                        // the player moves in this time slot towards the valve, so use distance - 1 here
                        nextStates.Add(new Player(nextValve, distance - 1));
                    }
                }

                nextStatesByPlayer[iplayer] = nextStates.ToArray();
            }
        }

        // ran out of time, cannot improve maxFlow
        remainingTime--;
        if (remainingTime < 1) {
            return maxFlow;
        }

        // the is not enough juice left for the remaining time to improve on maxFlow
        // we can shortcut here
        if (currentFlow + Residue(valvesToOpen, map, remainingTime) <= maxFlow) {
            return maxFlow;
        }

        // all is left is going over every possible step combinations for each players:
        for (var i0 = 0; i0 < nextStatesByPlayer[0].Length; i0++) {
            for (var i1 = 0; i1 < nextStatesByPlayer[1].Length; i1++) {

                player0 = nextStatesByPlayer[0][i0];
                player1 = nextStatesByPlayer[1][i1];

                // there is no point in walking to the same valve
                // if one of the players has other thing to do:
                if ((nextStatesByPlayer[0].Length > 1 || nextStatesByPlayer[1].Length > 1) && player0.valve == player1.valve) {
                    continue;
                }

                // this is an other optimization, if both players are walking
                // we can advance time until one of them reaches target:
                var advance = 0;
                if (player0.distance > 0 && player1.distance > 0) {
                    advance = Math.Min(player0.distance, player1.distance);
                    player0 = player0 with { distance = player0.distance - advance };
                    player1 = player1 with { distance = player1.distance - advance };
                }

                maxFlow = MaxFlow(
                    map,
                    maxFlow,
                    currentFlow,
                    player0,
                    player1,
                    valvesToOpen,
                    remainingTime - advance
                );
            }
        }

        return maxFlow;
    }

    int Residue(BitArray valvesToOpen, Map map, int remainingTime) {
        var flow = 0;
        for (var i = 0; i < valvesToOpen.Length; i++) {
            if (valvesToOpen[i]) {
                if (remainingTime <= 0) {
                    break;
                }

                flow += map.valves[i].flowRate * (remainingTime - 1);

                if ((i & 1) == 0) {
                    remainingTime--;
                }
            }
        }
        return flow;
    }
}