using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  class Day1
  {
    public static void Main(string[] args)
    {
      Dictionary<string, int> numbersDict = new Dictionary<string, int> {
          {"1", 1}, {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5},
          {"6", 6}, {"7", 7}, {"8", 8}, {"9", 9},
          {"one", 1}, {"two", 2}, {"three", 3}, {"four", 4}, {"five", 5},
          {"six", 6}, {"seven", 7}, {"eight", 8}, {"nine", 9}
        };

      var lines = File.ReadAllLines(@"Datasets\Day1.txt");
      //Part1(lines);
      Part2(lines, numbersDict);
    }

    private static void Part1(string[] lines)
    {
      var sum = 0;
      foreach (var line in lines)
      {
        var code = line.First(x => int.TryParse(x.ToString(), out _)).ToString() + line.Last(x => int.TryParse(x.ToString(), out _)).ToString();
        sum += int.Parse(code);

      }
      Console.WriteLine($"Part one: {sum}");
    }

    private static void Part2(string[] lines, Dictionary<string, int> numbers)
    {
      var sum = 0;

      foreach (var line in lines)
      {
        var numberAndPosition = new Dictionary<int, string>();

        foreach (var n in numbers.Keys)
        {
          var ixs = FindIndexes(line, n);
          foreach(var i in ixs)
          {
            numberAndPosition.Add(i, numbers[n].ToString());
          }

        }

        var num = numberAndPosition.OrderBy(x => x.Key).Select(x => x.Value).ToList();
        var code = num.First() + num.Last();
        Console.WriteLine($"Line {line}:  { code }");

        sum += int.Parse(code);
      }
      Console.WriteLine($"Part two: {sum}");
    }

    public static IEnumerable<int> FindIndexes(string source, string matchString)
    {
      matchString = Regex.Escape(matchString);
      foreach (Match match in Regex.Matches(source, matchString))
      {
        yield return match.Index;
      }
    }
  }
}
