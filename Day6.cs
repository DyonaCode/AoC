using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace AoC
{
    static class Day6
    {
       public static void Part1()
       {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day6resource.txt");

            string pattern = @"\b\d+\b";

            MatchCollection timeMatches = Regex.Matches(lines[0], pattern);
            MatchCollection recordMatches = Regex.Matches(lines[1], pattern);

            List<int> times = new List<int>();
            List<int> records = new List<int>();

            foreach (Match timeMatch in timeMatches)
            {
                if (int.TryParse(timeMatch.Value, out int number))
                {
                    times.Add(number);
                }
            }

             foreach (Match recordMatch in recordMatches)
            {
                if (int.TryParse(recordMatch.Value, out int number))
                {
                    records.Add(number);
                }
            }

            int sum = 1;

            //Loop through each time 
            for(int i = 0; i < times.Count; i++)
            {
                int OverallTime = times[i];
                int mintime = 1;
                int maxChargeTime = times[i] - 1;
                int currentRecord = records[i];
                int AmountBetterThanRecord = 0;

                //Loop through each config of charge time and travel time. 
                for(int j = mintime; j <= maxChargeTime; j++)
                {
                    int chargeTime = j;
                    int travelTime = OverallTime - chargeTime;
                    int distance = chargeTime * travelTime;

                    if(distance > currentRecord)
                    {
                        AmountBetterThanRecord++;
                    }
                }
                
                sum *= AmountBetterThanRecord;

            }

            Console.WriteLine(sum);
           

        }


        public static void Part2()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day6resource.txt");

            string timeString = lines[0].Substring(5).Replace(" ", "").Replace("z", "");
            string recordString = lines[1].Substring(9).Replace(" ", "");

            long time = long.Parse(timeString);
            long record = long.Parse(recordString); 
        
            int mintime = 1;
            long maxChargeTime = time - 1;
            long AmountBetterThanRecord = 0;
          

            //Loop through each config of charge time and travel time. 
            for(int j = mintime; j <= maxChargeTime; j++)
            {
                long chargeTime = j;
                long travelTime = time - chargeTime;
                long distance = chargeTime * travelTime;

                if(distance > record)
                {
                    AmountBetterThanRecord++;
                }
            }
                
             Console.WriteLine($"AmountBetterThanRecord: {AmountBetterThanRecord}");
        }

        public static void Part2Part2()
        {
            string[] lines = File.ReadAllLines("/Users/jamesrogers/code/AoC/Resources/day6resource.txt");

            string timeString = lines[0].Substring(5).Replace(" ", "").Replace("z", "");
            string recordString = lines[1].Substring(9).Replace(" ", "");

            long time = long.Parse(timeString);
            long record = long.Parse(recordString); 

            long midPoint = time / 2;
            long maxDistance = midPoint * (time - midPoint);

            if (maxDistance <= record)
            {
                Console.WriteLine($"AmountBetterThanRecord: 0");
                return;
            }

            // Solve the quadratic equation travelTime * chargeTime = record
            // This is equivalent to (time - chargeTime) * chargeTime = record
            double discriminant = Math.Sqrt(time * time - 4 * record);
            long chargeTimeLowerBound = (long)Math.Ceiling((time - discriminant) / 2);
            long chargeTimeUpperBound = (long)Math.Floor((time + discriminant) / 2);

            long AmountBetterThanRecord = chargeTimeUpperBound - chargeTimeLowerBound + 1;

            Console.WriteLine($"AmountBetterThanRecord: {AmountBetterThanRecord}");
        }

    }
}
