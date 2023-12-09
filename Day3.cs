using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using System.Text;

namespace AoC
{
    static class Day3
    {
        public static void Part1()
        {
            
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/day3resource.txt");

            // set savenumber bool
            bool saveNumber;

            // set list of numbers
            List<int> listOfNumbers = new List<int>();

            //char[] symbols = new char[] { '&', '*', '%', '$', '£', '@', '+', '-', '_', '(', ')', '!', '"', '^', '=', '|', '#'};

            int lineNumber = 0;
            
            // loop through each line 
            while(lineNumber <= lines.Length - 1 )
            {
                string line = lines[lineNumber];
                //Loop through each char in line 
                for(int i = 0; i < line.Length; i++)
                {
                    saveNumber = false;
                    char character = line[i];
                    int StartWindowIndex = 0;
                    int EndWindowIndex = 0;
                    var number = new StringBuilder();

                    // if char is digit, start analysis of number
                    if(char.IsDigit(character))
                    {
                        
                        StartWindowIndex = i != 0 ? i - 1 : i;


                        // keep iterating until it hits something not digit. 
                        int j = i;
                        while(j < line.Length && char.IsDigit(line[j]))
                        {
                            char testNumber = line[j];
                            number.Append(line[j]);
                            j++;
                        }
                        
                        // set EndWindowIndex to current position. 
                        EndWindowIndex = j < line.Length ? j : j - 1;
                     



                        // iterate through top row of window and check for  special char
                        int top = StartWindowIndex;
                    
                        
                        while(top <= EndWindowIndex && top < line.Length && !saveNumber && lineNumber != 0)
                        {
                          
                            string prevLine = lines[lineNumber - 1];
                            // If special character reached then save number and break
                            if(prevLine[top] != '.' && !char.IsDigit(prevLine[top]))
                            {
                                saveNumber = true;
                            }
                            top++;
                        }
                        // left of first digit encountered
                        if(line[StartWindowIndex] != '.' && !char.IsDigit(line[StartWindowIndex]) && !saveNumber)
                        {
                            saveNumber = true;
                        }

                        // right of last digit encountered
                        if(line[EndWindowIndex] != '.' && !char.IsDigit(line[EndWindowIndex]) && !saveNumber)
                        {
                            saveNumber = true;
                        }

                        // iterate through botttom row of window and check for  special char
                        int bot = StartWindowIndex;
                        
                        while(lineNumber < lines.Length - 1 && !saveNumber && bot <= EndWindowIndex)
                        {
                            string nextLine = lines[lineNumber + 1];
                            if(nextLine[bot] != '.' && !char.IsDigit(nextLine[bot]))
                            {
                                saveNumber = true;
                            }
                            bot++;
                        }

                           // if savenumber is true
                        if(saveNumber)
                        {
                            // add number to list of ints 
                            string numberString = number.ToString();
                            int savableNumber = int.Parse(numberString);
                            listOfNumbers.Add(savableNumber);
                            i = EndWindowIndex;
                        }
                    }
                }
                
                lineNumber++;
            }
            int sum = 0;
            // sum list of ints together. 
            foreach(var num in listOfNumbers)
            {
                sum += num;
            }

            Console.WriteLine(sum);
        }

        public static void Part2()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day3resource.txt");

            // set list of numbers
            List<int> listOfNumbers = new List<int>();

            int lineNumber = 0;
            
            // loop through each line 
            while(lineNumber <= lines.Length - 1 )
            {
                string line = lines[lineNumber];

                //Loop through each char in line 
                for(int i = 0; i < line.Length; i++)
                {
                    char character = line[i];

                    // if char is digit, start analysis of number
                    if(character == '*')
                    {
                        var surroundingNumbers = GetSurroundingNumbers(lines, lineNumber, i);
                        if(surroundingNumbers.Count >=2)
                        {
                            int gearRatio = 1;
                            foreach(var num in surroundingNumbers)
                            {
                                gearRatio *= num;
                            }
                            listOfNumbers.Add(gearRatio);
                        }
                    }
                }
                lineNumber++;
            }
            int sum = 0;
            // sum list of ints together. 
            foreach(var num in listOfNumbers)
            {
                sum += num;
            }
            Console.WriteLine(sum);
        }

        // Part of part 2 
        private static List<int> GetSurroundingNumbers(string[] text, int row, int col)
        {
            List<int> numbers = new List<int>();
            for (int i = row - 1; i <= row + 1; i++)
            {
                if (i >= 0 && i < text.Length)
                {
                    // get consecutive numbers from each line
                    var matches = Regex.Matches(text[i], @"\d+");
                    foreach (Match match in matches)
                    {
                        // Check if the number is adjacent to the '*'
                        int start = match.Index;
                        int end = match.Index + match.Length - 1;
                        if (IsAdjacent(col, start, end))
                        {
                            numbers.Add(int.Parse(match.Value));
                        }
                    }
                }
            }
            
            return numbers;
        }
        private static bool IsAdjacent(int starIndex, int start, int end)
        {
            // Check the number is adjacent to the  index of the *
            bool isAdjacent = (start <= starIndex + 1 && end >= starIndex - 1);
            return isAdjacent;
        }
    }
}
