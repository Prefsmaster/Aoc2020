using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    internal class AocDay7
    {
        static Dictionary<string, List<Bag>> _allBags;
        private static void Main()
        {
            const string wantedBagColor = "shiny gold";

            _allBags = ReadData("input.txt");
            Console.WriteLine($"{_allBags.Count} bags total.");

            // Part 1, test all bags to see if they contain the wanted one somewhere.
            var count = 0;
            foreach (var b in _allBags.Keys)
            {
                if (BagCanContain(b, wantedBagColor)) count++;
            }
            Console.WriteLine($"{count} bags must contain one or more {wantedBagColor} bags.");

            // Part 2
            Console.WriteLine($"{BagContains(wantedBagColor)} bags contained in 1 {wantedBagColor} bag.");
        }

        private static Dictionary<string, List<Bag>> ReadData(string filename)
        {
            var bags = new Dictionary<string, List<Bag>>();

            var file = new StreamReader(filename);
            string line;
            do
            {
                line = file.ReadLine();
                
                if (string.IsNullOrEmpty(line)) continue;

                // process line, split into words
                // example: mirrored olive bags contain    --> keep 1st 2 words, ignore 'bags contain'
                //           4 bright orange bags,         --> keep number and 2 color words, dump trailing 'bag' variation
                //           5 dim silver bags,                     ''
                //           1 wavy tan bag,                        ''
                //           5 striped crimson bags.                ''  
                //                                             until end of parts is reached
                // OR:
                // example: muted crimson bags contain     --> keep 1st 2 words, dump next 2
                //           no other bags.                -->  terminate on 'no'

                var parts = line.Split();
                var p = 0;

                var baseBagName = parts[p++] + " " + parts[p++];
                p += 2; // skip over  'bags contain'
                    
                var containsBags = new List<Bag>();
 
                while (p < parts.Length && !parts[p].Equals("no"))
                {
                    var numberOfBags = int.Parse(parts[p++]);
                    var subBagName = parts[p++] + " " + parts[p++];
                    p++;
                    containsBags.Add(new Bag{Color = subBagName, Count = numberOfBags});
                }
                
                // bag is complete
                bags.Add(baseBagName, containsBags);
            } while (line != null);

            return bags;
        }

        private static bool BagCanContain(string key, string wantedColor)
        {
            var subBags = _allBags[key];

            if (subBags.Count == 0) return false;

            if (subBags.Any(b => b.Color.Equals(wantedColor))) return true;

            foreach (var b in subBags)
            {
                if (BagCanContain(b.Color, wantedColor)) return true;
            }
            return false;
        }

        private static int BagContains(string bagId)
        {
            var subBags = _allBags[bagId];

            if (subBags.Count == 0) return 0;   // bag contains 0 other bags
            
            // add # of contained bags themselves
            var contentCount = subBags.Sum(b => b.Count);

            // add their contained bags.
            foreach (var b in subBags)
            {
                contentCount += b.Count * (BagContains(b.Color));
            }
            return contentCount;
        }

        private class Bag
        {
            public string Color;
            public int Count;
        }
    }
}
