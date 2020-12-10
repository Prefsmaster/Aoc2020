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
            // add device
            values.Add(values[^1]+3);

            Console.WriteLine(Part1(values));
            Console.WriteLine(Part2(values));
        }

        private static int Part1(List<int> joltages)
        {
            var threes = 0; // device and largest
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
        // a group of 2 delta's of 1 has 2 permutations
        // a group of 3 delta's of 1 has 4
        // a group of 4 delta's has 7  (2*4-1)
        // group size 5 has 13 (7*2-1)
        // group size 6 has 25 (13*2-1)
        // etcetera.
        // formula: permutations = 2^(n-1) - 2^(n-3) + 1;

        // example 1: 0,1,4,5,6,7,10,11,12,15,16,19,22
        // deltas:     1 3 1 1 1 3  1  1  3  1  3  3
        // group size        3        2
        // permutations      4        2
        // total: 4*2

        // example 2:  0,1,2,3,4,7,8,9,10,11,14,17,18,19,20,23,24,25,28,31,32,33,34,35,38,39,42,45,46,47,48,49,52
        // deltas:      1 1 1 1 3 1 1 1  1  3  3  1  1  1  3  1  1  3  3  1  1  1  1  3  1  3  3  1  1  1  1  3
        // group size      4          4              3          2             4                        4
        // permutations    7          7              4          2             7                        7
        // total: 7*7*4*2*7*7 = 19208

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
                    permutations *= (int) (Math.Pow(2, groupCount - 1) - Math.Pow(2, groupCount - 3) + 1);
                    groupCount = 0;
                }
            }
            return (permutations);
        }
    }
}
