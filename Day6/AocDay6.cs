using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Day6
{
    internal class AocDay6
    {
        private static void Main()
        {
            Part1("input.txt");

            Part2("input.txt");
        }

        private static void Part1(string filename)
        {
            var file = new StreamReader(filename);

            string line;
            var groupAnswers = new StringBuilder();

            var total = 0;
            do
            {
                line = file.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    total += groupAnswers.ToString().Distinct().Count();
                    groupAnswers.Clear();
                }
                else
                {
                    groupAnswers.Append(line);
                }
            } while (line != null);

            Console.WriteLine($"total is: {total}");
        }
        private static void Part2(string filename)
        {
            var file = new StreamReader(filename);

            var total = 0;
            var counts = new int[26];
            var groups = 0;

            string line;
            do
            {
                line = file.ReadLine();

                if (string.IsNullOrEmpty(line))
                {
                    total += counts.Count(c => c == groups);
                    Array.Clear(counts, 0, 26);
                    groups = 0;
                }
                else
                {
                    groups++;
                    foreach (var c in line)
                    {
                        counts[c - 'a']++;
                    }
                }
            } while (line != null);

            Console.WriteLine($"total is: {total}");
        }
    }
}
