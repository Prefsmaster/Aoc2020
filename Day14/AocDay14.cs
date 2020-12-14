using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    internal class AocDay14
    {
        private static void Main()
        {
            var commands = File.ReadAllLines("input.txt");
            Part1(commands);
            Part2(commands);
        }

        private static void Part1(string[] commands)
        {
            var memory = new Dictionary<long,long>();
            foreach (var line in commands)
            {
                var andMask = -1L;
                var orMask = 0L;

                var parts = line.Split();
                if (parts[0] == "mask")
                {
                    andMask = orMask = 0;
                    foreach (var b in parts[2])
                    {
                        andMask = (andMask<<1) + (b == 'X' ? 1 :0);
                        orMask = (orMask<<1) + (b == '1' ? 1 : 0);
                    }
                }
                else
                {
                    //var memParts = parts[0].Split('[');
                    //var address = long.Parse(memParts[1][..^1]);
                    //var value = long.Parse(parts[2]);
                    //memory[address] = (value & andMask) | orMask;
                    memory[long.Parse(parts[0].Split('[')[1][..^1])] = (long.Parse(parts[2]) & andMask) | orMask;
                }
            }
            Console.WriteLine(memory.Sum(p => p.Value));
        }
        private static void Part2(string[] commands)
        {
            Console.WriteLine("implement me");
        }
    }
}
