using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day8
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day8.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var navigation = lines.First().ToCharArray();
      var nodes = lines.Skip(2).Select(x => x.Split(" = (")).Select( y => new Node(y[0], y[1].Split(", ")[0], y[1].Split(", ")[1].Trim(')')));

      var count = 0;
      var start = nodes.Single(x => x.Self == "AAA");
      while (start.Self != "ZZZ")
      {
        //Console.WriteLine($"Start: {start.Key} L: {start.Value.Left} R: {start.Value.Right} i={count % navigation.Length} instruction {navigation[count % navigation.Length]}"); // 254024898 
        start = navigation[count % navigation.Length] == 'L' ? nodes.Single(x => x.Self == start.Left) : nodes.Single(x => x.Self == start.Right);
        count++;
      }
      
      Console.WriteLine(count); // 21797 
    }

    private static void Part2(IEnumerable<string> lines)
    {
      var navigation = lines.First().ToCharArray();
      var nodes = lines.Skip(2).Select(x => x.Split(" = (")).Select(y => new Node(y[0], y[1].Split(", ")[0], y[1].Split(", ")[1].Trim(')')));

      var count = 0;
      var starts = nodes.Where(x => x.Self.EndsWith("A")).ToList();

      while (starts.Any(x => !x.Self.EndsWith("Z")))
      {
        var move = navigation[count % navigation.Length];
        foreach (var n in starts.Where(x => !x.Self.EndsWith("Z")))
        {
          //Console.WriteLine($"Start: {n.Self} L: {n.Left} R: {n.Right} i={count} instruction {move}");
          var next = navigation[count % navigation.Length] == 'L' ? nodes.Single(x => x.Self == n.Left) : nodes.Single(x => x.Self == n.Right);
          n.Update(next.Self, next.Left, next.Right);
        }
        count++;
      }

      var result = CalculateLCD(starts.Select(x => x.Path).ToArray());
      Console.WriteLine(result); // 23977527174353 
    }

    public class Node
    {
      private string _self;
      private string _left;
      private string _right;
      private int _path = 0;

      public string Self => _self;

      public string Left => _left;

      public string Right => _right;

      public long Path => _path;

      public Node(string self, string left, string right)
      {
        _self = self;
        _left = left;
        _right = right;
      }

      public void Update(string self, string left, string right)
      {
        _self = self;
        _left = left;
        _right = right;
        _path++;
      }
    }

    // Function to calculate GCD
    static long CalculateGCD(long a, long b)
    {
      while (b != 0)
      {
        var temp = b;
        b = a % b;
        a = temp;
      }
      return a;
    }

    // Function to calculate LCD for an array of numbers
    static double CalculateLCD(IReadOnlyList<long> numbers)
    {
      var result = numbers[0];
      for (var i = 1; i < numbers.Count; i++)
      {
        result = (result * numbers[i]) / CalculateGCD(result, numbers[i]);
      }
      return result;
    }
  }
}
