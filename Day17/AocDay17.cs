using System;
using System.IO;

namespace Day17
{
    internal class AocDay17
    {
        private static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            Part1(input,6);
            Part2(input,6);
        }

        private static void Part1(string[] input, int iterations)
        {
            var patternSide = input[0].Length;

            // allocate an array with room to expand for the number of iterations;
            // allow for a little more room to avoid boundary checking 
            
            var cubeSide = patternSide + 2 * iterations;
            var currentState = Get3DArray(cubeSide + 2);

            // place the initial pattern in the 'middle' of the cube
            for (var y = 0; y < patternSide; y++)
                for (var x = 0; x < patternSide; x++)
                    if (input[y][x] == '#') 
                        currentState[x+iterations+1][y+iterations+1][1 + cubeSide / 2] = 1;
 
            // count while we go
            var alive = 0;
            while (iterations-- > 0)
            {
                var newState = Get3DArray(cubeSide + 2);
                alive = 0;
                for (var x = 1; x < cubeSide; x++)
                    for (var y = 1; y < cubeSide; y++)
                        for (var z = 1; z < cubeSide; z++)
                        {
                            var n = NeighborCount3D(currentState, x, y, z);
                            if (currentState[x][y][z] != 0)
                                alive += newState[x][y][z] = (n == 2 || n == 3) ? 1 : 0;
                            else
                                alive += newState[x][y][z] = (n == 3) ? 1 : 0;
                        }
                currentState = newState;                
            }
            Console.WriteLine(alive);
        }
        private static void Part2(string[] input, int iterations)
        {
            var patternSide = input[0].Length;
            // allocate a 3d array with room to expand for the number of iterations;
            // allow for a little more room to avoid boundary checking 
            var cubeSide = patternSide + 2 * iterations;
            var currentState = Get4DArray(cubeSide + 2);

            // place the initial pattern in the 'middle' of the cube
            var zStart = 1 + cubeSide / 2;
            for (var y = 0; y < patternSide; y++)
                for (var x = 0; x < patternSide; x++)
                    if (input[y][x] == '#') 
                        currentState[x+iterations+1][y+iterations+1][zStart][zStart] = 1;

            var alive = 0;
            while (iterations-- > 0)
            {
                var newState = Get4DArray(cubeSide + 2);
                alive = 0;
                for (var x = 1; x < cubeSide; x++)
                    for (var y = 1; y < cubeSide; y++)
                        for (var z = 1; z < cubeSide; z++)
                            for (var w = 1; w < cubeSide; w++)
                            {
                                var n = NeighborCount4D(currentState, x, y, z, w);
                                if (currentState[x][y][z][w] != 0)
                                    alive += newState[x][y][z][w] = (n == 2 || n == 3) ? 1 : 0;
                                else
                                    alive += newState[x][y][z][w] = (n == 3) ? 1 : 0;
                            }
                currentState = newState;                
            }
            Console.WriteLine(alive);
        }

        private static int NeighborCount3D(int[][][] a, int x, int y, int z)
        {
            var count = 0;
            for (var dx = -1;dx<=1;dx++)
            for (var dy = -1;dy<=1;dy++)
            for (var dz = -1;dz<=1;dz++)
            {
                if (!(dx == 0 && dy == 0 && dz == 0))
                    count += a[x + dx][y + dy][z + dz];
            }
            return count;
        }
        private static int[][][] Get3DArray(int side)
        {
            var array = new int[side][][];
            for (var y = 0; y < side; y++)
            {
                array[y] = new int[side][];
                for (var z = 0; z < side; z++)
                {
                    array[y][z] = new int[side];
                }
            }
            return array;
        }

        private static int NeighborCount4D(int[][][][] a, int x, int y, int z, int w)
        {
            var count = 0;
            for (var dx = -1;dx<=1;dx++)
            for (var dy = -1;dy<=1;dy++)
            for (var dz = -1;dz<=1;dz++)
            for (var dw = -1;dw<=1;dw++)
            {
                if (!(dx == 0 && dy == 0 && dz == 0 && dw ==0))
                    count += a[x + dx][y + dy][z + dz][w + dw];
            }
            return count;
        }

        private static int[][][][] Get4DArray(int side)
        {
            var array = new int[side][][][];
            for (var y = 0; y < side; y++)
            {
                array[y] = new int[side][][];
                for (var z = 0; z < side; z++)
                {
                    array[y][z] = new int[side][];
                    for (var w = 0; w < side; w++)
                    {
                        array[y][z][w] = new int[side];
                    }
                }
            }
            return array;
        }
    }
}
