using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AoC
{
    static class Day2
    {
        public static void Part2()
        {
            //resource
            string resourcePath = @"/Users/jamesrogers/code/AoC/Resources/day2resource.txt";
            // create sum result int. 
            int sumResult = 0;
            int sumPart2Result = 0;

            // create dictionary<string, int> of colours and max possible
            var colourMaxs = new Dictionary<string, int>()
            {
                { "red", 12 },
                { "green", 13 },
                { "blue", 14 }
            };

            // set isPossible bool to true by default
            bool isPossible;

            // set itorator for each line 
            int gameID = 1;

            //loop through each line 
            using (var reader = new StreamReader(resourcePath))
            {
                string line;
                while ((line = reader.ReadLine()) is not null)
                {
                    isPossible = true;

                    var gamecolourMins = new Dictionary<string, int>(){
                        { "red", 0 },
                        { "green", 0 },
                        { "blue", 0 }
                    };

                    //split by colon to remove game ID 
                    string[] gameLine = line.Split(":");
  
                    // split by semi colon to get rounds
                    string[] game = gameLine[1].Split(";");
              
                    // loop through each game
                    foreach(var rounds in game)
                    {
                        if(!isPossible)
                        {
                            break;
                        }
                        // split by comma to get round colours.
                        string[] roundColours = rounds.Split(",");
          
                        // loop through round colours. 
                        foreach(var roundColour in roundColours)
                        {
                            if(!isPossible)
                            {
                                break;
                            }
                            //remove whitespace from start
                            string roundcolourTrimmed = roundColour.TrimStart();
                        
                            //split by space down the middle to get number and colour
                            string[] colourOutcome = roundcolourTrimmed.Split(" ");
                       
                            // e.g. colourOutcome[0] = 10 | colourOutcome[1] = "green"
                            //Get int from colourOutcome number
                            int colourNumber = int.Parse(colourOutcome[0]);
                                        
                            // colour is matched against dictionary with colours to max numbers
                            int colourMax = colourMaxs[colourOutcome[1]];
                            
                            // for part 2: set minimums
                            var currentColourMax = gamecolourMins[colourOutcome[1]];

                            if(currentColourMax < colourNumber || currentColourMax == 0)
                            {
                                gamecolourMins[colourOutcome[1]] = colourNumber;
                            }
                            /*
                            // match against colour max. 
                            if(colourNumber > colourMax)
                            {
                                // if number exceeds colour max, flag 'isPossible' as false
                                isPossible = false;
                                break;
                            }*/

                        }
                    }
                    // add the gameID to the sum if the game is possible. 
                    if(isPossible)
                    {
                        sumResult = sumResult + gameID;
                        int gamePowerResult = 1;
                        
                        foreach(var value in gamecolourMins.Values)
                        {
                            gamePowerResult *= value;
                        }

                        sumPart2Result = sumPart2Result + gamePowerResult;
                    }
                    // increment game ID
                    gameID++;
                }
            }
                Console.WriteLine(nameof(sumPart2Result));
                Console.WriteLine(sumPart2Result);
        } 
  }
}