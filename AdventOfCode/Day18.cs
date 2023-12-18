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
      // Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var instructions = lines.Select(x => new Instruction(int.Parse(x.Split(" ")[1]), Enum.Parse<Direction>(x.Split(" ")[0]), x.Split(" ")[2])).ToList();
      var size = 250; //  250;
      var visited = Enumerable.Range(0, 2 * size).Select(x => new char[size * 2].Select(c => '.').ToArray()).ToArray();
      var col = 200; // 200;
      var row = 200; //  200;
      for (int i1 = 0; i1 < instructions.Count; i1++)
      {
        var inst = instructions[i1];
        switch (inst)
        {
          case { Direction: Direction.L }:
          {
            for (var i = 0; i < inst.Length; i++)
            {
              visited[row][col] = '#';
              if (visited[row + 1][col] == '.') visited[row + 1][col] = '*';
              col--;
            }
            break;
          }
          case { Direction: Direction.R }:
          {
            for (var i = 0; i < inst.Length; i++)
            {

              visited[row][col] = '#';
              if (visited[row - 1][col] == '.') visited[row - 1][col] = '*';
              col++;
            }

            break;
          }
          case { Direction: Direction.D }:
          {
            for (var i = 0; i < inst.Length; i++)
            {
              visited[row][col] = '#';
              if (visited[row][col + 1] == '.') visited[row][col + 1] = '*';
              row++;
            }

            break;
          }
          case { Direction: Direction.U }:
          {
            for (var i = 0; i < inst.Length; i++)
            {

              visited[row][col] = '#';
              if (visited[row][col - 1] == '.') visited[row][col - 1] = '*';
              row--;
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
          if (visited[index][i..nextIndex].All(x => x != '*'))
          {
            while (i < nextIndex)
            {
              visited[index][++i] = '#';
            }
          }
          //else
          //{
          //  while (i < nextIndex)
          //  {
          //    if (visited[index - 1][i] == '#')
          //    {
          //      visited[index][i] = '#';
          //    }
          //    i++;
          //  }
          //  }


          //while (r[i] == '#')
          //{
          //  while (++i <= nextIndex)
          //  {
          //    visited[index][i] = '#';
          //  }
          //}

          //i = nextIndex; 
          //nextIndex = r.IndexOf('#', i + 1);

          //while (i < r.Count && nextIndex != -1 && r[i] == '.' && i < nextIndex)
          //{
          //  if (visited[index - 1][i] == '#')
          //  {
          //    visited[index][i] = '#';
          //  }
          //  i++;
          //}
          i = nextIndex;
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
      Console.WriteLine(result); // 45159

    }

    private static void Part2(IEnumerable<string> lines)
    {
      var instructions = lines.Select(x => x.Split(" ")[2]).
        Select(x => new Instruction(Convert.ToInt32(x[2..7], 16), CharToDirection(x), x))
        .ToList();
      var size = 250; //  250;
      var visited = new List<Position>();
      long col = 0; // 200;
      long row = 0; //  200;
      for (int i1 = 0; i1 < instructions.Count; i1++)
      {
        var inst = instructions[i1];
        switch (inst)
        {
          case { Direction: Direction.L }:
          {
            visited.Add(new Position(row, col, '#'));
            visited.Add(new Position(row, col - inst.Length, '#'));
            visited.Add(new Position(row + 1, col, '*'));
            visited.Add(new Position(row + 1, col - inst.Length, '*'));
            col -= inst.Length;
            break;
          }
          case { Direction: Direction.R }:
          {
            visited.Add(new Position(row, col, '#'));
            visited.Add(new Position(row, col + inst.Length, '#'));
            visited.Add(new Position(row - 1, col, '*'));
            visited.Add(new Position(row - 1, col + inst.Length, '*'));
            col += inst.Length;
            break;
          }
          case { Direction: Direction.D }:
          {
            visited.Add(new Position(row, col, '#'));
            visited.Add(new Position(row + inst.Length, col, '#'));
            visited.Add(new Position(row, col + 1, '*'));
            visited.Add(new Position(row + inst.Length, col + 1, '*'));
            row += inst.Length;
            break;
          }
          case { Direction: Direction.U }:
          {
            visited.Add(new Position(row, col, '#'));
            visited.Add(new Position(row - inst.Length, col, '#'));
            visited.Add(new Position(row, col - 1, '*'));
            visited.Add(new Position(row - inst.Length, col - 1, '*'));
            row -= inst.Length;
            break;
          }
        }
      }

      var rows = visited.Select(x => x.Row).OrderBy(x => x).ToList();
      visited = visited.OrderBy(x => x.Row).ThenBy(x => x.Column).ToList();
      long result = 0;
      long prevRow = 0;
      long delta = 0;
      foreach (var r in rows)
      {
        prevRow = r - prevRow;
        var positions = visited.Where(x => x.Row == r).ToList();
        //if (visited[index].All(x => x != '#')) continue;
        var i = 1;
        var p1 = positions.First();
        var p2 = positions.Skip(i).FirstOrDefault();
        while (p2 != null)
        {
          if (p1.Char != '*' && p2.Char != '*')
          {
            delta += (p2.Column - p1.Column);
          }
          p1 = p2;
          p2 = positions.Skip(++i).FirstOrDefault();
        }
        result += delta * prevRow;
      }
      Console.WriteLine(result); // 45159

    }

    private static Direction CharToDirection(string x)
    {
      var values = Enum.GetValues(typeof(Direction)).Cast<Direction>();
      return values.FirstOrDefault(e => (int)e == x[7] - '0');
    }

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
      R = 0,
      D = 1,
      L = 2,
      U = 3
    }

    public record Position(long Row, long Column, char Char);
  }
}
