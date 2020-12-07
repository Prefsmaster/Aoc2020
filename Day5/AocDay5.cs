using System;
using System.IO;

namespace Day5
{
    internal class AocDay5
    {
        private static void Main()
        {
            var file = new StreamReader("input.txt");

            var maxId = 0;
            string  line;

            const int bits = 10;
            const int maxSeats = 1 << bits;

            var occupiedSeats = new bool[maxSeats];

            while((line = file.ReadLine()) != null)
            {
                var id = 0;
                foreach (var c in line)
                {
                    id *= 2;
                    if (c == 'B' || c=='R') id += 1;
                }
                
                if (id>maxId) maxId = id;

                occupiedSeats[id] = true;

            }
            Console.WriteLine($"Largest Id: {maxId}");

            // check seats <0..1023> (1..1022) and neighbors 
            for (var seat = 1; seat < maxSeats-1; seat++)
            {
                if (!occupiedSeats[seat] && occupiedSeats[seat - 1] && occupiedSeats[seat + 1])
                    Console.WriteLine($"My seat id is {seat}");
            }
        }
    }
}
