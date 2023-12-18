using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day18
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day18.txt");
      Part1(lines);
      //Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var instructions = lines.Select(x => new Instruction(int.Parse(x.Split(" ")[1]), Enum.Parse<Direction>(x.Split(" ")[0]), x.Split(" ")[2])).ToList();
      var size = 250; //  250;
      var visited = Enumerable.Range(0, 2 * size).Select(x => new char[size * 2].Select(c => '.').ToArray()).ToArray();
      var col = 200; // 200;
      var row = 200; //  200;
      foreach (var inst in instructions)
      {

        switch (inst)
        {
          case { Direction: Direction.L }:
          {
            for (var i = 0; i < inst.Length; i++)
            {
              col--;
              visited[row][col] = '#';
            }

            break;
          }
          case { Direction: Direction.R }:
          {
            for (var i = 0; i < inst.Length; i++)
            {
              col++;
              visited[row][col] = '#';
            }

            break;
          }
          case { Direction: Direction.D }:
          {
            for (var i = 0; i < inst.Length; i++)
            {
              row++;
              visited[row][col] = '#';
            }

            break;
          }
          case { Direction: Direction.U }:
          {
            for (var i = 0; i < inst.Length; i++)
            {
              row--;
              visited[row][col] = '#';
            }

            break;
          }
        }
      }

      DrawSurface(visited);
      File.WriteAllLines(@"C:\Temp\\lines_open.txt", visited.Select(l => string.Join("", l)));
      for (var index = 0; index < visited.Length - 1; index++)
      {
        var r = visited[index].ToList();
        if (visited[index].All(x => x != '#')) continue;

        var i = r.IndexOf('#', 0);
        var nextIndex = r.IndexOf('#', i + 1);
        while (nextIndex != -1)
        {
          while (r[i] == '#')
          {
            while (++i <= nextIndex)
            {
              visited[index][i] = '#';
            }
          }

          i = nextIndex; 
          nextIndex = r.IndexOf('#', i + 1);

          while (i < r.Count && nextIndex != -1 && r[i] == '.' && i < nextIndex)
          {
            if (visited[index - 1][i] == '#')
            {
              visited[index][i] = '#';
            }
            i++;
          }
          nextIndex = r.IndexOf('#', i + 1);
        }

        //nextIndex = r.ToList().IndexOf('#', i + 1);
        //if (nextIndex == -1) continue;

        //if (r[i + 2] == '.' && previous[(i + 2)..nextIndex].All(x => x == '#')) while (i++ < nextIndex) r[i] = '#';


        //previous = visited[index];
      }

      File.WriteAllLines(@"C:\Temp\\lines.txt", visited.Select(l => string.Join("", l)));
      DrawSurface(visited);

      var result = visited.Sum(l => l.Count(x => x == '#'));
      Console.WriteLine(result); // 54146 (too high)

    }

    private static void Part2(IEnumerable<string> lines)
    {

    }

    //private static bool IsInside(Position p, char[][] surface)
    //{
    //  //return -1 < p.r && p.r < surface.Length && -1 < p.c && p.c < surface[0].Length;
    //}

    //private static Position? GetNextPosition(Position p, IReadOnlyList<char[]> surface)
    //{
    //  return p.Direction switch
    //  {
    //    Direction.Right => surface[0].Length <= p.c + 1 ? null : p with { c = p.c + 1 },
    //    Direction.Left => p.c - 1 < 0 ? null : p with { c = p.c - 1 },
    //    Direction.Down => surface.Count <= p.r + 1 ? null : p with { r = p.r + 1 },
    //    Direction.Up => p.r - 1 < 0 ? null : p with { r = p.r - 1 },
    //    _ => null
    //  };
    //}

    //private static void Dig(ref char[][] surface, ref int rix, ref int cix, Instruction current, Instruction next)
    //{
    //  var tuple = Tuple.Create(current, next);
    //  if (current.Direction == next.Direction) throw new ArgumentOutOfRangeException();

    //  switch (tuple)
    //  {
    //    case { Item1.Direction: Direction.D, Item2.Direction: Direction.R }:
    //    for (var i = 0; i < tuple.Item1.Length; i++)
    //    {
    //      for (var j = 0; j < tuple.Item2.Length; j++)
    //      {
    //        surface[rix++][cix++] = '#';
    //      }
    //    }
    //    break;
    //    case { Item1.Direction: Direction.L, Item2.Direction: Direction.U }:
    //    for (var i = 0; i < tuple.Item1.Length; i++)
    //    {
    //      for (var j = 0; j < tuple.Item2.Length; j++)
    //      {
    //        surface[rix--][cix--] = '#';
    //      }
    //    }
    //    break;
    //    case { Item1.Direction: Direction.U, Item2.Direction: Direction.R }:
    //    for (var i = 0; i < tuple.Item1.Length; i++)
    //    {
    //      for (var j = 0; j < tuple.Item2.Length; j++)
    //      {
    //        surface[rix--][cix++] = '#';
    //      }
    //    }
    //    break;
    //    case { Item1.Direction: Direction.U, Item2.Direction: Direction.R }:
    //    for (var i = 0; i < tuple.Item1.Length; i++)
    //    {
    //      for (var j = 0; j < tuple.Item2.Length; j++)
    //      {
    //        surface[rix--][cix++] = '#';
    //      }
    //    }
    //    break;
    //  }


    //}

    private static void DrawSurface(char[][] surface)
    {
      foreach (var line in surface)
      {
        Console.WriteLine(string.Join("", line));
      }
      Console.WriteLine();
    }

    public record Instruction(int Length, Direction Direction, string Color);

    public enum Direction
    {
      U,
      D,
      L,
      R
    }
  }
}
