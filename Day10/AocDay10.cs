using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day10
{
    internal class AocDay10
    {
        private static void Main()
        {
            var values = File.ReadAllLines("input.txt").Select(int.Parse).ToList();

            // add outlet;
            values.Add(0);
            values = values.OrderBy(j => j).ToList();
            // add device (== largest adapter + 3)
            values.Add(values[^1]+3);

            Console.WriteLine(Part1(values));
            Console.WriteLine(Part2(values));

            BothPartsIn1(values);

        }

        private static void BothPartsIn1(List<int> joltages)
        {
            int threes = 0, ones = 0, groupCount =0;
            long permutations = 1;
 
            for (var i = 1; i < joltages.Count; i++)
            {
                if (joltages[i] - joltages[i - 1] == 1)
                {
                    groupCount++;
                }
                else
                {
                    ones += groupCount;
                    threes++;
                    permutations *= (int) (Math.Pow(2, groupCount - 1) - Math.Pow(2, groupCount - 3) + 1);
                    groupCount = 0;
                }
            }
            Console.WriteLine(ones * threes);
            Console.WriteLine(permutations);
        }

        private static int Part1(List<int> joltages)
        {
            var threes = 0;
            var ones = 0;

            for (var i = 1; i < joltages.Count; i++)
            {
                var delta = joltages[i] - joltages[i - 1];
                if (delta == 1) ones++;
                if (delta == 3) threes++;
            }
            return (ones * threes);
        }

        // Part 2
        // this works when delta 'joltage' between adapters is either 1 or 3.
        // for a group of 1-delta's one or more adapters can be omitted, yielding permutations.      

        // a group of 2 delta's of 1 has 2 permutations (first adapter can be present or absent)
        // a group of 3 delta's of 1 has 4 (first 2 adapters can be all present, only one, or none)
        // a group of 4 delta's has 7 or  (2*4-1): an extra adapter is added, duplicating the possibilities for 3, but 3 absent is impossible
        // group size 5 has 13 (7*2-1) double again, but 4 absent in a row is impossible
        // group size 6 has 25 (13*2-1) etc 

        // formula: permutations = (int) 2^(n-1) - 2^(n-3) + 1;

        // group = 0:  (int) (0.5 - 0.125 + 1) = (int) 1.375 = 1 
        // group = 1:  (int) (1 - 0.25 + 1)    = (int) 1.75  = 1 
        // group = 2:  (int) (2 - 0.5 + 1)     = (int) 2.5   = 2 
        // group = 3:  (int) (4 - 1 + 1)                     = 4 
        // group = 4:  (int) (8 - 2 + 1)                     = 7
        // group = 5:  (int) (16 - 4 + 1)                    = 13

        // example 1:   0,1,4,5,6,7,10,11,12,15,16,19,22
        // deltas:       1 3 1 1 1 3  1  1  3  1  3  3
        //               |   \---/   \----/    |
        // group size:   1     3        2      1
        // permutations: 1     4        2      1
        // Possible adapter combinations: 1 * 4 * 2 * 1 = 8

        // example 2:  0,1,2,3,4,7,8,9,10,11,14,17,18,19,20,23,24,25,28,31,32,33,34,35,38,39,42,45,46,47,48,49,52
        // deltas:      1 1 1 1 3 1 1 1  1  3  3  1  1  1  3  1  1  3  3  1  1  1  1  3  1  3  3  1  1  1  1  3
        //              \-----/   \------/        \-----/     \--/        \--------/     |        \--------/
        // group size:     4          4              3          2             4          1             4
        // permutations:   7          7              4          2             7          1             7
        // Possible adapter combinations: 7 * 7 * 4 * 2 * 7 * 1 * 7 = 19208

        private static long Part2(List<int> joltages)
        {
            long permutations = 1;
            var groupCount = 0;
            for (var i = 1; i < joltages.Count; i++)
            {
                var delta = joltages[i] - joltages[i - 1];
                if (delta == 1)
                {
                    groupCount++;
                }
                else
                {
                    permutations *= (int)(Math.Pow(2, groupCount - 1) - Math.Pow(2, groupCount - 3) + 1);
                    groupCount = 0;
                }
            }
            return (permutations);
        }
    }
}
