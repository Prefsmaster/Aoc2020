using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day04
{
    internal class AocDay4
    {
        private static void Main()
        {
            var passports = ReadInput(@"input.txt");

            Console.WriteLine($"Valid Passports: {passports.Count(p => p.IsValidRule1())}");
            Console.WriteLine($"Valid Passports: {passports.Count(p => p.IsValidRule1() && p.FieldsAreValid())}");

        }

        private static List<PassportData> ReadInput(string filename)
        {
            var file = new StreamReader(filename);

            string line;
            var passportLines = new List<string>();
            var passports = new List<PassportData>();

            do
            {
                line = file.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    passports.Add(new PassportData(passportLines));
                    passportLines.Clear();
                }
                else
                {
                    passportLines.Add(line);
                }
            } while (line != null);
            
            return passports;
        }
        private class PassportData
        {
            private Dictionary<string, string> Fields { get; }
            public PassportData(List<string> input)
            {
                Fields = new Dictionary<string, string>();
                foreach (var line in input)
                {
                    var pairs = line.Split(" ");
                    foreach (var pair in pairs)
                    {
                        var members = pair.Split(":");
                        Fields.Add(members[0],members[1]);
                    }
                }
            }

            public bool IsValidRule1()
            {
                // all fields, or only CID is missing
                if (Fields.Count == 8) return true;
                return Fields.Count == 7 && !Fields.ContainsKey("cid");
            }

            public bool FieldsAreValid()
            {
                // byr is an int, and [1920..2002]
                if (int.TryParse(Fields["byr"], out var byr))
                {
                    if (byr < 1920 || byr > 2002)
                        return false;
                }
                else
                    return false;

                // iyr is an int, and [2010..2020]
                if (int.TryParse(Fields["iyr"], out var iyr))
                {
                    if (iyr < 2010 || iyr > 2020)
                        return false;
                }
                else
                    return false;

                // eyr is an int, and [2020..2030]
                if (int.TryParse(Fields["eyr"], out var eyr))
                {
                    if (eyr < 2020 || eyr > 2030)
                        return false;
                }
                else
                    return false;

                // Height
                var hgt = Fields["hgt"];
                if (hgt.EndsWith("cm"))
                {
                    var len = hgt[..^2]; 
                    
                    if (int.TryParse(len, out var units))
                    {
                        if (units < 150 || units > 193)
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (hgt.EndsWith("in"))
                {
                    var len = hgt[..^2];
                    if (int.TryParse(len, out var units))
                    {
                        if (units < 59 || units > 76)
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else return false;
 
                // Hair color
                var hcl = Fields["hcl"];
                if (hcl.Length != 7) return false;
                if (hcl.StartsWith("#"))
                {
                    var hexString = hcl[1..];

                    if (hexString.Any(c => !"0123456789abcdef".Contains(c)))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                // Eye color
                if (!new List<string> {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(Fields["ecl"]))
                    return false;

                // Passport ID = 9 digits
                var pid = Fields["pid"];

                return pid.Length == 9 && pid.All(c => "0123456789".Contains(c));
            }
        }
    }
}
