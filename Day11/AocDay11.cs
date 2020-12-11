using System;
using System.IO;
using System.Linq;

namespace Day11
{
    internal class AocDay11
    {
        private static void Main()
        {
            var seatGrid = File.ReadAllLines("input.txt");

            bool isEqual;
            do
            {
                var newGrid = Step1(seatGrid);
                isEqual = seatGrid.SequenceEqual(newGrid);
                seatGrid = newGrid;
            } while (!isEqual);

            // count seats
            // print
            Console.WriteLine(CountOccupied(seatGrid));

            seatGrid = File.ReadAllLines("input.txt");
            do
            {
                var newGrid = Step2(seatGrid);
                isEqual = seatGrid.SequenceEqual(newGrid);
                seatGrid = newGrid;
            } while (!isEqual);

            Console.WriteLine(CountOccupied(seatGrid));
        }

        private static string[] Step1(string[] inGrid)
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
                    for (var ty = y-1; ty <= y+1; ty++)
                    for (var tx = x-1; tx <= x+1; tx++)
                    {
                        if (tx == x && ty == y) continue;
                        if (ty >= 0 && ty < height && tx >= 0 && tx < width && inGrid[ty][tx] == '#') occupiedNeighbors++;
                    }

                    if (inGrid[y][x] == '#') occupiedNeighbors--;

                    outGrid[y] += inGrid[y][x] switch
                    {
                        'L' when occupiedNeighbors == 0 => "#",
                        '#' when occupiedNeighbors >= 4 => "L",
                        _ => inGrid[y][x]
                    };
                }
            }

            return outGrid;
        }

        private static string[] Step2(string[] inGrid)
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
                    for (var dx = -1; dx <= 1; dx++)
                    {
                        if (dx == 0 && dy == 0) continue;

                        var seatFound = false;

                        var tx = x + dx;
                        var ty = y + dy;
                        while (ty >= 0 && ty < height && tx >= 0 && tx < width && !seatFound)
                        {
                            if (inGrid[ty][tx] != '.') seatFound = true;
                            else
                            {
                                tx += dx;
                                ty += dy;
                            }
                        }
                        if (seatFound && inGrid[ty][tx]=='#') occupiedNeighbors++;
                    }

                    outGrid[y] += inGrid[y][x] switch
                    {
                        'L' when occupiedNeighbors == 0 => "#",
                        '#' when occupiedNeighbors >= 5 => "L",
                        _ => inGrid[y][x]
                    };
                }
            }

            return outGrid;
        }
        
        private static int CountOccupied(string[] inGrid)
        {
            var width = inGrid[0].Length;
            var height = inGrid.Length;
            var occupiedSeats = 0; 

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (inGrid[y][x] == '#') occupiedSeats++;
                }
            }
            return occupiedSeats;
        }
    }
}
