using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


namespace AoC
{
    static class Day5
    {
        public static void Part1OLD()
        {
            //get lines 
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day5resource.txt");
            
            // store seeds in list.
            List<long> seeds = new List<long>();
            string[] seedLine = lines[0].Split(":");

            foreach(var seed in seedLine[1].Trim().Split(' '))
            {
                seeds.Add(long.Parse(seed));
            }


            // ================ Create Dictionary of maps =================
            var maps = new Dictionary<string, Dictionary<string, Dictionary<long, long>>>();


            //Main Loop + iterator 
            int lineI = 1;
            while(lineI < lines.Count())
            {
                string line = lines[lineI];
                string mapNames = string.Empty;

                // create dictionary to map names
                var tempMapNames = new Dictionary<string, Dictionary<long, long>>();

                // Inner loop to create map
                if(line.Contains("map"))
                {
                    mapNames = line.Split(" map:")[0];
                    string mapNameSource = mapNames.Split("-to-")[0];
                    string mapNameDest = mapNames.Split("-to-")[1];

                    // create dictionary to map stuff
                    var tempMap = new Dictionary<long, long>();

                    lineI++;
                    while(lineI < lines.Count() && !string.IsNullOrEmpty(line))
                    {
                        line = lines[lineI];
                        //Parse line
                        string[] mapSettings = line.Split(' ');
                        long dest = long.Parse(mapSettings[0]);
                        long source = long.Parse(mapSettings[1]);
                        long range = long.Parse(mapSettings[2]);

                        // Add mappings to dictionary
                        for(long i = 0; i < range; i++)
                        {
                            tempMap[source + i] = dest + i; 
                        }
                        lineI++;
                        if(lineI < lines.Count())
                        {
                            line = lines[lineI];
                        }
                    }
                    //Make dictionaries and add to encompassing dictionary. 
                    tempMapNames.Add(mapNameDest, tempMap);
                    maps.Add(mapNameSource, tempMapNames);
                }
                lineI++;
            }
            List<long> seedDests = new List<long>();

            foreach(long seed in seeds)
            {
                string source = "seed";
                string destination = string.Empty;
                long sourceValue = seed;

                for(long i = 0; i < maps.Count; i++)
                {
                    if(maps.ContainsKey(source))
                    {
                        //get new destination Name
                        destination = maps[source].Keys.FirstOrDefault();

                        //get inner dictionary Using the Current source and destination
                        var innerDict = maps[source][destination];

                        if (innerDict.ContainsKey(sourceValue))
                        {
                            // get destination value of seed. 
                            sourceValue = innerDict[sourceValue];
                        }
                    }
                   source = destination;
                }
                seedDests.Add(sourceValue);
            }
            long minSeed = seedDests[0];

            foreach (long num in seedDests)
            {
                if (num < minSeed)
                {
                    minSeed = num; // Update minSeed if current number is smaller
                }
            }
            Console.WriteLine(minSeed);
        }

        public static void Part1()
        {
            // Read lines from the file
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day5resource.txt");

            // Parse seed values
            List<long> seeds = lines[0].Split(":")[1].Trim().Split(' ').Select(long.Parse).ToList();

            // Create a dictionary to hold range mappings
            var rangeMaps = new Dictionary<string, Dictionary<string, List<(long lowEnd, long highEnd, long offset)>>>();

            // Iterate over lines to populate rangeMaps
            int lineIndex = 1;
            while (lineIndex < lines.Length)
            {
                string line = lines[lineIndex];
                if (line.Contains("map"))
                {
                    string[] parts = line.Split(" map:");
                    string sourceMap = parts[0].Split("-to-")[0];
                    string destMap = parts[0].Split("-to-")[1];

                    var mapEntries = new List<(long lowEnd, long highEnd, long offset)>();
                    lineIndex++;

                    while (lineIndex < lines.Length && !string.IsNullOrWhiteSpace(lines[lineIndex]))
                    {
                        var settings = lines[lineIndex].Split(' ')
                            .Select(long.Parse).ToArray();
                        long dest = settings[0];
                        long source = settings[1];
                        long range = settings[2];

                        mapEntries.Add((source, source + range - 1, dest - source));
                        lineIndex++;
                    }

                    if (!rangeMaps.ContainsKey(sourceMap))
                    {
                        rangeMaps[sourceMap] = new Dictionary<string, List<(long lowEnd, long highEnd, long offset)>>();
                    }

                    rangeMaps[sourceMap][destMap] = mapEntries;
                }

                lineIndex++;
            }

            // Process seeds through the maps
            List<long> seedDests = seeds
                .Select(seed => GetSeedDests(seed, rangeMaps))
                .ToList();

            // Find the minimum value
            long minSeed = seedDests.Min();

            Console.WriteLine(minSeed);
        }

        private static long GetSeedDests(
            long seed,
            Dictionary<string, Dictionary<string, List<(long lowEnd, long highEnd, long offset)>>> rangeMaps)
        {
            string currentMap = "seed";
            while (rangeMaps.ContainsKey(currentMap))
            {
                string nextMap = rangeMaps[currentMap].Keys.First();
                foreach (var (lowEnd, highEnd, offset) in rangeMaps[currentMap][nextMap])
                {
                    if (lowEnd <= seed && seed <= highEnd)
                    {
                        seed += offset;
                        break;
                    }
                }

                currentMap = nextMap;
            }

            return seed;
        }


  public static void Part2()
{
    // Read lines from the file
    string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day5test.txt");

    // Parse seed ranges
    string[] seedRangeStrings = lines[0]
        .Split(":")[1]
        .Trim()
        .Split(' ');

    List<(long start, long end)> seedRanges = new List<(long start, long end)>();
    for (int i = 0; i < seedRangeStrings.Length; i += 2)
    {
        long start = long.Parse(seedRangeStrings[i]);
        long length = long.Parse(seedRangeStrings[i + 1]);
        long end = start + length - 1;
        seedRanges.Add((start, end));
    }

    // Create a dictionary to hold range mappings
    var rangeMaps = new Dictionary<string, Dictionary<string, List<(long lowEnd, long highEnd, long offset)>>>();

    // Loop to get Mapping ranges
    int lineIndex = 1;
    while (lineIndex < lines.Length)
    {
        string line = lines[lineIndex];
        
        if (line.Contains("map"))
        {
            string[] parts = line.Split(" map:");
            string sourceMap = parts[0].Split("-to-")[0];
            string destMap = parts[0].Split("-to-")[1];

            var mapEntries = new List<(long lowEnd, long highEnd, long offset)>();
            lineIndex++;

            while (lineIndex < lines.Length && !string.IsNullOrWhiteSpace(lines[lineIndex]))
            {
                var settings = lines[lineIndex].Split(' ')
                    .Select(long.Parse).ToArray();
                long dest = settings[0];
                long source = settings[1];
                long range = settings[2];

                mapEntries.Add((source, source + range - 1, dest - source));
                lineIndex++;
            }

            if (!rangeMaps.ContainsKey(sourceMap))
            {
                rangeMaps[sourceMap] = new Dictionary<string, List<(long lowEnd, long highEnd, long offset)>>();
            }

            rangeMaps[sourceMap][destMap] = mapEntries;
        }

        lineIndex++;
    }

    // Process seed ranges through the maps and find the minimum value
    long minSeed = GetLowestValue(seedRanges, rangeMaps);

    Console.WriteLine(minSeed);
}



        private static long GetLowestValueBruteForce(
            long seedLow,
            long seedHigh,
            Dictionary<string, Dictionary<string, List<(long lowEnd, long highEnd, long offset)>>> rangeMaps)
        {
            long currentMin = long.MaxValue;

            for (long seed = seedLow; seed <= seedHigh; seed++)
            {
                long transformedSeed = seed;
                foreach (var rangeMap in rangeMaps)
                {
                    string currentMap = rangeMap.Key;
                    string nextMap = rangeMap.Value.Keys.First();
                    transformedSeed = ApplyMap(transformedSeed, rangeMaps[currentMap][nextMap]);
                }

                if (transformedSeed < currentMin)
                {
                    currentMin = transformedSeed;
                }
            }

            return currentMin;
        }

        private static long ApplyMap(long number, List<(long lowEnd, long highEnd, long offset)> map)
        {
            foreach (var (lowEnd, highEnd, offset) in map)
            {
                if (number >= lowEnd && number <= highEnd)
                {
                    return number + offset; // Apply the offset if in range
                }
            }

            return number; // Return the original number if no range applies
        }

        private static long GetLowestValue(
            List<(long start, long end)> seedRanges,
            Dictionary<string, Dictionary<string, List<(long lowEnd, long highEnd, long offset)>>> rangeMaps)
        {
            foreach (var rangeMap in rangeMaps)
            {
                //get  maps from first translation
                var nextMap = rangeMap.Value.First().Value;

                var newRanges = new List<(long start, long end)>();

                //loop through every seend range 
                foreach (var seedRange in seedRanges)
                {
                    //loop through every map
                    foreach (var (lowEnd, highEnd, offset) in nextMap)
                    {
                        // If seed range has no overlap, no need to change anything.
                        if (seedRange.end < lowEnd || seedRange.start > highEnd)
                        {
                            continue;
                        }

                        // get maximumn low end of seed range to map range 
                        long newStart = Math.Max(seedRange.start, lowEnd) + offset;
                        // get minimum high end of seed range to map range 
                        long newEnd = Math.Min(seedRange.end, highEnd) + offset;
                        //add new map range 
                        newRanges.Add((newStart, newEnd));
                    }
                }

                // Update seedRanges with the new ranges for the next map. only need to take the lowest.
                seedRanges = newRanges;
            }

            // Find the lowest start value from the final transformed ranges
            return seedRanges.Min(range => range.start);
        }


    }
}
