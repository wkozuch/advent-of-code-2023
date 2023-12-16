using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day16
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day16.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
      DrawSurface(surface);
      var visited = Enumerable.Range(0, surface[0].Length).Select(x => new char[surface[x].Length].Select(c => '.').ToArray()).ToArray();
      var visitedPositions = new List<Position>();
      var start = new Position(0, -1, Direction.Right);
      var rays = new List<Position>() { start };

      for (var i = 0; i < rays.Count; i++)
      {
        //Console.Clear();
        //DrawSurface(surface);
        //DrawSurface(visited);
        var draw = 0;
        var current = rays[i];
        if (i != 0) visited[current.r][current.c] = '#';
        //var next = GetNextPosition(current, surface);
        while (current == start || IsInside(current, surface))
        {
          //Console.Clear();
          //DrawSurface(surface);
          //if (draw++ % 10 == 0)
          //{
          //  Console.Clear();
          //  DrawSurface(visited);
          //}
          if (current != start) visited[current.r][current.c] = '#';
          var next = GetNextPosition(current, surface);
          if (next == null || visitedPositions.Any(p => p.c == current.c && p.r == current.r && p.Direction == current.Direction)) break;
          visitedPositions.Add(current);

          var nextTile = surface[next.r][next.c];

          switch (current)
          {
            case { Direction: Direction.Left } when nextTile == '/':
            current = new Position(current.r, current.c - 1, Direction.Down);
            break;
            case { Direction: Direction.Right } when nextTile == '/':
            current = new Position(current.r, current.c + 1, Direction.Up);
            break;
            case { Direction: Direction.Up } when nextTile == '/':
            current = new Position(current.r - 1, current.c, Direction.Right);
            break;
            case { Direction: Direction.Down } when nextTile == '/':
            current = new Position(current.r + 1, current.c, Direction.Left);
            break;
            case { Direction: Direction.Right } when nextTile == '\\':
            current = new Position(current.r, current.c + 1, Direction.Down);
            break;
            case { Direction: Direction.Left } when nextTile == '\\':
            current = new Position(current.r, current.c - 1, Direction.Up);
            break;
            case { Direction: Direction.Up } when nextTile == '\\':
            current = new Position(current.r - 1, current.c, Direction.Left);
            break;
            case { Direction: Direction.Down } when nextTile == '\\':
            current = new Position(current.r + 1, current.c, Direction.Right);
            break;
            case { Direction: Direction.Left } when nextTile is '-' or '.':
            current = new Position(current.r, current.c - 1, Direction.Left);
            break;
            case { Direction: Direction.Right } when nextTile is '-' or '.':
            current = new Position(current.r, current.c + 1, Direction.Right);
            break;
            case { Direction: Direction.Down } when nextTile is '|' or '.':
            current = new Position(current.r + 1, current.c, Direction.Down);
            break;
            case { Direction: Direction.Up } when nextTile is '|' or '.':
            current = new Position(current.r - 1, current.c, Direction.Up);
            break;
            case { Direction: Direction.Right } when nextTile == '|':
            {
              if (!rays.Any(r => r.r == current.r && r.c == current.c + 1 && r.Direction == Direction.Down)) rays.Add(new Position(current.r, current.c + 1, Direction.Down));
              current = new Position(current.r, current.c + 1, Direction.Up);
              break;
            }
            case { Direction: Direction.Left } when nextTile == '|':
            {
              if (!rays.Any(r => r.r == current.r && r.c == current.c - 1 && r.Direction == Direction.Down)) rays.Add(new Position(current.r, current.c - 1, Direction.Down));
              current = new Position(current.r, current.c - 1, Direction.Up);
              break;
            }
            case { Direction: Direction.Down } when nextTile == '-':
            {
              if (!rays.Any(r => r.r == current.r + 1 && r.c == current.c && r.Direction == Direction.Right)) rays.Add(new Position(current.r + 1, current.c, Direction.Right));
              current = new Position(current.r + 1, current.c, Direction.Left);
              break;
            }
            case { Direction: Direction.Up } when nextTile == '-':
            {
              if (!rays.Any(r => r.r == current.r - 1 && r.c == current.c && r.Direction == Direction.Right)) rays.Add(new Position(current.r - 1, current.c, Direction.Right));
              current = new Position(current.r - 1, current.c, Direction.Left);
              break;
            }
          }
        }
      }
      DrawSurface(visited);

      var result = visited.Sum(l => l.Count(x => x == '#'));
      Console.WriteLine(result); // 7798

    }

    private static void Part2(IEnumerable<string> lines)
    {
      var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
      DrawSurface(surface);

      var starts = Enumerable.Range(0, surface[0].Length).Select(x => new Position(-1, x, Direction.Down)).ToList();
      starts.AddRange(Enumerable.Range(0, surface[0].Length).Select(x => new Position(surface.Length, x, Direction.Up)).ToList());
      starts.AddRange(Enumerable.Range(0, surface.Length).Select(x => new Position(x, surface[0].Length, Direction.Left)).ToList());
      starts.AddRange(Enumerable.Range(0, surface.Length).Select(x => new Position(x, -1, Direction.Right)).ToList());
      var max = double.MinValue;
      foreach (var start in starts)
      {
        var visited = Enumerable.Range(0, surface[0].Length).Select(x => new char[surface[x].Length].Select(c => '.').ToArray()).ToArray();
        var visitedPositions = new List<Position>();
        var rays = new List<Position>() { start };
        for (var i = 0; i < rays.Count; i++)
        {
          var current = rays[i];
          if (i != 0) visited[current.r][current.c] = '#';
          while (current == start || IsInside(current, surface))
          {
            if (current != start) visited[current.r][current.c] = '#';
            var next = GetNextPosition(current, surface);
            if (next == null || visitedPositions.Any(p => p.c == current.c && p.r == current.r && p.Direction == current.Direction)) break;
            visitedPositions.Add(current);

            var nextTile = surface[next.r][next.c];

            switch (current)
            {
              case { Direction: Direction.Left } when nextTile == '/':
              current = new Position(current.r, current.c - 1, Direction.Down);
              break;
              case { Direction: Direction.Right } when nextTile == '/':
              current = new Position(current.r, current.c + 1, Direction.Up);
              break;
              case { Direction: Direction.Up } when nextTile == '/':
              current = new Position(current.r - 1, current.c, Direction.Right);
              break;
              case { Direction: Direction.Down } when nextTile == '/':
              current = new Position(current.r + 1, current.c, Direction.Left);
              break;
              case { Direction: Direction.Right } when nextTile == '\\':
              current = new Position(current.r, current.c + 1, Direction.Down);
              break;
              case { Direction: Direction.Left } when nextTile == '\\':
              current = new Position(current.r, current.c - 1, Direction.Up);
              break;
              case { Direction: Direction.Up } when nextTile == '\\':
              current = new Position(current.r - 1, current.c, Direction.Left);
              break;
              case { Direction: Direction.Down } when nextTile == '\\':
              current = new Position(current.r + 1, current.c, Direction.Right);
              break;
              case { Direction: Direction.Left } when nextTile is '-' or '.':
              current = new Position(current.r, current.c - 1, Direction.Left);
              break;
              case { Direction: Direction.Right } when nextTile is '-' or '.':
              current = new Position(current.r, current.c + 1, Direction.Right);
              break;
              case { Direction: Direction.Down } when nextTile is '|' or '.':
              current = new Position(current.r + 1, current.c, Direction.Down);
              break;
              case { Direction: Direction.Up } when nextTile is '|' or '.':
              current = new Position(current.r - 1, current.c, Direction.Up);
              break;
              case { Direction: Direction.Right } when nextTile == '|':
              {
                if (!rays.Any(r => r.r == current.r && r.c == current.c + 1 && r.Direction == Direction.Down)) rays.Add(new Position(current.r, current.c + 1, Direction.Down));
                current = new Position(current.r, current.c + 1, Direction.Up);
                break;
              }
              case { Direction: Direction.Left } when nextTile == '|':
              {
                if (!rays.Any(r => r.r == current.r && r.c == current.c - 1 && r.Direction == Direction.Down)) rays.Add(new Position(current.r, current.c - 1, Direction.Down));
                current = new Position(current.r, current.c - 1, Direction.Up);
                break;
              }
              case { Direction: Direction.Down } when nextTile == '-':
              {
                if (!rays.Any(r => r.r == current.r + 1 && r.c == current.c && r.Direction == Direction.Right)) rays.Add(new Position(current.r + 1, current.c, Direction.Right));
                current = new Position(current.r + 1, current.c, Direction.Left);
                break;
              }
              case { Direction: Direction.Up } when nextTile == '-':
              {
                if (!rays.Any(r => r.r == current.r - 1 && r.c == current.c && r.Direction == Direction.Right)) rays.Add(new Position(current.r - 1, current.c, Direction.Right));
                current = new Position(current.r - 1, current.c, Direction.Left);
                break;
              }
            }
          }
        }

        var result = visited.Sum(l => l.Count(x => x == '#'));
        max = Math.Max(max, result);
      }
      Console.WriteLine(max); // 8026
    }

    private static bool IsInside(Position p, char[][] surface)
    {
      return -1 < p.r && p.r < surface.Length && -1 < p.c && p.c < surface[0].Length;
    }

    private static Position? GetNextPosition(Position p, IReadOnlyList<char[]> surface)
    {
      return p.Direction switch
      {
        Direction.Right => surface[0].Length <= p.c + 1 ? null : p with { c = p.c + 1 },
        Direction.Left => p.c - 1 < 0 ? null : p with { c = p.c - 1 },
        Direction.Down => surface.Count <= p.r + 1 ? null : p with { r = p.r + 1 },
        Direction.Up => p.r - 1 < 0 ? null : p with { r = p.r - 1 },
        _ => null
      };
    }

    private static void DrawSurface(char[][] surface)
    {
      foreach (var line in surface)
      {
        Console.WriteLine(string.Join("", line));
      }
      Console.WriteLine();
    }

    public record Position(int r, int c, Direction Direction);

    public enum Direction
    {
      Up,
      Down,
      Left,
      Right
    }
  }
}
