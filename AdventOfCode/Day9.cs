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
      var histories = lines.Select(l => l.Split(" ").Select(x => long.Parse(x)).ToArray()).ToList();
      long sum = 0;
      foreach (var h in histories)
      {
        var deltas = new Dictionary<int, List<long>>();

        var d = h.ToList();
        var n = new List<long>();
        var level = 0;
        while (d.Distinct().Count() != 1)
        {
          Console.WriteLine(level + ": " + string.Join(", ", d));
          for (var i = 0; i < d.Count - 1; i++)
          {
            n.Add(d[i + 1] - d[i]);
          }
          //Console.WriteLine(level + 1 + ": " + string.Join(", ", n));
          deltas.Add(level++, n);
          d = n;
          n = new List<long>();
        }

        var value = h.Last() + deltas.Sum(x => x.Value.Last());
        Console.WriteLine("Sum: " + value + " = " + h.Last() + " + " + string.Join(" + ", deltas.Values.Select(x => x.Last())));
        sum += value;
      }

      Console.WriteLine(sum); // 1882395907

    }

    private static void Part2(IEnumerable<string> lines)
    {
      var histories = lines.Select(l => l.Split(" ").Select(x => long.Parse(x)).ToArray()).ToList();
      long sum = 0;
      foreach (var h in histories)
      {
        var firstElements = new List<long>();

        var d = h.ToList();
        var value = h.First();
        var level = 0;
        while (d.Distinct().Count() != 1)
        {
          Console.WriteLine(level++ + ": " + string.Join(", ", d));
          var n = new List<long>();
          for (var i = 0; i < d.Count - 1; i++)
          {
            n.Add(d[i + 1] - d[i]);
          }

          firstElements.Add(n.First());
          d = n;
        }
        
        for (var i = 0; i < firstElements.Count; i++)
        {
          var sign = i % 2 == 0 ? -1 : 1;
          value += sign* firstElements[i];
        }
        sum += value;
      }

      Console.WriteLine(sum); // 1005

    }
  }
}
