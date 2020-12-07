using System;
using System.IO;
using System.Linq;

namespace Day01
{
    internal class AocDay1
    {
        private static void Main()
        { 
            var values = File.ReadAllLines(@"input\input.txt").Select(int.Parse).ToArray();

            Console.WriteLine(Part1(values));
            Console.WriteLine(Part2(values));
        }

        private static int Part1(int[] numbers)
        {
            for (var val1 = 0; val1 < numbers.Length;val1++)
            {
                for (var val2 = val1 + 1; val2 < numbers.Length; val2++)
                {
                    if (numbers[val1] + numbers[val2] == 2020)
                    {
                        return (numbers[val1] * numbers[val2]);
                    }
                }
            }

            return -1;

        }
        private static int Part2(int[] numbers)
        {
            for (var val1 = 0; val1 < numbers.Length; val1++)
            {
                for (var val2 = val1 + 1; val2 < numbers.Length; val2++)
                {
                    for (var val3 = val2 + 1; val3 < numbers.Length; val3++)
                    {
                        if (numbers[val1] + numbers[val2] + numbers[val3] == 2020)
                        {
                            return (numbers[val1] * numbers[val2] * numbers[val3]);
                        }
                    }
                }
            }

            return -1;
        }

    }
}
