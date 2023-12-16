using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day15
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day15.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var sequence = lines.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();
      var result = 0;
      foreach (var s in sequence)
      {
        var currentValue = 0;
        foreach (var c in s)
        {
          currentValue += (int)c;
          var t = currentValue * 17;
          currentValue = t % 256;
        }
        result += currentValue;
        Console.WriteLine($"{s} becomes {currentValue}");
      }

      Console.WriteLine(result); // 510801

    }

    private static void Part2(IEnumerable<string> lines)
    {
      var sequence = lines.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()).ToList();
      var boxes = new Dictionary<int, List<Lens>>();
      foreach (var s in sequence)
      {
        var split = s.Split('-', '=');
        var label = split[0];
        var box = GetHashValue(label);
        if (s.EndsWith('-'))
        {
          if (boxes.ContainsKey(box) && boxes[box].Any(l => l.Label == label)) boxes[box].RemoveAll(l => l.Label == label);
        }

        if (s.Contains('='))
        {
          var focalPoint = split.Last();
          if (!boxes.ContainsKey(box)) boxes.Add(box, new List<Lens>());
          if (boxes[box].Any(l => l.Label == label))
          {
            var index = boxes[box].FindIndex(l => l.Label == label);
            boxes[box][index] = new Lens(label, int.Parse(focalPoint));
          }
          else
          {
            boxes[box].Add(new Lens(label, int.Parse(focalPoint)));
          }
        }

        Console.WriteLine("After " + s);
        foreach (var b in boxes)
        {
          Console.WriteLine($"Box {b.Key}: " + string.Join(" ", b.Value));
        }
        Console.WriteLine();
      }

      var result = 0;
      for (var i = 0; i < 256; i++)
      {
        if (!boxes.ContainsKey(i)) continue;
        for (var s = 0; s < boxes[i].Count; s++)
        {
          result += (i + 1) * (s + 1) * boxes[i][s].FocalPoint;
        }
      }
      Console.WriteLine(result); // 212763

    }

    public record Lens(string Label, int FocalPoint)
    {
      public override string ToString()
      {
        return $"[{Label} {FocalPoint}]";
      }
    }

    private static int GetHashValue(string s)
    {
      var currentValue = 0;
      foreach (var c in s)
      {
        currentValue += c;
        var t = currentValue * 17;
        currentValue = t % 256;
      }

      return currentValue;
    }
  }
}
