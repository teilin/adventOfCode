using adventofcode.Day01;
using adventofcode.Day02;
using adventofcode.Day03;
using adventofcode.Day04;
using adventofcode.Day05;
using adventofcode.Day06;
using adventofcode.Day07;
using adventofcode.Day08;
using adventofcode.Day09;
using adventofcode.Day10;
using adventofcode.Day11;
using adventofcode.Day12;
using adventofcode.Day13;
using adventofcode.Day14;
using adventofcode.Day15;
using adventofcode.Day16;
using adventofcode.Day17;
using adventofcode.Day18;
using adventofcode.Day19;
using adventofcode.Day20;
using adventofcode.Day21;
using adventofcode.Day22;
using adventofcode.Day23;
using adventofcode.Day24;
using adventofcode.Day25;
using Microsoft.Extensions.DependencyInjection;

namespace adventofcode.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<ISolver,SonarSweep>();
        services.AddScoped<ISolver,Dive>();
        services.AddScoped<ISolver,BinaryDiagnostic>();
        services.AddScoped<ISolver,GiantSquid>();
        services.AddScoped<ISolver,HydrothermalVenture>();
        services.AddScoped<ISolver,Lanternfish>();
        services.AddScoped<ISolver,TreacheryWhales>();
        services.AddScoped<ISolver,SavenSegmentSearch>();
        services.AddScoped<ISolver,SmokeBasin>();
        services.AddScoped<ISolver,SyntaxScoring>();
        services.AddScoped<ISolver,DumboOctopus>();
        services.AddScoped<ISolver,PassagePathing>();
        services.AddScoped<ISolver,TransparentOrigami>();
        services.AddScoped<ISolver,ExtendedPolymerization>();
        services.AddScoped<ISolver,Chiton>();
        services.AddScoped<ISolver,PacketDecoder>();
        services.AddScoped<ISolver,TrickShot>();
        services.AddScoped<ISolver,Snailfish>();
        services.AddScoped<ISolver,BeaconScanner>();
        services.AddScoped<ISolver,TrenchMap>();
        services.AddScoped<ISolver,DiracDice>();
        services.AddScoped<ISolver,ReactorReboot>();
        services.AddScoped<ISolver,Amphipod>();
        services.AddScoped<ISolver,ArithmeticLogicUnit>();
        services.AddScoped<ISolver,SeaCucumber>();

        return services;
    }
}