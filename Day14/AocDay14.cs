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
            var andMask = -1L;
            var orMask = 0L;

            foreach (var line in commands)
            {

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
                    memory[long.Parse(parts[0].Split('[')[1][..^1])] = (long.Parse(parts[2]) & andMask) | orMask;
                }
            }
            Console.WriteLine(memory.Sum(p => p.Value));
        }
        private static void Part2(string[] commands)
        {
            var memory = new Dictionary<ulong,long>();

            ulong andMask = 0L;
            ulong orMask = 0L;
            
            var floatingBitsCount = 0;
            var xOffsets = new int[36];

            foreach (var line in commands)
            {
                var parts = line.Split();
                if (parts[0] == "mask")
                {
                    andMask = orMask = 0;
                    floatingBitsCount= 0;
                    for (var b = 0;b<36;b++)
                    {
                        var c = parts[2][b];
                        orMask = (orMask<<1) + (ulong)(c == '1' ? 1 : 0);
                        andMask = (andMask<<1) + (ulong)(c != 'X' ? 1 : 0);

                        // remember where the floating bits are 
                        if (c == 'X')
                            xOffsets[floatingBitsCount++] = 35-b;
                    }
                }
                else
                {
                    var memParts = parts[0].Split('[');

                    // create base address (all floating bits 0, force bits to 1 according to orMask) 
                    var baseAddress = (ulong.Parse(memParts[1][..^1]) | orMask) & andMask;
                    
                    var value = long.Parse(parts[2]);
                    for (var offset = 0; offset < (1 << floatingBitsCount); offset++)
                    {
                        var realAddress = baseAddress;

                        // set correct bits
                        var bit = 0;
                        var bits = offset;
                        while (bits != 0)
                        {
                            if ((bits & 1) != 0)
                                realAddress |= (ulong)(1L << xOffsets[bit]);

                            bit++;
                            bits >>= 1;
                        }
                        memory[realAddress] = value;
                    }
                }
            }
            Console.WriteLine(memory.Count);
            Console.WriteLine(memory.Sum(p => p.Value));
        }
    }
}
