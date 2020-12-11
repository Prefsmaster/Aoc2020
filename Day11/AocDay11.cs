using System;
using System.IO;
using System.Linq;

namespace Day11
{
    internal class AocDay11
    {
        private static void Main()
        {
            const string filename = "input.txt";
            SolveDay11(filename,4,1);
            SolveDay11(filename,5,-1);
        }

        private static void SolveDay11(string filename, int maxNeighbors, int maxSteps)
        {
            var seatGrid = File.ReadAllLines(filename);
            bool isEqual;
            do
            {
                var newGrid = IterateStep(seatGrid,maxNeighbors,maxSteps);
                isEqual = seatGrid.SequenceEqual(newGrid);
                seatGrid = newGrid;
            } while (!isEqual);
            Console.WriteLine(seatGrid.Sum(l => l.Count(x => x == '#')));
        }

        private static string[] IterateStep(string[] inGrid, int maxNeighbors, int maxSteps)
        {
            var width = inGrid[0].Length;
            var height = inGrid.Length;

            var outGrid = new string[height]; 

            for (var y = 0; y < height; y++)
            {
                outGrid[y] = "";
                for (var x = 0; x < width; x++)
                {
                    var occupiedNeighbors = 0;
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        for (var dx = -1; dx <= 1; dx++)
                        {
                            if (dx == 0 && dy == 0) continue;

                            var seatFound = false;

                            var tx = x + dx;
                            var ty = y + dy;
                            var steps = maxSteps;

                            while (ty >= 0 && ty < height && tx >= 0 && tx < width && !seatFound && steps-- != 0)
                            {
                                if (inGrid[ty][tx] != '.') seatFound = true;
                                else
                                {
                                    tx += dx;
                                    ty += dy;
                                }
                            }

                            if (seatFound && inGrid[ty][tx] == '#') occupiedNeighbors++;
                        }
                    }
                    outGrid[y] += inGrid[y][x] switch
                    {
                        'L' when occupiedNeighbors == 0 => "#",
                        '#' when occupiedNeighbors >= maxNeighbors => "L",
                        _ => inGrid[y][x]
                    };
                }
            }
            return outGrid;
        }
    }
}
