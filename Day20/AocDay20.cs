using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    internal class AocDay20
    {

        private static void Main()
        {
            const string filename = "input.txt";
            var tiles = ReadTiles(filename);
            Console.WriteLine($"read {tiles.Count} tiles with side {tiles[0].Data.Length}\n");

            tiles[0].ShowAllSides();

            DoPart1(tiles);
            DoPart2(tiles);
        }

        private static void DoPart2(List<Tile> tiles)
        {
            var photo = new Photo((int)Math.Sqrt(tiles.Count));

            var success = photo.SolveBoard(tiles,0);

            photo.Dump();
        }

        private static void DoPart1(List<Tile> tiles)
        {
            // solution idea was: find the 4 tiles that have only 2 possible 'connecting'
            // neighbors. a possible neighbor shares a side pattern.
            // we count candidates that share a side, and when this equals 2
            // it must be a corner piece, and we multiply them...
            // of course, we must not check with the candidate itself!
            long answer = 1;
            for (var candidateIndex = 0; candidateIndex < tiles.Count; candidateIndex++)
            {
                var neighborCount =
                    tiles.Where((t, neighborIndex) => candidateIndex != neighborIndex && tiles[candidateIndex].SideValues.Count(t.SideValues.Contains) > 0)
                        .Count();

                if (neighborCount == 2)
                {
                    Console.WriteLine($"tile {tiles[candidateIndex].Id} has only 2 possible neighbors");
                    answer *= tiles[candidateIndex].Id;
                }
            }
            Console.WriteLine($"answer part1 = {answer}\n");
        }

        private static List<Tile> ReadTiles(string filename)
        {
            var file = new StreamReader(filename);
            string line;
            var tiles = new List<Tile>();
            do
            {
                // 'Tile 1234:' part
                var id = int.Parse(file.ReadLine().Split()[1][..^1]);

                // read first line of data to determine side size
                line = file.ReadLine();
                var lines = line.Length;
                var data = new string[lines];
                data[0] = line;
                for (var l = 1; l < lines; l++)
                    data[l] = file.ReadLine();

                tiles.Add(new Tile(id, data));

                // read trailing empty line, will return null after last tile.
                line = file.ReadLine();
            } while (line != null);
            return tiles;
        }
    }

    internal class Tile
    {
        private const int Permutations = 8;

        public int Id;
        public int[] SideValues;
        public char[][] Data;
        public bool IsPlaced;

        // orientation 0-3 = normal rotated 0-3 times counterclockwise
        // orientation 4-7 = flipped X, rotated 0-3 times counterclockwise
        // orientation 8-11 = flipped Y, rotated 0-3 times counterclockwise
        public int Orientation;

        public Tile(int id, string[] data)
        {
            Id = id;
            Orientation = 0;
            IsPlaced = false;

            FillData(data);
            CreateSideValues();
            Console.WriteLine($"{string.Join(",", SideValues)}");
        }

        private static readonly int[,] FlippedSidesMap =
        {
            // normal orientation T R B L
            { 0,1,2,3 },
            // flipped, use L' B' R' T'
            { 7,6,5,4 }
            // No need to provide flipped x+y or diagonally x+y because this is just
            // a rotation by 180 degrees, and is already
            // covered by the 4 rotations of 'normal' 
        };

        private void FillData(string[] data)
        {
            Data = new char[data.Length][];
            for (var l = 0; l < data.Length; l++)
            {
                Data[l] = data[l].ToCharArray();
            }
        }
        private void CreateSideValues()
        {
            SideValues = new int[8];
            var maxIndex = Data.Length - 1; 
            var b = 1; var b2 = 1 << maxIndex;
            for (var p = 0; p <= maxIndex; p++)
            {
                // Top and Top'
                if (Data[0][p] == '#') { SideValues[0] += b; SideValues[4] += b2; }
                // Right and Right'
                if (Data[p][^1] == '#') { SideValues[1] += b; SideValues[5] += b2; }
                // Bottom and Bottom'
                if (Data[^1][maxIndex-p] == '#') { SideValues[2] += b; SideValues[6] += b2; }
                // Left and Left'
                if (Data[maxIndex-p][0] == '#') { SideValues[3] += b; SideValues[7] += b2; }
                b <<= 1; b2 >>= 1;
            }
        }

        public int Side(Sides side) => SideValues[FlippedSidesMap[Orientation / 4,((Orientation % 4) + (int)side)%4]];
        public int Side2(Sides side) => SideValues[FlippedSidesMap[1-Orientation / 4,3-((Orientation % 4) + (int)side)%4]];

        public bool SidesMatch(Tile tile, Sides own, Sides other)
        {
            if (tile == null) return true;
            var ownValue = Side(own);
            var otherValue = tile.Side2(other);
            if (otherValue != ownValue)
                return false;
            return true;
        }

        public void ShowAllSides()
        {
            var s = FlippedSidesMap.Length;

            Console.WriteLine($"Tile {Id} values: {string.Join(",", SideValues)}");
            for (var r = 0; r < Permutations; r++)
            {
                Orientation = r;
                Console.Write($"{Side(Sides.Top):D3} ");
                Console.Write($"{Side(Sides.Right):D3} ");
                Console.Write($"{Side(Sides.Bottom):D3} ");
                Console.Write($"{Side(Sides.Left):D3}");
                Console.Write($"{Side2(Sides.Top):D3} ");
                Console.Write($"{Side2(Sides.Right):D3} ");
                Console.Write($"{Side2(Sides.Bottom):D3} ");
                Console.WriteLine($"{Side2(Sides.Left):D3}");
                if (r%4==3)
                    Console.WriteLine();
            }
        }
    }

    internal enum Sides
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }

    internal class Photo
    {
        private const int Permutations = 8;

        public Tile[] Parts;
        private readonly int _side;

        public Photo(int side)
        {
            _side = side;
            Parts = new Tile[_side*_side];
        }
 
        public bool SolveBoard(List<Tile> tiles, int pos)
        {
            foreach(var candidate in tiles.Where(t=>!t.IsPlaced))
            {
                for ( var o = 0; o < Permutations; o++)
                {
                    candidate.Orientation = o;
                    if (!TileFits(candidate, pos)) continue;

                    Parts[pos] = candidate;
                    candidate.IsPlaced = true;

                    if ((pos == _side * _side - 1) || (SolveBoard(tiles, pos + 1))) return true;

                    Parts[pos] = null;
                    candidate.IsPlaced = false;
                }
            }
            return false;
        }

        private bool TileFits(Tile t, int pos)
        {
            var term1 = (pos/_side <= 0) || (t.SidesMatch(Parts[pos-_side], Sides.Top, Sides.Bottom));
            var term2 = (pos%_side <= 0) || (t.SidesMatch(Parts[pos- 1], Sides.Left, Sides.Right));
            return term1 && term2;
        }

        public void Dump()
        {
            var p = 0;
            for (var y = 0; y < _side; y++)
            {
                for (var x = 0; x < _side; x++)
                {
                    Console.Write($"{(Parts[p] == null ? 0 : Parts[p].Id):D4}:{(Parts[p] == null ? 0 : Parts[p].Orientation):D2} ");
                    p++;
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
