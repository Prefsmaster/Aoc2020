using System;
using System.Collections.Generic;

namespace Day15
{
    internal class AocDay15
    {
        private static void Main()
        {
            var input = new[] { 9,12,1,4,17,0,18 };
            Day15(input, 2020);
            Day15(input, 30000000);
        }

        private static void Day15(int[] input, int turns)
        {
            var turn = 1;
            var numberSpoken = 0;

            var numbers = new Dictionary<int, int[]>();
            foreach (var t in input)
                numbers[numberSpoken = t] = new []{turn++,0};

            while (turn <= turns)
            {
                numberSpoken = (numbers[numberSpoken][1] == 0) ? 0 : numbers[numberSpoken][0] - numbers[numberSpoken][1];

                if (numbers.ContainsKey(numberSpoken))
                {
                    numbers[numberSpoken][1] = numbers[numberSpoken][0];
                    numbers[numberSpoken][0] = turn++;
                }
                else
                {
                    numbers[numberSpoken] = new []{turn++,0};
                }
            }
            Console.WriteLine($"{numbers.Count} different numbers spoken in {turns} turns. Last number: {numberSpoken}");
        }
    }
}
