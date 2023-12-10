using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day10
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day10.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
      var pipes = lines.SelectMany((x, i) => x.Select((y, j) => new Position(y, i, j))).ToList();
      DrawSurface(surface);
      var start = pipes.Single(p => p.Pipe == 'S');
      Console.WriteLine($"{start.Pipe}: [{start.Col},{start.Row}]");
      Position next = new Position('*', 0, 0);
      var current = FindStartPipe(start, surface);
      var count = 0;
      while (current.Pipe != '*')
      {
        next = GetNextPosition(current, surface);
        count++;
        //DrawSurface(surface);
        surface[current.Row][current.Col] = '*';
        current = next;
      }
      Console.WriteLine($"Count: {count / 2}"); // 6956
    }

    private static void Part2(IEnumerable<string> lines)
    {
      var surface = lines.Select(x => x.Select(y => y).ToArray()).ToArray();
      var pipes = lines.SelectMany((x, i) => x.Select((y, j) => new Position(y, i, j))).ToList();
      DrawSurface(surface);
      var start = pipes.Single(p => p.Pipe == 'S');
      Console.WriteLine($"{start.Pipe}: [{start.Col},{start.Row}]");
      Position next = new Position('*', 0, 0);
      var current = FindStartPipe(start, surface);
      var inside = Enumerable.Range(0, lines.Count()).Select(x => Enumerable.Range(0, lines.First().Length).Select(y => '.').ToArray()).ToArray();
      while (current.Pipe != '*')
      {
        next = GetNextPositionWithDirectionPainting(current, surface, inside);
        //Console.Clear();
        //DrawSurface(inside);
        surface[current.Row][current.Col] = '*';
        inside[current.Row][current.Col] = '*';
        current = next;
      }
      Console.Clear();
      DrawSurface(inside);
      var count = inside.Sum(x => x.Count(x => x == 'I'));
      Console.WriteLine($"Count: {count}"); // 455  (278 + 277 manually counted) 

    }

    private static Position FindStartPipe(Position position, char[][] surface)
    {
      var startPipes = new List<char>() { '|', '-', 'F', 'L', '7', 'J' };
      var connected = new List<Position>();
      var top = position.Row - 1 < 0 ? '.' : surface[position.Row - 1][position.Col];
      var bottom = position.Row + 1 > surface.Length ? '.' : surface[position.Row + 1][position.Col];
      var left = position.Col - 1 < 0 ? '.' : surface[position.Row][position.Col - 1];
      var right = position.Col + 1 > surface[0].Length ? '.' : surface[position.Row][position.Col + 1];
      if (top == '|' || top == '7' || top == 'F')
      {
        connected.Add(new Position(top, position.Row - 1, position.Col));
        startPipes.RemoveAll(c => !new[] { '|', 'L', 'J' }.Contains(c));
      }
      if (left == '-' || left == 'F' || left == 'L')
      {
        connected.Add(new Position(left, position.Row, position.Col - 1));
        startPipes.RemoveAll(c => !new[] { '-', '7', 'J' }.Contains(c));
      }
      if (right == '-' || right == '7' || right == 'J')
      {
        connected.Add(new Position(right, position.Row, position.Col + 1));
        startPipes.RemoveAll(c => !new[] { '-', 'F', 'L' }.Contains(c));
      }
      if (bottom == '|' || bottom == 'L' || bottom == 'J')
      {
        connected.Add(new Position(bottom, position.Row + 1, position.Col));
        startPipes.RemoveAll(c => !new[] { '|', '7', 'F' }.Contains(c));
      }
      var start = startPipes.Single();
      surface[position.Row][position.Col] = start;
      return new Position(start, position.Row, position.Col);
    }

    private static Position GetNextPosition(Position current, char[][] surface)
    {
      if (current.Pipe == '|') return surface[current.Row + 1][current.Col] != '*' ? new Position(surface[current.Row + 1][current.Col], current.Row + 1, current.Col) : new Position(surface[current.Row - 1][current.Col], current.Row - 1, current.Col);
      if (current.Pipe == '-') return surface[current.Row][current.Col + 1] != '*' ? new Position(surface[current.Row][current.Col + 1], current.Row, current.Col + 1) : new Position(surface[current.Row][current.Col - 1], current.Row, current.Col - 1);
      if (current.Pipe == 'L') return surface[current.Row - 1][current.Col] != '*' ? new Position(surface[current.Row - 1][current.Col], current.Row - 1, current.Col) : new Position(surface[current.Row][current.Col + 1], current.Row, current.Col + 1);
      if (current.Pipe == 'J') return surface[current.Row - 1][current.Col] != '*' ? new Position(surface[current.Row - 1][current.Col], current.Row - 1, current.Col) : new Position(surface[current.Row][current.Col - 1], current.Row, current.Col - 1);
      if (current.Pipe == '7') return surface[current.Row][current.Col - 1] != '*' ? new Position(surface[current.Row][current.Col - 1], current.Row, current.Col - 1) : new Position(surface[current.Row + 1][current.Col], current.Row + 1, current.Col);
      if (current.Pipe == 'F') return surface[current.Row][current.Col + 1] != '*' ? new Position(surface[current.Row][current.Col + 1], current.Row, current.Col + 1) : new Position(surface[current.Row + 1][current.Col], current.Row + 1, current.Col);

      return current;
    }

    private static Position GetNextPositionWithDirectionPainting(Position current, char[][] surface, char[][] visited)
    {
      // always left
      if (current.Pipe == '|')
      {
        var bottom = surface[current.Row + 1][current.Col];
        if (bottom == '*')
        {
          // going up => mark left 
          MarkLeft(current, visited);
          return new Position(surface[current.Row - 1][current.Col], current.Row - 1, current.Col);

        }
        // going down = right is on the left
        MarkRight(current, visited);
        return new Position(surface[current.Row + 1][current.Col], current.Row + 1, current.Col);
      }

      if (current.Pipe == '-')
      {
        var right = surface[current.Row][current.Col + 1];
        if (right == '*')
        {
          // going left => bottom is on the left
          MarkBottom(current, visited);
          return new Position(surface[current.Row][current.Col - 1], current.Row, current.Col - 1);

        }
        // going right => top is on the left
        MarkTop(current, visited);
        return new Position(surface[current.Row][current.Col + 1], current.Row, current.Col + 1);
      }

      if (current.Pipe == 'L')
      {
        var top = surface[current.Row - 1][current.Col];
        if (top == '*')
        {
          // going left => mark right
          MarkTop(current, visited);
          MarkTopRight(current, visited);
          MarkRight(current, visited);
          return new Position(surface[current.Row][current.Col + 1], current.Row, current.Col + 1);
        }
        // going top => bottom is left
        MarkLeft(current, visited);
        MarkBottomLeft(current, visited);
        MarkBottom(current, visited);

        return new Position(surface[current.Row - 1][current.Col], current.Row - 1, current.Col);
      }

      if (current.Pipe == 'J')
      {
        var top = surface[current.Row - 1][current.Col];
        if (top == '*')
        {
          // going left
          MarkBottom(current, visited);
          MarkBottomRight(current, visited);
          MarkRight(current, visited);
          return new Position(surface[current.Row][current.Col - 1], current.Row, current.Col - 1);
        }

        MarkLeft(current, visited);
        MarkTopLeft(current, visited);
        MarkTop(current, visited);
        return new Position(surface[current.Row - 1][current.Col], current.Row - 1, current.Col);
      }

      if (current.Pipe == '7')
      {
        var left = surface[current.Row][current.Col - 1];
        var bottom = surface[current.Row + 1][current.Col];
        if (left == '*')
        {
          // going down => top is left
          MarkTop(current, visited);
          MarkTopLeft(current, visited);
          MarkLeft(current, visited);
          return new Position(surface[current.Row + 1][current.Col], current.Row + 1, current.Col);
        }
        //going left => top is left
        MarkLeft(current, visited);
        MarkBottomLeft(current, visited);
        MarkBottom(current, visited);
        return new Position(surface[current.Row][current.Col - 1], current.Row, current.Col - 1);
      }

      if (current.Pipe == 'F')
      {
        var bottom = surface[current.Row + 1][current.Col];
        var right = surface[current.Row][current.Col + 1];
        if (right == '*')
        {
          MarkRight(current, visited);
          MarkBottomRight(current, visited);
          MarkBottom(current, visited);
          return new Position(surface[current.Row + 1][current.Col], current.Row + 1, current.Col);
        }
        MarkTop(current, visited);
        MarkTopLeft(current, visited);
        MarkLeft(current, visited);
        return new Position(surface[current.Row][current.Col + 1], current.Row, current.Col + 1);

      }

      return current;
    }

    private static void MarkTop(Position current, char[][] surface)
    {
      if (current.Row - 1 > 0 && surface[current.Row - 1][current.Col] == '.') surface[current.Row - 1][current.Col] = 'I';
    }

    private static void MarkBottom(Position current, char[][] surface)
    {
      if (current.Row + 1 < surface.Length && surface[current.Row + 1][current.Col] == '.') surface[current.Row + 1][current.Col] = 'I';
    }

    private static void MarkRight(Position current, char[][] surface)
    {
      if (current.Col + 1 < surface[0].Length && surface[current.Row][current.Col + 1] == '.') surface[current.Row][current.Col + 1] = 'I';
    }

    private static void MarkLeft(Position current, char[][] surface)
    {
      if (current.Col - 1 > 0 && surface[current.Row][current.Col - 1] == '.') surface[current.Row][current.Col - 1] = 'I';
    }

    private static void MarkTopLeft(Position current, char[][] surface)
    {
      if (current.Col - 1 > 0 && current.Row - 1 > 0 && surface[current.Row - 1][current.Col - 1] == '.') surface[current.Row - 1][current.Col - 1] = 'I';
    }

    private static void MarkTopRight(Position current, char[][] surface)
    {
      if (current.Col + 1 < surface[0].Length && current.Row - 1 > 0 && surface[current.Row - 1][current.Col + 1] == '.') surface[current.Row - 1][current.Col + 1] = 'I';
    }

    private static void MarkBottomLeft(Position current, char[][] surface)
    {
      if (current.Col - 1 > 0 && current.Row + 1 < surface.Length && surface[current.Row + 1][current.Col - 1] == '.') surface[current.Row + 1][current.Col - 1] = 'I';
    }

    private static void MarkBottomRight(Position current, char[][] surface)
    {
      if (current.Col + 1 < surface[0].Length && current.Row + 1 < surface.Length && surface[current.Row + 1][current.Col + 1] == '.') surface[current.Row + 1][current.Col + 1] = 'I';
    }

    private static void DrawSurface(char[][] surface)
    {
      foreach (var line in surface)
      {
        Console.WriteLine(string.Join("", line));
      }
      Console.WriteLine();
    }

    public record Position(char Pipe, int Row, int Col);
  }
}
