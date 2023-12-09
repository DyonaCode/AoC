using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;

namespace AoC
{
    public class Hand
    {
        public string HandCards { get; set; }
        public int Bid { get; set; }
    }
    public class HandComparer : Comparer<Hand>
    {
        public override int Compare(Hand x, Hand y)
        {
            int handStrengthComparison = CompareHandStrength(x.HandCards, y.HandCards);

            if (handStrengthComparison == 0)
            {
                // If hands are of equal rank, compare by bid
                return x.Bid.CompareTo(y.Bid);
            }

            return handStrengthComparison;
        }

        private int CompareHandStrength(string hand1, string hand2)
        {
            int hand1Rank = CalculateRank(hand1);
            int hand2Rank = CalculateRank(hand2);

            if (hand1Rank > hand2Rank)
            {
                return 1;
            }
            else if (hand1Rank < hand2Rank)
            {
                return -1;
            }

            // If ranks are equal, compare by card strength
            var valueDict = new Dictionary<char, int>()
            {
                {'2', 2 },
                {'3', 3 },
                {'4', 4 },
                {'5', 5 },
                {'6', 6 },
                {'7', 7 },
                {'8', 8 },
                {'9', 9 },
                {'T', 10 },
                {'J', 11 },
                {'Q', 12 },
                {'K', 13 },
                {'A', 14 },
            };

            for(int i = 0; i < hand1.Length; i++)
            {
                if(valueDict[hand1[i]] > valueDict[hand2[i]])
                {
                    return 1;
                }
                else if(valueDict[hand1[i]] < valueDict[hand2[i]])
                {
                    return -1;
                }
            }

            return 0; // Hands are equal
        }

        
        public int CalculateRank(string hand)
        {
            var dict = new Dictionary<char, int>();

            foreach(var character in hand)
            {
                if(dict.ContainsKey(character))
                {
                    dict[character] += 1;
                }
                else
                {
                    dict.Add(character, 1);
                }
            }
            int maxCount = dict.Values.Max();
            int distinctCount = dict.Keys.Count;

            // Determine the type of hand
            if (maxCount == 5) return 7; // Five of a kind
            if (maxCount == 4) return 6; // Four of a kind
            if (maxCount == 3 && distinctCount == 2) return 5; // Full house
            if (maxCount == 3) return 4; // Three of a kind
            if (maxCount == 2 && distinctCount == 3) return 3; // Two pair
            if (maxCount == 2) return 2; // One pair
            return 1; // High card
        }
   
    }

    public class HandComparer2 : Comparer<Hand>
    {
        public override int Compare(Hand x, Hand y)
        {
            int handStrengthComparison = CompareHandStrength(x.HandCards, y.HandCards);

            if (handStrengthComparison == 0)
            {
                // If hands are of equal rank, compare by bid
                return x.Bid.CompareTo(y.Bid);
            }

            return handStrengthComparison;
        }

        private int CompareHandStrength(string hand1, string hand2)
        {
            int hand1Rank = CalculateRank(hand1);
            int hand2Rank = CalculateRank(hand2);

            if (hand1Rank > hand2Rank)
            {
                return 1;
            }
            else if (hand1Rank < hand2Rank)
            {
                return -1;
            }

            // If ranks are equal, compare by card strength
            var valueDict = new Dictionary<char, int>()
            {
                {'J', 2 },
                {'2', 3 },
                {'3', 4 },
                {'4', 5 },
                {'5', 6 },
                {'6', 7 },
                {'7', 8 },
                {'8', 9 },
                {'9', 10 },
                {'T', 11 },
                {'Q', 12 },
                {'K', 13 },
                {'A', 14 },
            };

            for(int i = 0; i < hand1.Length; i++)
            {
                if(valueDict[hand1[i]] > valueDict[hand2[i]])
                {
                    return 1;
                }
                else if(valueDict[hand1[i]] < valueDict[hand2[i]])
                {
                    return -1;
                }
            }

            return 0; // Hands are equal
        }

        
        public int CalculateRank(string hand)
        {
            var dict = new Dictionary<char, int>();

            foreach(var character in hand)
            {
                if(dict.ContainsKey(character))
                {
                    dict[character] += 1;
                }
                else
                {
                    dict.Add(character, 1);
                }
            }

            if (dict.ContainsKey('J') && dict.Keys.Count != 1)
            {
                // Find the key with the maximum value that's not 'J'
                var maxKey = dict.Where(kvp => kvp.Key != 'J')
                                .OrderByDescending(kvp => kvp.Value)
                                .Select(kvp => kvp.Key)
                                .FirstOrDefault();

                // Add the value of 'J' to the max value key, if it exists
                if (maxKey != default(char))
                {
                    dict[maxKey] += dict['J'];
                }

                // Remove 'J' from the dictionary
                dict.Remove('J');
            }

            int maxCount = dict.Values.Max();
            int distinctCount = dict.Keys.Count;

            // Determine the type of hand
            if (maxCount == 5) return 7; // Five of a kind
            if (maxCount == 4) return 6; // Four of a kind
            if (maxCount == 3 && distinctCount == 2) return 5; // Full house
            if (maxCount == 3) return 4; // Three of a kind
            if (maxCount == 2 && distinctCount == 3) return 3; // Two pair
            if (maxCount == 2) return 2; // One pair
            return 1; // High card
        }
   
    }

    static class Day7
    {
        public static void Part1()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day7resource.txt");

            List<Hand> handCards = new List<Hand>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');

                string hand = line[0];
                int bid = int.Parse(line[1]);

                handCards.Add(new Hand { HandCards = hand, Bid = bid });
            }

            handCards.Sort(new HandComparer());
            int sum = 0;
            for(int i = 0; i < handCards.Count; i++)
            {
                sum += ((i + 1) * handCards[i].Bid); 
            }


            Console.WriteLine(sum);
        }

        public static void Part2()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day7resource.txt");

            List<Hand> handCards = new List<Hand>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');

                string hand = line[0];
                int bid = int.Parse(line[1]);

                handCards.Add(new Hand { HandCards = hand, Bid = bid });
            }

            handCards.Sort(new HandComparer2());
            int sum = 0;
            for(int i = 0; i < handCards.Count; i++)
            {
                sum += ((i + 1) * handCards[i].Bid);
            }


            Console.WriteLine(sum);
        }
    }

}
