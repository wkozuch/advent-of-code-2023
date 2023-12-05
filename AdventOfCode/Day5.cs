using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day5
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day5.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IReadOnlyList<string> lines)
    {
      var seeds = lines[0].Split("seeds: ")[1].Split(" ");
      var locations = new List<double>();
      foreach (var s in seeds)
      {
        var seed = double.Parse(s);
        var logging = "";
        var mapped = false;
        for (int i = 1; i < lines.Count; i++)
        {

          if (lines[i] == "") continue;
          if (lines[i].EndsWith(":"))
          {
            logging = lines[i].Split(" ")[0];
            mapped = false;
            continue;
          }

          if (mapped) continue;

          var mapping = lines[i].Split(" ").Select(x => double.Parse(x)).ToList();
          var destinationStart = mapping[0];
          var sourceStart = mapping[1];
          var range = mapping[2];

          if (sourceStart <= seed && seed <= sourceStart + range)
          {
            seed = destinationStart + (seed - sourceStart);
            mapped = true;
            Console.WriteLine($"{logging} : {seed}");
          }

        }

        locations.Add(seed);
        Console.WriteLine(seed);
      }

      Console.WriteLine(locations.Min()); // 806029445
    }

    private static void Part2(IReadOnlyList<string> lines)
    {
      var ranges = lines[0].Split("seeds: ")[1].Split(" ").Select(x => long.Parse(x)).ToList();
      var seedRanges = new List<(long, long)>();
      for (var i = 0; i < ranges.Count; i += 2)
      {
        seedRanges.Add((ranges[i], ranges[i] + ranges[i + 1] - 1));
      }
      
      var locations = new List<(double, double)>();
      var mappings = new List<Mapping>();

      var n = 0;
      var label = "";

      for (var i = 2; i < lines.Count; i++)
      {
        if (lines[i].EndsWith(":"))
        {
          label = $"#{++n:000} " + lines[i].Split(" ")[0];
          continue;
        }

        if (lines[i] == "") continue;

        var mapping = lines[i].Split(" ").Select(long.Parse).ToList();
        var destinationStart = mapping[0];
        var destinationEnd = destinationStart + mapping[2] - 1;
        var sourceStart = mapping[1];
        var sourceEnd = sourceStart + mapping[2] - 1;

        mappings.Add(new Mapping(n, label, destinationStart, destinationEnd, sourceStart, sourceEnd));
      }

      foreach (var m in mappings.OrderBy(x => x.Level).ThenBy(x => x.SourceStart)) Console.WriteLine(m);

      Console.WriteLine();

      foreach (var range in seedRanges)
      {
        var currentItems = new List<(double start, double end)>() { range };

        for (var level = 1; level < 8; level++)
        {
          var nextItems = new List<(double start, double end)>();

          for (var i = 0; i < currentItems.Count; i++)
          {
            var item = currentItems[i];
            var m = mappings.Where(x => x.Level == level);
            Console.WriteLine($"Level#{level}, {m.First().MapType} Seed range: {item.start} to {item.end}");
            var mapped = false;

            foreach (var r in m)
            {
              if (r.SourceStart <= item.start && item.end <= r.SourceEnd)
              {
                Console.WriteLine(r);
                var dS = item.start - r.SourceStart;
                var dT = item.end - item.start;
                mapped = true;
                nextItems.Add((r.DestinationStart + dS, r.DestinationStart + dS + dT));
                //start = r.DestinationStart + dS;
                //end = start + dT;
                break;
              }

              if (r.SourceStart <= item.start && item.start < r.SourceEnd && r.SourceEnd < item.end)
              {
                Console.WriteLine($"Outside on the end side {r}");
                Console.WriteLine($"Correct range is {item.start} to {r.SourceEnd}");
                var dS = item.start - r.SourceStart;
                var dT = item.end - item.start;
                mapped = true;
                nextItems.Add((r.DestinationStart + dS, r.DestinationEnd));
                currentItems.Add((r.SourceEnd + 1, item.end));
                break;
              }

              if (item.start <= r.SourceStart && r.SourceStart < item.end && item.end <= r.SourceEnd)
              {
                Console.WriteLine($"Outside on the start side {r}");
                Console.WriteLine($"Correct range is {r.SourceStart} to {item.end}");
                var dS = item.start - r.SourceStart;
                var dT = item.end - item.start + dS;
                mapped = true;
                nextItems.Add((r.DestinationStart, r.DestinationStart + dT));
                currentItems.Add((item.start, r.SourceStart - 1));
                break;
              }
            }
            
            if (!mapped) { nextItems.Add(item); }
          }
          currentItems = nextItems;
        }

        locations.AddRange(currentItems);
        Console.WriteLine();
      }

      Console.WriteLine(locations.Min(x => x.Item1)); // 59370572
    }

    public class Mapping
    {
      public int Level { get; }
      public string MapType { get; }
      public long DestinationStart { get; }
      public long DestinationEnd { get; }
      public long SourceStart { get; }
      public long SourceEnd { get; }

      public Mapping(int level, string mapType, long destinationStart, long destinationEnd, long sourceStart, long sourceEnd)
      {
        Level = level;
        MapType = mapType;
        DestinationStart = destinationStart;
        DestinationEnd = destinationEnd;
        SourceStart = sourceStart;
        SourceEnd = sourceEnd;
      }

      public override string ToString()
      {
        return $"{MapType} Source[{SourceStart};{SourceEnd}] Destination [{DestinationStart};{DestinationEnd}] ";
      }
    }
  }
}
