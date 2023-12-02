using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day2
  {
    private static readonly Dictionary<string, int> limitsForColor = new() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };

    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day2.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(string[] lines)
    {
      var no = 1;
      var sum = 0;

      foreach (var line in lines)
      {
        var sets = line.Split(": ").Last().Split("; ");
        var isPossible = true;

        foreach (var s in sets)
        {
          var pairs = s.Split(", ");
          foreach (var p in pairs)
          {
            var pair = p.Split(" ");
            var n = int.Parse(pair.First());
            var color = pair.Last();
            isPossible = isPossible && n <= limitsForColor[color];
          }
        }
        Console.WriteLine($"Game {no}: {isPossible}");
        if (isPossible) sum += no;
        no++;
      }
      Console.WriteLine($"Part one: {sum}");
    }

    private static void Part2(string[] lines)
    {
      var no = 1;
      var sum = 0;

      foreach (var line in lines)
      {
        var sets = line.Split(": ").Last().Split("; ");
        var game = new Dictionary<string, int> { { "red", 0 }, { "green", 0 }, { "blue", 0 } };

        foreach (var s in sets)
        {
          var pairs = s.Split(", ");
          foreach (var p in pairs)
          {
            var pair = p.Split(" ");
            var n = int.Parse(pair.First());
            var color = pair.Last();
            game[color] = Math.Max(game[color], n);
          }
        }
        var power = game["red"] * game["green"] * game["blue"];
        sum += power;
        Console.WriteLine($"Game {no}: {power}");
        no++;
      }
      Console.WriteLine($"Part two: {sum}");
    }
  }
}
