using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace AdventOfCode
{
  class Day14
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day14.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var surface = lines.Select(x => x.Select(y => y).ToList()).ToList();
      var result = 0;
      var newSurface = Enumerable.Range(0, surface.Count)
        .Select(_ => new List<char>())
        .ToList();
      DrawSurface(surface);

      var rix = 0;
      for (var cix = 0; cix < surface[0].Count; cix++)
      {
        var col = surface.Select(l => l.ElementAt(cix)).ToList();
        var newCol = Enumerable.Range(0, col.Count).Select(x => '.').ToArray();
        var zeroIndex = 0;
        for (var i = 0; i < col.Count; i++)
        {
          var e = col[i];
          switch (e)
          {
            case 'O':
            result += col.Count - zeroIndex;
            newCol[zeroIndex++] = e;
            break;
            case '#':
            zeroIndex = i + 1;
            newCol[i] = e;
            break;
          }
        }

        foreach (var c in newCol)
        {
          newSurface[rix++].Add(c);
        }

        rix = 0;
      }

      DrawSurface(newSurface);

      Console.WriteLine(result); // 110821
    }

    private static void Part2(IEnumerable<string> lines)
    {
      var surface = lines.Select(x => x.Select(y => y).ToList()).ToList();
      var result = 0;
      DrawSurface(surface);
      var newSurface = surface;
      var results = new List<int>();
      for (var i = 0; i < 1000000000; i++)
      {
        newSurface = TiltNorth(newSurface);
        newSurface = TiltWest(newSurface);
        newSurface = TiltSouth(newSurface);
        newSurface = TiltEast(newSurface, out var r);
        if (results.Contains(r))
        {
          Console.WriteLine($"{i:00}: {result}");
          result = r;
          break;
        }
        results.Add(r);
      }

      Console.WriteLine(result); // 83516
    }

    private static List<List<char>> TiltNorth(List<List<char>> surface)
    {
      var newSurface = Enumerable.Range(0, surface.Count)
        .Select(_ => new List<char>())
        .ToList();

      var rix = 0;
      for (var cix = 0; cix < surface[0].Count; cix++)
      {
        var col = surface.Select(l => l.ElementAt(cix)).ToList();
        var newCol = Enumerable.Range(0, col.Count).Select(x => '.').ToArray();
        var zeroIndex = 0;
        for (var i = 0; i < col.Count; i++)
        {
          var e = col[i];
          switch (e)
          {
            case 'O':
            newCol[zeroIndex++] = e;
            break;
            case '#':
            zeroIndex = i + 1;
            newCol[i] = e;
            break;
          }
        }

        foreach (var c in newCol)
        {
          newSurface[rix++].Add(c);
        }

        rix = 0;
      }

      return newSurface;
    }

    private static List<List<char>> TiltSouth(List<List<char>> surface)
    {
      var newSurface = Enumerable.Range(0, surface.Count)
        .Select(_ => new List<char>())
        .ToList();

      var rix = 0;
      for (var cix = 0; cix < surface[0].Count; cix++)
      {
        var col = surface.Select(l => l.ElementAt(cix)).ToList();
        var newCol = Enumerable.Range(0, col.Count).Select(x => '.').ToArray();
        var zeroIndex = col.Count - 1;
        for (var i = col.Count - 1; 0 <= i; i--)
        {
          var e = col[i];
          switch (e)
          {
            case 'O':
            newCol[zeroIndex--] = e;
            break;
            case '#':
            zeroIndex = i - 1;
            newCol[i] = e;
            break;
          }
        }

        foreach (var c in newCol)
        {
          newSurface[rix++].Add(c);
        }

        rix = 0;
      }

      return newSurface;
    }

    private static List<List<char>> TiltEast(List<List<char>> surface, out int result)
    {
      var newSurface = Enumerable.Range(0, surface.Count)
        .Select(_ => new List<char>())
        .ToList();
      result = 0;

      for (var rix = 0; rix < surface[0].Count; rix++)
      {
        var row = surface[rix].ToList();
        var newRow = Enumerable.Range(0, row.Count).Select(x => '.').ToArray();
        var zeroIndex = row.Count - 1;
        for (var i = row.Count - 1; 0 <= i; i--)
        {
          var e = row[i];
          switch (e)
          {
            case 'O':
            result += surface[0].Count - rix;
            newRow[zeroIndex--] = e;
            break;
            case '#':
            zeroIndex = i - 1;
            newRow[i] = e;
            break;
          }
        }

        newSurface[rix] = newRow.ToList();
      }

      return newSurface;
    }

    private static List<List<char>> TiltWest(List<List<char>> surface)
    {
      var newSurface = Enumerable.Range(0, surface.Count)
        .Select(_ => new List<char>())
        .ToList();

      for (var rix = 0; rix < surface[0].Count; rix++)
      {
        var row = surface[rix].ToList();
        var newRow = Enumerable.Range(0, row.Count).Select(x => '.').ToArray();
        var zeroIndex = 0;
        for (var i = 0; i < row.Count; i++)
        {
          var e = row[i];
          switch (e)
          {
            case 'O':
            newRow[zeroIndex++] = e;
            break;
            case '#':
            zeroIndex = i + 1;
            newRow[i] = e;
            break;
          }
        }

        newSurface[rix] = newRow.ToList();
      }

      return newSurface;
    }

    private static void DrawSurface(List<List<char>> surface)
    {
      foreach (var line in surface)
      {
        Console.WriteLine(string.Join("", line));
      }

      Console.WriteLine();
    }

  }
}
