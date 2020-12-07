using System;
using System.IO;
using System.Linq;

namespace Day3
{
    internal class AocDay3
    {
        private static void Main()
        {
            var treeRows = File.ReadAllLines(@"input.txt");
            Console.WriteLine($@"Trees hit for right 3, down 1 {CountTrees(treeRows, 3,1)}");
            Console.WriteLine($@"Answer for 5 slopes {
                    CountTrees(treeRows, 1,1)
                    * CountTrees(treeRows, 3,1)
                    * CountTrees(treeRows, 5,1)
                    * CountTrees(treeRows, 7,1)
                    * CountTrees(treeRows, 1,2)
                }");
        }

        private static long CountTrees(string[] rows, int dx, int dy)
        {
            var rowLength = rows[0].Length;
            return rows.Where((row, rowNumber) => rowNumber % dy == 0 && row[(rowNumber * dx/dy) % rowLength] == '#').Count();
        }
    }
}
