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

            DoPart1(tiles);
            DoPart2ApproachCheck(tiles);
        }

        private static void DoPart2ApproachCheck(List<Tile> tiles)
        {
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
                // keep all tiles with 4 neighbors
                tiles = tiles.Where(t => countIds[4].Contains(t.Id)).ToList();
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
//                    Console.WriteLine($"tile {tiles[candidateIndex].Id} has only 2 possible neighbors");
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
        public string[] Data;
        public Tile(int id, string[] data)
        {
            Data = data;
            Id = id;
            CreateSideValues();
            Console.WriteLine($"{string.Join(",", SideValues)}");
        }

        private void CreateSideValues()
        {
            SideValues = new int[8];
            var b = 1; var b2 = 1 << (Data.Length - 1);
            for (var p = 0; p < Data.Length; p++)
            {
                if (Data[0][p] == '#') { SideValues[0] += b; SideValues[4] += b2; }
                if (Data[p][^1] == '#') { SideValues[1] += b; SideValues[5] += b2; }
                if (Data[^1][p] == '#') { SideValues[2] += b; SideValues[6] += b2; }
                if (Data[p][0] == '#') { SideValues[3] += b; SideValues[7] += b2; }
                b <<= 1; b2 >>= 1;
            }
        }
    }
}
