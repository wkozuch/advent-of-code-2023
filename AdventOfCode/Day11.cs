using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day11
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day11.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var cosmos = lines.Select(x => x.Select(y => y).ToList()).ToList();
      //DrawSurface(cosmos);
      var expandedCosmos = new List<List<char>>();
      foreach (var line in cosmos)
      {
        if (line.All(c => c == '.')) expandedCosmos.Add(line.ToList());
        expandedCosmos.Add(line.ToList());
      }

      var si = 0;
      for (var cix = 0; cix < cosmos[0].Count; cix++)
      {
        var col = cosmos.Select(l => l.ElementAt(cix)).ToList();
        if (col.Any(c => c != '.')) continue;
        foreach (var line in expandedCosmos)
        {
          line.Insert(cix + si, '.');
        }
        si++;
      }
      //Console.WriteLine();
      //DrawSpace(expandedCosmos);

      var galaxies = new List<Galaxy>();
      var i = 1;
      for (var row = 0; row < expandedCosmos.Count; row++)
      {
        for (var column = 0; column < expandedCosmos[row].Count; column++)
        {
          if (expandedCosmos[row][column] == '#') galaxies.Add(new Galaxy(i++, row, column));
        }
      }

      var result = 0;
      for (var g1 = 1; g1 < galaxies.Count + 1; g1++)
      {
        for (var g2 = g1 + 1; g2 < galaxies.Count + 1; g2++)
        {
          if (g1 == g2) continue;
          var distance = Distance(galaxies.Single(g => g.Index == g2), galaxies.Single(g => g.Index == g1));
          //Console.WriteLine($"{g1}=>{g2}: {distance}");
          result += distance;
        }
      }
      Console.WriteLine(result); // 9599070
    }

    private static void Part2(IEnumerable<string> lines)
    {
      var cosmos = lines.Select(x => x.Select(y => y).ToList()).ToList();
      // DrawSurface(cosmos);

      var emptyRows = new List<int>();
      for (var rIndex = 0; rIndex < cosmos.Count; rIndex++)
      {
        var line = cosmos[rIndex];
        if (line.All(c => c == '.'))
        {
          emptyRows.Add(rIndex);
        }
      }

      var emptyColumns = new List<int>();
      for (var cIndex = 0; cIndex < cosmos[0].Count; cIndex++)
      {
        var col = cosmos.Select(l => l.ElementAt(cIndex)).ToList();
        if (col.Any(c => c != '.')) continue;
        emptyColumns.Add(cIndex);
      }

      var galaxies = new List<Galaxy>();
      var i = 1;
      for (var row = 0; row < cosmos.Count; row++)
      {
        for (var column = 0; column < cosmos[row].Count; column++)
        {
          if (cosmos[row][column] == '#') galaxies.Add(new Galaxy(i++, row, column));
        }
      }

      const int factor = 1000000 - 1;
      long result = 0;
      for (var g1 = 1; g1 < galaxies.Count + 1; g1++)
      {
        for (var g2 = g1 + 1; g2 < galaxies.Count + 1; g2++)
        {
          if (g1 == g2) continue;
          var galaxy1 = galaxies.Single(g => g.Index == g1);
          var galaxy2 = galaxies.Single(g => g.Index == g2);
          var distance = Distance(galaxy2, galaxy1);
          var r = emptyRows.Count(x => Math.Min(galaxy1.Row, galaxy2.Row) < x && x < Math.Max(galaxy1.Row, galaxy2.Row));
          var c = emptyColumns.Count(x => Math.Min(galaxy1.Col, galaxy2.Col) < x && x < Math.Max(galaxy1.Col, galaxy2.Col));
          // Console.WriteLine($"{g1}=>{g2}: {distance + factor * r + factor * c}");

          result += distance + factor * r + factor * c;
        }
      }
      Console.WriteLine(result); // 842645913794
    }

    private static void DrawSpace(List<List<char>> surface)
    {
      foreach (var line in surface)
      {
        Console.WriteLine(string.Join("", line.ToList()));
      }
      Console.WriteLine();
    }

    public record Galaxy(int Index, int Row, int Col);

    public static int Distance(Galaxy g1, Galaxy g2)
    {
      return Math.Abs(g2.Row - g1.Row) + Math.Abs(g2.Col - g1.Col);
    }
  }
}
