using System;
using System.IO;
using System.Linq;

namespace Day9
{
    internal class AocDay9
    {
        private static void Main()
        {
            var values = File.ReadAllLines("input.txt").Select(long.Parse).ToArray();
            // Part 1
            var invalidNumber = InvalidNumber(25, values);
            Console.WriteLine(invalidNumber);
            // Part 2
            Console.WriteLine(FindWeakness(invalidNumber,values));
        }

        private static long InvalidNumber(int preamble, long[] numbers)
        {
            for (var candidateIndex = preamble; candidateIndex < numbers.Length; candidateIndex++)
            {
                var found = false;
                var candidate = numbers[candidateIndex];
                for (var i1= candidateIndex-preamble; i1<candidateIndex && !found; i1++)
                    for (var i2 = i1 + 1; i2 < candidateIndex && !(found = numbers[i1] + numbers[i2] == candidate); i2++) {}
                if (!found) return candidate;
            }
            return -1; // Indicates all numbers are valid
        }

        // use sliding window approach. 
        // add from end until target reached or too big
        // subtract from start until target reached or too small
        private static long FindWeakness(long targetValue, long[] numbers)
        {
            int startIndex = 0, endIndex = 0;
            var sum = 0L;

            while (sum != targetValue || endIndex-startIndex <=2)
            {
                while (sum < targetValue) sum += numbers[endIndex++];
                while (sum > targetValue) sum -= numbers[startIndex++];
            }

            return numbers[startIndex..endIndex].Min() +
                   numbers[startIndex..endIndex].Max();
        }
    }
}
