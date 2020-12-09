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

            var invalidNumber = InvalidNumber(25, values);
            Console.WriteLine(invalidNumber);
            Console.WriteLine(FindWeakness(invalidNumber,values));
        }

        private static long InvalidNumber(int preamble, long[] numbers)
        {

            for (var candidateIndex = preamble; candidateIndex < numbers.Length; candidateIndex++)
            {
                var found = false;
                var candidate = numbers[candidateIndex];
                for (var i1= candidateIndex-preamble; i1<candidateIndex && !found; i1++)
                {
                    for (var i2 = i1 + 1; i2<candidateIndex && !found; i2++)
                    {
                        found = (numbers[i1] + numbers[i2] == candidate);
                    }
                }
                if (!found) return candidate;
            }
            return -1; // Indicate failure
        }

        private static long FindWeakness(long targetValue, long[] numbers)
        {
            var startIndex = 0;
            var endIndex = 0;
            var sum = 0L;

            while (sum != targetValue || endIndex-startIndex <=2)
            {
                // use sliding window approach

                // add from end until target reached or too big
                while (sum < targetValue) sum += numbers[endIndex++];

                // subtract from start until target reached or too small
                while (sum > targetValue) sum -= numbers[startIndex++];
            }

            return numbers[startIndex..endIndex].Min() +
                   numbers[startIndex..endIndex].Max();
        }
    }
}
