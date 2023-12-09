using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Globalization;
using System.ComponentModel;

namespace AoC
{
    static class Day9
    {
        public static void Part1()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day9resource.txt");
            long result = 0;
            // loop through lines 
            foreach(var line in lines)
            {
                
                //create dictionary of lists. ID :  List<int>
                var dictList = new Dictionary<int, List<long>>();

                // split each line by space 
                // add all items to a list of ints
                List<long> baseList = line.Split(' ').Select(i => long.Parse(i)).ToList();

                //Add to Dict
                dictList.Add(0, baseList);
                
                int iterator = 0;
                // while loop
                while(true)
                {
                    // create new list.
                    List<long> tempList = new List<long>();

                    // Get previous List 
                    List<long> prevList = dictList[iterator];

                    //for each two elements in list, compare and add difference to new list.
                    for(int i = 0; i < prevList.Count - 1; i++)
                    {
                        long difference = prevList[i + 1] - prevList[i];
                        tempList.Add(difference);
                    }
                    iterator++;

                    //Add templist to dictionary
                    dictList.Add(iterator, tempList);     

                    // if list is all zeros break.
                    if(tempList.TrueForAll(num => num == 0))
                    {
                        break;
                    }
                }

                // for i  list in dictLists  
                // max list ID - 1 add value to the end with the value of the previous value + the last elemnent in the list above. 

                for(int i = dictList.Keys.Max(); i > 0; i--)
                {
                    long prevValue = dictList[i - 1].Last();
                    long aboveLastValue = dictList[i].Last();
                    dictList[i - 1].Add(prevValue + aboveLastValue);

                    // If list is base list, take newset added number and add to total. 
                    if(i - 1 == 0)
                    {
                        result += dictList[i - 1].Last();
                    }
                }
                
            }
            Console.WriteLine(result);
        }

           public static void Part2()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day9resource.txt");
            long result = 0;
            // loop through lines 
            foreach(var line in lines)
            {
                
                //create dictionary of lists. ID :  List<int>
                var dictList = new Dictionary<int, List<long>>();

                // split each line by space 
                // add all items to a list of ints
                List<long> baseList = line.Split(' ').Select(i => long.Parse(i)).ToList();

                //Add to Dict
                dictList.Add(0, baseList);
                
                int iterator = 0;
                // while loop
                while(true)
                {
                    // create new list.
                    List<long> tempList = new List<long>();

                    // Get previous List 
                    List<long> prevList = dictList[iterator];

                    //for each two elements in list, compare and add difference to new list.
                    for(int i = 0; i < prevList.Count - 1; i++)
                    {
                        long difference = prevList[i + 1] - prevList[i];
                        tempList.Add(difference);
                    }
                    iterator++;

                    //Add templist to dictionary
                    dictList.Add(iterator, tempList);     

                    // if list is all zeros break.
                    if(tempList.TrueForAll(num => num == 0))
                    {
                        break;
                    }
                }

                // for i  list in dictLists  
                // max list ID - 1 add value to the end with the value of the previous value + the last elemnent in the list above. 

                for(int i = dictList.Keys.Max(); i > 0; i--)
                {
                    long firstValue = dictList[i - 1].First();
                    long aboveLastValue = dictList[i].First();
                    dictList[i - 1].Insert(0, firstValue - aboveLastValue);

                    // If list is base list, take newset added number and add to total. 
                    if(i - 1 == 0)
                    {
                        result += dictList[i - 1].First();
                    }
                }
                
            }
            Console.WriteLine(result);
        }
    }
}
