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

            var success = photo.RecursiveBoard(tiles);

            // check to see if idea for ring-wise solution will work
            //var countIds = new List<int>[5];
            //for (var cil = 0; cil < 5; cil++)
            //    countIds[cil] = new List<int>();

            //while (tiles.Count != 0)
            //{
            //    for (var cil = 0; cil < 5; cil++) countIds[cil].Clear();
            //    for (var a1 = 0; a1 < tiles.Count; a1++)
            //    {
            //        var matchingSides = tiles.Where((t, a2) => a1 != a2 && tiles[a1].SideValues.Count(t.SideValues.Contains) > 0).Count();
            //        countIds[matchingSides].Add(tiles[a1].Id);
            //    }
            //    Console.WriteLine($"{string.Join(",", countIds.Select(cid => cid.Count))}");

            //    var tilesToPlace = tiles.Where(t => countIds[2].Contains(t.Id)).ToList();
            //    tilesToPlace.AddRange(tiles.Where(t => countIds[3].Contains(t.Id)));
            //    tilesToPlace.AddRange(tiles.Where(t => countIds[0].Contains(t.Id)));

            //    var success = photo.RecursiveRing(tilesToPlace);

            //    // photo.Dump();
            //    // continue with next ring

            //    tiles = tiles.Where(t => countIds[4].Contains(t.Id)).ToList();

            //    //if (tiles.Count == 1)
            //    //{
            //    //    photo.PlaceRing(tiles, new List<Tile>());
            //    //    tiles.Clear();
            //    //}
            //    //else
            //    //{
            //    //    var corners = tiles.Where(t => countIds[2].Contains(t.Id)).ToList();
            //    //    var edges = tiles.Where(t => countIds[3].Contains(t.Id)).ToList();

            //    //    // place ring
            //    //    photo.PlaceRing(corners, edges);                

            //    //    // continue with next ring
            //    //    tiles = tiles.Where(t => countIds[4].Contains(t.Id)).ToList();
            //    //}
            //}
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
            // flipped diagonally
            { 7,6,5,4 },
            // flipped y = vertically so use B R' T L'
            { 4,3,6,1 },
            // flipped diagonally
            { 5,2,7,0 },
            // flipped y = vertically so use B R' T L'
            { 2,5,0,7 },
            // flipped diagonally
            { 3,4,2,6 }
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

        public int Side(Sides side) => SideValues[FlippedSidesMap[Orientation / 4,((Orientation % 4) + (int)side)%4]];

        public bool SidesMatch(Tile tile, Sides own, Sides other)
        {
            return (tile == null) || Side(own) == tile.Side(other);
        }

        public void ShowAllSides()
        {
            Console.WriteLine($"Tile {Id} values: {string.Join(",", SideValues)}");
            for (var r = 0; r < 24; r++)
            {
                Orientation = r;
                Console.Write($"{Side(Sides.Top):D3} ");
                Console.Write($"{Side(Sides.Right):D3} ");
                Console.Write($"{Side(Sides.Bottom):D3} ");
                Console.WriteLine($"{Side(Sides.Left):D3}");
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
        public Tile[][] Parts;
        private readonly int _side;
        private bool _haveFirstRing;

        private readonly int[] _dx = { 1, 0, -1, 0 };
        private readonly int[] _dy = { 0, 1, 0, -1 };

        private int _tilesPerSide;


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
            return DoNextRing(corners, edges);
        }

        public bool RecursiveRing(List<Tile> tiles)
        {
            _tilesPerSide = tiles.Count / 4;
            var start = (_side - _tilesPerSide - 1) / 2;
            var success = false;
            success = PlaceTile(tiles, 0, start, start);
            return success;
        }
        public bool RecursiveBoard(List<Tile> tiles)
        {
            tries = 0;
            var success = false;
            success = PlaceTileBoard(tiles,1, 1);
            return success;
        }

        private bool PlaceTile(List<Tile> tiles, int placed, int x, int y)
        {
            foreach(var candidate in tiles)
            {
 //               Console.WriteLine($"{candidate.Id} {placed} {x} {y}");
                // try each orientation.
                if (candidate.IsPlaced) continue;

                for (var o = 0; o < 24; o++)
                {
                    candidate.Orientation = o;

                    if (!TileFits(candidate, x, y)) continue;

                    Parts[y][x] = candidate;
                    candidate.IsPlaced = true;

                    if (tiles.All(t => t.IsPlaced))
                    {
                        Dump();
//                        return true;
                        Parts[y][x] = null;
                        candidate.IsPlaced = false;
                    }
                    else
                    {
                        // one more placed, do next
                        var nextX = x + _dx[placed / _tilesPerSide];
                        var nextY = y + _dy[placed / _tilesPerSide];
                        placed++;
                        if (PlaceTile(tiles, placed, nextX, nextY))
                        {
                            return true;
                        }

                        placed--;
                        Parts[y][x] = null;
                        candidate.IsPlaced = false;
                    }
                }
            }
            return false;
        }

        private long tries;
        private bool PlaceTileBoard(List<Tile> tiles, int x, int y)
        {
            foreach(var candidate in tiles.Where(t=>!t.IsPlaced))
            {
                for ( var o = 0; o < 24; o++)
                {
                    if (tries++%1000000 == 0)
                        Console.Write($"{tiles.Count(t => t.IsPlaced)} ");

                    candidate.Orientation = o;
                    if (!TileFits(candidate, x, y)) continue;

                    Parts[y][x] = candidate;
                    candidate.IsPlaced = true;

                    if (tiles.All(t => t.IsPlaced))
                    {
                        Dump();
//                        return true;
                        Parts[y][x] = null;
                        candidate.IsPlaced = false;
                    }
                    else
                    {

                        // one more placed, do next
                        var nextX = x + 1;
                        var nextY = y;
                        if (nextX == _side-1)
                        {
                            nextX = 1;
                            nextY++;
                        }
                        if (PlaceTileBoard(tiles, nextX, nextY))
                        {
                            return true;
                        }
                        Parts[y][x] = null;
                        candidate.IsPlaced = false;
                    }
                }
            }
            return false;
        }


        private bool DoFirstRing(List<Tile> corners, List<Tile> edges)
        {
            var edgesPerSide = edges.Count / 4;
            var success = false;

            var orgCorners = new List<Tile>(corners);
            var orgEdges = new List<Tile>(edges);
            for (var orientation = 0; orientation < 24 && !success; orientation++)
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
                    x += _dx[side];
                    y += _dy[side];
                    success = true;
                }
            } while (success && piecesToPlace != 0);
            return success;
        }

        private bool TryPiece(Tile t, int x, int y)
        {
            for (var o = 0; o < 24; o++)
            {
                t.Orientation = o;
                if (TileFits(t, x, y)) return true;
            }
            return false;
        }

        //private bool TileFits(Tile t, int x, int y) => true;
        private bool TileFits(Tile t, int x, int y) =>
            t.SidesMatch(Parts[y - 1][x], Sides.Top, Sides.Bottom) &&
            t.SidesMatch(Parts[y + 1][x], Sides.Bottom, Sides.Top) &&
            t.SidesMatch(Parts[y][x + 1], Sides.Right, Sides.Left) &&
            t.SidesMatch(Parts[y][x - 1], Sides.Left, Sides.Right);

        public void Dump()
        {
            for (var y = 0; y < _side; y++)
            {
                for (var x = 0; x < _side; x++)
                {
                    Console.Write($"{(Parts[y][x] == null ? 0 : Parts[y][x].Id):D4}:{(Parts[y][x] == null ? 0 : Parts[y][x].Orientation):D2} ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
