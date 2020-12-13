using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Day13
{
    internal class AocDay13
    {
        private static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            Part1(input);
            Part2(input[1]);
        }

        private static void Part1(string[] input)
        {
            var timestamp = long.Parse(input[0]);

            var roundTimes = input[1].Split(',').Where(p => p != "x").Select(decimal.Parse);

            decimal minBiggest = 2*timestamp;
            decimal busId = -1;
            foreach (var t in roundTimes)
            {
                var n = Math.Ceiling(timestamp / t) * t;
                if (n < minBiggest)
                {
                    minBiggest = n;
                    busId = t;
                }
            }
            Console.WriteLine(busId * (minBiggest-timestamp));
        }

        // first find timestamp that works for first pair, using 1st value for stepsize.
        // step size now becomes val1*val2
        // repeat for each next value.
        private static void Part2(string input)
        {
            var roundTimes = input.Split(',').Select(p => p == "x" ? 0: long.Parse(p)).ToArray();

            var c = 0;
            var b = roundTimes[c];
            long timestamp = 0;

            do
            {
                while (roundTimes[++c] == 0) { }
                while ((timestamp+c) % roundTimes[c] !=0 ) timestamp += b;
                b *= roundTimes[c];
            } while (c != roundTimes.Length - 1);

            Console.WriteLine(timestamp);
        }

    }
}
