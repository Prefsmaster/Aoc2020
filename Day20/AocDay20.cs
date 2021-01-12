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
            const string filename = "test1.txt";
            var tiles = ReadTiles(filename);
            Console.WriteLine($"read {tiles.Count} tiles with side {tiles[0].Data.Length}\n");

            DoPart1(tiles);
            DoPart2ApproachCheck(tiles);
        }

        private static void DoPart2ApproachCheck(List<Tile> tiles)
        {
            var photo = new Photo((int)Math.Sqrt(tiles.Count)); 

            // check to see if idea for ring-wise solution will work
            var countIds = new List<int>[5];
            for (var cil=0;cil<5;cil++)
                countIds[cil] = new List<int>();
            
            while (tiles.Count!=0)
            {
                for (var cil=0;cil<5;cil++) countIds[cil].Clear();
                for (var a1 = 0; a1 < tiles.Count; a1++)
                {
                    var matchingSides = tiles.Where((t, a2) => a1 != a2 && tiles[a1].SideValues.Count(t.SideValues.Contains) > 0).Count();
                    countIds[matchingSides].Add(tiles[a1].Id);
                }
                Console.WriteLine($"{string.Join(",", countIds.Select(cid=>cid.Count))}");

                if (tiles.Count == 1)
                {
                    photo.PlaceRing(tiles, new List<Tile>());
                    tiles.Clear();
                }
                else
                {
                    var corners = tiles.Where(t => countIds[2].Contains(t.Id)).ToList();
                    var edges = tiles.Where(t => countIds[3].Contains(t.Id)).ToList();
                
                    // place ring
                    photo.PlaceRing(corners, edges);                

                    // continue with next ring
                    tiles = tiles.Where(t => countIds[4].Contains(t.Id)).ToList();
                }
            }
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
        public int Id;
        public int[] SideValues;
        public int[][] SideTable;
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
            InitSideTable(data.Length);
            FillSideTable();
        }

        private void InitSideTable(int bits)
        {
            SideTable = new int[3][];
            for (var t = 0; t < 3; t++)
            {
                SideTable[t] = new int[7];
                for (var s = 0; s < 7; s++) SideTable[t][s] = 1 << bits;
            }
        }

        private static readonly int[,] FlippedSidesMap =
        {
            // normal orientation T R B L
            { 0,1,2,3 },
            // flipped x = horizontally so use T' L B' R
            { 4,3,6,1 },
            // flipped y = vertically so use B R' T L'
            { 2,5,0,7 }
            // No need to provide flipped x+y because this is just
            // a rotation by 180 degrees, and is already
            // covered by the 4 rotations of 'normal' 
        };
        private void FillSideTable()
        {
            // 3 flip states. 0 = none, 1=hor. e, 2 = ver.
            for (var f = 0; f < 3; f++)
            {
                // 4 rotations of 90 degrees
                for (var s = 0; s < 4; s++)
                {
                    SideTable[f][s] = SideValues[FlippedSidesMap[f,s]];
                    if (s != 3)
                        SideTable[f][s + 4] = SideValues[FlippedSidesMap[f,s]];
                }
                Console.WriteLine($"{string.Join(",", SideTable[f])}");
            }
            Console.WriteLine();
        }

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
            var b = 1; var b2 = 1 << (Data.Length - 1);
            for (var p = 0; p < Data.Length; p++)
            {
                // Top and Top'
                if (Data[0][p] == '#') { SideValues[0] += b; SideValues[4] += b2; }
                // Right and Right'
                if (Data[p][^1] == '#') { SideValues[1] += b; SideValues[5] += b2; }
                // Bottom and Bottom'
                if (Data[^1][p] == '#') { SideValues[2] += b; SideValues[6] += b2; }
                // Left and Left'
                if (Data[p][0] == '#') { SideValues[3] += b; SideValues[7] += b2; }
                b <<= 1; b2 >>= 1;
            }
        }

        public int Side(Sides side) => SideTable[Orientation / 4][Orientation % 4 + (int)side];

        public bool SidesMatch(Tile tile, Sides own, Sides other)
        {
            return (tile==null) || Side(own) == tile.Side(other);
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
        public Tile[][] Parts;
        private int _side;
        private bool _haveFirstRing;
        public Photo(int side)
        {
            _side = side + 2;
            // create a picture with a ring of forever 'null' tiles around it.
            Parts = new Tile[_side][];
            for (var r = 0; r < _side; r++)
            {
                Parts[r] = new Tile[_side];
            }
        }

        public bool PlaceRing(List<Tile> corners, List<Tile> edges)
        {
            if (!_haveFirstRing)
                return _haveFirstRing = DoFirstRing(corners, edges);
            return DoNextRing(corners,edges);
        }

        private bool DoFirstRing(List<Tile> corners, List<Tile> edges)
        {
            var edgesPerSide = edges.Count / 4;
            var success = false;
            
            var orgCorners = new List<Tile>(corners);
            var orgEdges = new List<Tile>(edges);
            for (var orientation = 0; orientation < 12 && !success; orientation++)
            {
                var x = 1;
                var y = 1;
                success = true;

                for (var side = 0; side < 4 && success; side++)
                {
                    if (side == 0) corners[0].Orientation = orientation;

                    success = PlaceTiles(corners, 1, ref x, ref y, side, side == 0);
                    if (success && edges.Count >= edgesPerSide)
                        success = PlaceTiles(edges, edgesPerSide, ref x, ref y, side);
                }

                if (success) continue;

                // reset photo
                for (var r = 0; r < _side; r++)
                {
                    for (var t = 0; t < _side; t++)
                        Parts[r][t] = null;
                }
                edges = new List<Tile>(orgEdges);
                corners = new List<Tile>(orgCorners);
            }
            return success;
        }

        private bool DoNextRing(List<Tile> corners, List<Tile> edges)
        {
            var edgesPerSide = edges.Count / 4;
 
            var x = (_side - edgesPerSide - 2) / 2;
            var y = x;

            var success = true;

            for (var side = 0; side < corners.Count && success; side++)
            {
                success = PlaceTiles(corners, 1, ref x, ref y, side);
                if (success && edges.Count >= edgesPerSide)
                    success = PlaceTiles(edges, edgesPerSide, ref x, ref y, side);
            }
            return success;
        }

        private bool PlaceTiles(List<Tile> tiles, int piecesToPlace, ref int x, ref int y, int side, bool firstCorner = false)
        {
            int[] dx = {1, 0, -1, 0};
            int[] dy = {0, 1, 0, -1};

            var success = false;
            do
            {
                for (var c = 0; c < tiles.Count && piecesToPlace != 0; c++)
                {
                    var fits = firstCorner ? TileFits(tiles[c], x, y) : TryPiece(tiles[c], x, y);
                    if (!fits) continue;

                    piecesToPlace--;
                    Parts[y][x] = tiles[c];
                    tiles.RemoveAt(c);
                    x += dx[side];
                    y += dy[side];
                    success = true;
                }
            } while (success && piecesToPlace != 0);
            return success;
        }

        private bool TryPiece(Tile t, int x, int y)
        {
            for (var o = 0; o < 12; o++)
            {
                t.Orientation = o;
                if (TileFits(t, x, y)) return true;
            }
            return false;
        }

        private bool TileFits(Tile t, int x, int y) =>
            t.SidesMatch(Parts[x][y - 1], Sides.Top, Sides.Bottom) &&
            t.SidesMatch(Parts[x][y + 1], Sides.Bottom, Sides.Top) &&
            t.SidesMatch(Parts[x + 1][y], Sides.Right, Sides.Left) &&
            t.SidesMatch(Parts[x - 1][y], Sides.Left, Sides.Right);
    }
}
