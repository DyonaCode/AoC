using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AoC
{
    static class Day8
    {
        public static void Part1()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day8resource.txt");

            // Create direction map
            var LRdict = new Dictionary<char, int>()
            {
                {'L', 0 },
                {'R', 1 }
            };
             
            // Create Node Dictionary 
            var nodeDict = new Dictionary<string, Node>();
            for(int i = 2; i < lines.Count(); i++)
            {
                string line = lines[i];

                // Create a new Node instance
                Node node = new Node
                {
                    NodeCode = line.Substring(0,3),
                    LeftNextNode = line.Substring(7, 3),
                    RightNextNode = line.Substring(12, 3)
                };

                // Add node to dictionary
                nodeDict.Add(node.NodeCode, node);
            }

            // Set current node code
            string currentNodeCode = "AAA";
            int result = 0;
            string directions = lines[0];

            // Loop through directions
            for(int i = 0; i < directions.Length; i++)
            {
                char direction = directions[i];
                int leftRightNumber = LRdict[direction];
                Node currentNode = nodeDict[currentNodeCode];

                // Determine next node based on direction
                string nextNodeCode = leftRightNumber == 0 ? currentNode.LeftNextNode : currentNode.RightNextNode;
                
                // Update current node code
                currentNodeCode = nextNodeCode;

                // Check for end condition
                if(currentNodeCode == "ZZZ")
                {
                    result++;
                    break;
                }

                // Reset if at the end of directions
                if(i == directions.Length - 1)
                {
                    i = -1;
                }

                result++;
            }

            Console.WriteLine(result);
        }


    public static void Part2()
    {
        string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day8resource.txt");
        var nodeDict = ParseNodes(lines.Skip(2));

        var stepsToZ = nodeDict.Keys
            .Where(key => key.EndsWith('A'))
            .Select(key => CalculateStepsToZ(nodeDict, key, lines[0]))
            .ToList();

        BigInteger lcm = stepsToZ.Aggregate(BigInteger.One, (acc, val) => LCM(acc, val));

        Console.WriteLine($"Steps for all paths to end with 'Z': {lcm}");
    }

    private static Dictionary<string, Node> ParseNodes(IEnumerable<string> lines)
    {
        var nodeDict = new Dictionary<string, Node>();
        foreach (string line in lines)
        {
            string[] parts = line.Split(new[] { " = (", ", ", ")" }, StringSplitOptions.RemoveEmptyEntries);
            nodeDict[parts[0]] = new Node
            {
                NodeCode = parts[0],
                LeftNextNode = parts[1],
                RightNextNode = parts[2]
            };
        }
        return nodeDict;
    }

    private static BigInteger CalculateStepsToZ(Dictionary<string, Node> nodeDict, string startNode, string directions)
    {
        var current = startNode;
        BigInteger steps = 0;

        while (!current.EndsWith('Z'))
        {
            int directionIndex = directions[(int)(steps % directions.Length)] == 'L' ? 0 : 1;
            current = directionIndex == 0 ? nodeDict[current].LeftNextNode : nodeDict[current].RightNextNode;
            steps++;
        }

        return steps;
    }

    private static BigInteger LCM(BigInteger a, BigInteger b)
    {
        return a / GCD(a, b) * b;
    }

    private static BigInteger GCD(BigInteger a, BigInteger b)
    {
        while (b != 0)
        {
            BigInteger temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}

    public class Node
    {
        public string NodeCode { get; set; }
        public string LeftNextNode { get; set; }
        public string RightNextNode { get; set; }
    }

}
