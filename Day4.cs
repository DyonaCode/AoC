using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


namespace AoC
{
    static class Day4
    {
        public static void Part1()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day4resource.txt");

            int sumPoints = 0;
            int lineNumber = 0;
            
            while(lineNumber < lines.Length)
            {
                int cardPoints = 0;
                string line = lines[lineNumber];
                //Get main data
                string[] data = line.Split(": ");

                string[] winVsYourNums = data[1].Split(" | ");

                string pattern = @"\d\d?";

                var matchesWin = Regex.Matches(winVsYourNums[0], pattern);
                
                var matchesYours = Regex.Matches(winVsYourNums[1], pattern);

                List<int> numsWin = new List<int>();
                List<int> numsYours = new List<int>();
                foreach (Match match in matchesWin)
                {
                    numsWin.Add(int.Parse(match.Value));
                }
                foreach (Match match in matchesYours)
                {
                    numsYours.Add(int.Parse(match.Value));
                }

                foreach(var yourNum in numsYours)
                {
                    if(numsWin.Contains(yourNum))
                    {
                        if(cardPoints != 0)
                        {
                            cardPoints *= 2;
                            
                        } 
                        else 
                        {
                            cardPoints = 1;
                        }
                    }
                }
                sumPoints += cardPoints;
                lineNumber++;
            }

            Console.WriteLine(sumPoints);
        }

        public static void Part2()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day4resource.txt");

            var cardValuesDict = new Dictionary<int, int>();
            int cardID = 1;
            foreach(string line in lines)
            {
                //Get cardID
                // string[] data = line.Split(": ");
                // var keyString = data[0].Substring(4).TrimStart();
                // int key = int.Parse(keyString);

                //Set up lists of both sets of Numbers 
                string[] data = line.Split(": ");
                string[] winVsYourNums = data[1].Split(" | ");
                string pattern = @"\d\d?";
                var matchesWin = Regex.Matches(winVsYourNums[0], pattern);
                var matchesYours = Regex.Matches(winVsYourNums[1], pattern);
                 List<int> numsWin = new List<int>();
                List<int> numsYours = new List<int>();
                foreach (Match match in matchesWin)
                {
                    numsWin.Add(int.Parse(match.Value));
                }
                foreach (Match match in matchesYours)
                {
                    numsYours.Add(int.Parse(match.Value));
                }

                //Perform analysis to get the amount of cards the current card has won 
                int cardPoints = 0;
                foreach(var yourNum in numsYours)
                {
                    if(numsWin.Contains(yourNum))
                    {
                      cardPoints++;
                    }
                }

                cardValuesDict[cardID] = cardPoints;
                cardID++;
            }

            var newCardsKVP = new List<KeyValuePair<int, int>>();

            foreach (var kvp in cardValuesDict)
            {
                newCardsKVP.Add(kvp);
            }

            int i = 0;
            while (i < newCardsKVP.Count)
            {
                var currentPair = newCardsKVP[i];
                int indexsOfCopies = currentPair.Value;

                for (int j = 1; j <= indexsOfCopies; j++)
                {
                    int nextKey = currentPair.Key + j;
                    if (cardValuesDict.ContainsKey(nextKey))
                    {
                        newCardsKVP.Add(new KeyValuePair<int, int>(nextKey, cardValuesDict[nextKey]));
                    }
                }

                i++;
            }

            int sumPoints = newCardsKVP.Count;
            Console.WriteLine(sumPoints);
        }
    }
}
