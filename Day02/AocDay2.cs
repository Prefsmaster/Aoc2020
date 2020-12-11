using System;
using System.IO;
using System.Linq;

namespace Day02
{
    internal class AocDay2
    {
        private static void Main()
        {
            var passwords= File.ReadAllLines(@"input.txt").Select(s => new PasswordData(s)).ToArray();
                
            Console.WriteLine($"Correct Passwords rule 1: {passwords.Count(p => p.IsValidPart1())}");
            Console.WriteLine($"Correct Passwords rule 2: {passwords.Count(p => p.IsValidPart2())}");
        }

        private class PasswordData
        {
            private int MinOccurrence { get; }
            private int MaxOccurrence { get; }
            private char CriticalCharacter { get; }
            private string PassWord { get; }

            public PasswordData(string inputData)
            {
                var parts = inputData.Split(" ");
                var minMax = parts[0].Split("-");

                MinOccurrence = int.Parse(minMax[0]);
                MaxOccurrence = int.Parse(minMax[1]);
                CriticalCharacter = parts[1][0];
                PassWord = parts[2];
            }

            public bool IsValidPart1()
            {
                var freq = PassWord.Count(c => c == CriticalCharacter);
                return freq >= MinOccurrence && freq <= MaxOccurrence;
            }
            public bool IsValidPart2()
            {
                return PassWord[MinOccurrence-1]==CriticalCharacter ^ PassWord[MaxOccurrence-1]==CriticalCharacter;
            }
        }
    }
}
