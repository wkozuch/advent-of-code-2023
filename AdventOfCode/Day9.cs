using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day9
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day9.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var histories = lines.Select(l => l.Split(" ").Select(long.Parse).ToArray()).ToList();
      long sum = 0;
      foreach (var h in histories)
      {
        var d = h.ToList();
        var value = d.Last();
        var level = 0;
        while (d.Distinct().Count() != 1)
        {
          Console.WriteLine(level++ + ": " + string.Join(", ", d));
          var n = new List<long>();
          for (var i = 0; i < d.Count - 1; i++)
          {
            n.Add(d[i + 1] - d[i]);
          }

          value += n.Last();

          d = n;
        }

        sum += value;
      }

      Console.WriteLine(sum); // 1882395907

    }

    private static void Part2(IEnumerable<string> lines)
    {
      var histories = lines.Select(l => l.Split(" ").Select(long.Parse).ToArray()).ToList();
      long sum = 0;
      foreach (var h in histories)
      {
        var d = h.ToList();
        var value = d.First();
        var level = 0;
        while (d.Distinct().Count() != 1)
        {
          Console.WriteLine(level++ + ": " + string.Join(", ", d));
          var n = new List<long>();
          for (var i = 0; i < d.Count - 1; i++)
          {
            n.Add(d[i + 1] - d[i]);
          }

          var sign = level % 2 == 0 ? 1 : -1;
          value += sign * n.First();

          d = n;
        }

        sum += value;
      }

      Console.WriteLine(sum); // 1005

    }
  }
}
