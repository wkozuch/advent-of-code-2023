using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
  class Day7
  {
    public static void Main(string[] args)
    {
      var lines = File.ReadAllLines(@"Datasets\Day7.txt");
      Part1(lines);
      Part2(lines);
    }

    private static void Part1(IEnumerable<string> lines)
    {
      var hands = lines.Select(x => x.Split(" ")).Select(y => new Hand(y[0], long.Parse(y[1])));
      var ordered = hands.OrderBy(x => x.Type).ThenBy(x => x.Card1).ThenBy(x => x.Card2).ThenBy(x => x.Card3).ThenBy(x => x.Card4).ThenBy(x => x.Card5);
      double winnings = 0;
      long index = 1;
      foreach (var hand in ordered)
      {
        Console.WriteLine($"#{index} {hand}");
        winnings += index++ * hand.Bid;
      }

      Console.WriteLine(winnings); // 254024898 
    }

    private static void Part2(IEnumerable<string> lines)
    {
      var hands = lines.Select(x => x.Split(" ")).Select(y => new Hand(y[0], long.Parse(y[1]), true));
      var ordered = hands.OrderBy(x => x.Type).ThenBy(x => x.Card1).ThenBy(x => x.Card2).ThenBy(x => x.Card3).ThenBy(x => x.Card4).ThenBy(x => x.Card5);
      double winnings = 0;
      long index = 1;
      foreach (var hand in ordered)
      {
        Console.WriteLine($"#{index} {hand}");
        winnings += index++ * hand.Bid;
      }
      Console.WriteLine(winnings); // 254115617
    }

    public class Hand
    {
      private readonly Dictionary<char, int> _cardValues = new()
      {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 }
      };

      public long Bid { get; }
      private readonly string _hand;
      private readonly bool _considerJokers;

      public Hand(string hand, long bid, bool considerJokers = false)
      {
        Bid = bid;
        _hand = hand;
        _considerJokers = considerJokers;
        if (considerJokers) _cardValues['J'] = 0;
      }

      public int Type => _considerJokers ? GetHandTypeWithJoker_() : GetHandType_();

      public int Card1 => _cardValues[_hand[0]];
      public int Card2 => _cardValues[_hand[1]];
      public int Card3 => _cardValues[_hand[2]];
      public int Card4 => _cardValues[_hand[3]];
      public int Card5 => _cardValues[_hand[4]];

      private int GetHandType_()
      {
        var cards = _hand.ToCharArray();
        var uniqueCount = cards.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        return cards.Distinct().Count() switch
        {
          1 => 7,// five of a kind
          2 => uniqueCount.Max(x => x.Value) == 4 ? 6 : 5,// four of a kind or full house // 
          3 => uniqueCount.Max(x => x.Value) == 3 ? 4 : 3,// three of a kind or two pairs
          4 => 2,// one pair
          5 => 1,
          _ => 0,
        };
      }

      private int GetHandTypeWithJoker_()
      {
        var cards = _hand.ToCharArray();
        if (!cards.Contains('J')) return GetHandType_();

        var uniqueCount = cards.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        var jokerCount = uniqueCount['J'];
        return (jokerCount, uniqueCount.Distinct().Count()) switch
        {
          (5, _) => 7, // five of kind 
          (4, 2) => 7, // five of kind
          (3, 3) => 6, // four of a kind
          (3, 2) => 7, // five of kind 
          (2, 4) => 4, // three of a kind
          (2, 3) => 6, // four of a kind
          (2, 2) => 7, // five of kind 
          (1, 5) => 2, // one pair
          (1, 4) => 4, // three of a kind
          (1, 3) => uniqueCount.Max(x => x.Value) == 3 ? 6 : 5, // four of a kind or full house // 
          (1, 2) => 7, // five of kind 
          _ => throw new ArgumentException(),
        };
      }

      public override string ToString()
      {
        return $"{_hand} {Type} {Bid}";
      }
    }
  }
}
