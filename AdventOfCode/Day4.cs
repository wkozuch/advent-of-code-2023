using System;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day4
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day4.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(string[] lines)
    {
      var sum = 0.0;
      foreach (var line in lines)
      {
        var all = line.Split(": ").Last().Split(" | ");
        var winning = all[0].Split(new string[] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);
        var numbers = all[1].Split(new string[] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);

        var points = numbers.Intersect(winning).ToList();
        if (points.Count != 0) sum += Math.Pow(2.0, (double)points.Count - 1);
      }
      Console.WriteLine(sum); // 27454
    }

    private static void Part2(string[] lines)
    {
      var cards = Enumerable.Range(0, lines.Length).ToDictionary(x => x, x => 1);
      for (int i = 0; i < lines.Length; i++) // current card
      {
        var all = lines[i].Split(": ").Last().Split(" | ");
        var winning = all[0].Split(new string[] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);
        var numbers = all[1].Split(new string[] { " ", "  " }, StringSplitOptions.RemoveEmptyEntries);

        var points = numbers.Intersect(winning).ToList();
        for (int j = 0; j < cards[i]; j++) // how many with copies
        {
          for (int k = i + 1; k < i + points.Count + 1; k++)
          {
            cards[k]++;
          }
        }
      }
      Console.WriteLine(cards.Values.Sum()); // 6857330
    }
  }
}
