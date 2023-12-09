using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace AoC
{
    static class Day1
    {
        public static void Part2()
        {
                var dict = new Dictionary<string, string>(){
                 {  "one", "1" },
                   { "two", "2"},
                   { "three", "3"},
                   { "four", "4"},
                   { "five", "5"},
                   { "six", "6"},
                   { "seven", "7"},
                   { "eight", "8"},
                   { "nine", "9"},
                   { "ten", "10"}
                };

                string resourcePath = @"/Users/jamesrogers/code/AoC/Resources/day1resource.txt";
                int result = 0;
                using (var reader = new StreamReader(resourcePath))
                {
                    string line;
                    bool shouldBreak = false;
                    while ((line = reader.ReadLine()) is not null)
                    {
                        int startIndex = 0;
                        int endIndex = line.Length - 1 ;

                        string leftNumber = "0";
                        string rightNumber = "0";

                        // Find first number
                        while(startIndex <= line.Length - 1 && !shouldBreak)
                        {

                            if(Char.IsDigit(line[startIndex]))
                            {
                                leftNumber = line[startIndex].ToString();
                                shouldBreak = true; 
                                break;
                            }
                            //build substring at the start
                            string startString = "";
                            for(int i = 0; i <= startIndex; i++)
                            {
                                startString += line[i];
                            }
                            if(startString.Length >= 3)
                            {
                                //check if substrin contains digit
                                foreach(var digit in dict.Keys)
                                {
                                    if(startString.Contains(digit))
                                    {
                                        leftNumber = dict[digit].ToString();
                                        shouldBreak = true; 
                                        break;
                                    }
                                }
                            }
                          
                          
                            startIndex++;
                        }
                        shouldBreak = false;

                        
                        //find second number
                        while(endIndex >= 0 && !shouldBreak)
                        {
                            var test = line[endIndex];
                            if(Char.IsDigit(line[endIndex]))
                            {
                                rightNumber = line[endIndex].ToString();
                                shouldBreak = true; 
                                break;
                            }

                            //build substring at the start
                            string endString = "";
                            for(int i = endIndex; i <= line.Length -1; i++)
                            {
                                endString += line[i];
                            }

                             if(endString.Length >= 3)
                            {
                                //check if substrin contains digit
                                foreach(var digit in dict.Keys)
                                {
                                    if(endString.Contains(digit))
                                    {
                                        rightNumber = dict[digit].ToString();
                                        shouldBreak = true; 
                                        break;
                                    }
                                }
                            }

                            endIndex--;
                        }
                        shouldBreak = false;

                        string concatenatedNumber = leftNumber + rightNumber;

                        int lineResult = int.Parse(concatenatedNumber);
                        Console.WriteLine(lineResult);
                        
                        result = result + lineResult;
                    }
                }
                Console.WriteLine(result);
        }
    }
}
