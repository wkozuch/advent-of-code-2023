using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
  class Day3
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day3.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(string[] lines)
    {
      var sum = 0;
      for (int i = 0; i < lines.Length; i++)
      {
        var previousLine = i != 0 ? lines[i - 1] : "";
        var line = lines[i];
        var nextLine = i + 1 < lines.Length ? lines[i + 1] : "";
        var numbers = FindNumbersWithPositions(line);

        foreach (Match info in numbers)
        {
          var number = int.Parse(info.Value);
          var index = info.Index;
          var length = info.Value.Length;

          var previous = FindSymbolsWithoutPeriod(Substring(previousLine, index, length)).Any();
          var current = FindSymbolsWithoutPeriod(Substring(line, index, length)).Any();
          var next = FindSymbolsWithoutPeriod(Substring(nextLine, index, length)).Any();
          var adjacentToSymbol = previous || current || next;
          if (adjacentToSymbol) sum += number;
          Console.WriteLine($"{number}: {adjacentToSymbol}");
        }
      }
      Console.WriteLine(sum); // 526404
    }

    private static void Part2(string[] lines)
    {
      var sum = 0;
      for (int i = 0; i < lines.Length; i++)
      {
        var previousLine = i != 0 ? lines[i - 1] : "";
        var line = lines[i];
        var nextLine = i + 1 < lines.Length ? lines[i + 1] : "";
        MatchCollection asterixis = FindAsterix(line);

        foreach (Match info in asterixis)
        {
          var value = info.Value;
          var index = info.Index;
          var previous = FindNumbersWithPositions(previousLine).Where(m => IsAdjecent(m, index)).Select(x => int.Parse(x.Value));
          var current = FindNumbersWithPositions(line).Where(n => n.Index + n.Length - 1 == index - 1 || n.Index == index + 1).Select(x => int.Parse(x.Value));
          var next = FindNumbersWithPositions(nextLine).Where(m => IsAdjecent(m, index)).Select(x => int.Parse(x.Value));
          var adjecentToNumbers = previous.Concat(current).Concat(next);
          if (adjecentToNumbers.Count() == 2)
          {
            sum += adjecentToNumbers.First() * adjecentToNumbers.Last();
            Console.WriteLine($"{value}: {string.Join(",", adjecentToNumbers)}");
          }
        }
      }
      Console.WriteLine(sum); // 84399773
    }

    static bool IsAdjecent(Match match, int index)
    {
      var start = match.Index;
      var end = match.Index + match.Length - 1;
      if (start == index - 1 || start == index || start == index + 1) return true;
      if (end == index - 1 || end == index || end == index + 1) return true;
      if (start < index - 1 && index + 1 < end) return true;
      return false;
    }

    static MatchCollection FindNumbersWithPositions(string text)
    {
      Regex regex = new Regex(@"\d+");
      return regex.Matches(text);
    }

    static MatchCollection FindSymbolsWithoutPeriod(string text)
    {
      Regex regex = new Regex(@"[^.\w\s]");
      return regex.Matches(text);
    }

    static MatchCollection FindAsterix(string text)
    {
      Regex regex = new Regex(@"\*");
      return regex.Matches(text);
    }

    static string Substring(string text, int index, int length)
    {
      if (text == "") return text;
      var start = index > 0 ? index - 1 : 0;
      length = index > 0 ? length + 2 : length + 1;
      int maxLength = Math.Min(length, text.Length - start);
      var s = text.Substring(start, maxLength);
      Console.WriteLine(s);
      return s;
    }
  }
}
