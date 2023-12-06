using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day6
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day6.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IReadOnlyList<string> lines)
    {
      var times = lines[0].Split("Time: ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
      var distances = lines[1].Split("Distance: ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
      var result = 1;
      for (var i = 0; i < times.Count; i++)
      {
        var time = times[i];
        var distance = distances[i];
        var wins = 0;
        for (var speed = 1; speed < time; speed++)
        {
          var timeLeft = time - speed;
          var raceDistance = speed * (timeLeft);
          Console.WriteLine($"Race {i + 1}, Speed {speed} RaceDistance  {raceDistance} {raceDistance > distance}");
          if (raceDistance > distance) wins++;
        }

        result *= wins;
        Console.WriteLine($"Race {i + 1}, Wins {wins}");
      }

      Console.WriteLine(result); // 806029445
    }

    private static void Part2(IReadOnlyList<string> lines)
    {
      var time = long.Parse(string.Join("", lines[0].Split("Time: ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()));
      var distance = long.Parse(string.Join("", lines[1].Split("Distance: ")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList()));

      var zero1 = Math.Ceiling(SolveQuadraticEquation(-1, time, -distance, true));
      var zero2 = Math.Ceiling(SolveQuadraticEquation(-1, time, -distance, false));

      var result = Math.Abs(zero2 - zero1);

      Console.WriteLine(result); // 29891250
    }

    private static double SolveQuadraticEquation(double a, double b, double c, bool pos)
    {
      var preRoot = b * b - 4 * a * c;
      if (preRoot < 0)
      {
        return double.NaN;
      }

      var sgn = pos ? 1.0 : -1.0;
      return (sgn * Math.Sqrt(preRoot) - b) / (2.0 * a);
    }
  }
}
