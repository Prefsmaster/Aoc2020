using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    internal class AocDay16
    {
        private static void Main()
        {
            var input = File.ReadAllLines("input.txt");

            var fields = GetFields(input);

            Part1(input, fields);
            Part2(input, fields);
        }

        private static void Part1(string[] input, List<Field> fields)
        {
            var line = fields.Count + 5;
            var answer = 0;
            while (line<input.Length)
            {
                var values = input[line++].Split(',').Select(int.Parse).ToArray();
                answer += values.Where(v => !fields.Any(f => f.IsValid(v))).Sum();
            }
            Console.WriteLine(answer);
        }

        private static List<Field> GetFields(string[] input)
        {
            var line = 0;
            var fields = new List<Field>();
            while (!string.IsNullOrEmpty(input[line]))
            {
                fields.Add(new Field(input[line]));
                line++;
            }
            return fields;
        }

        private static void Part2(string[] input, List<Field> fields)
        {
            var nFields = fields.Count;

            var line = nFields + 2;
            var myTicketValues = input[line].Split(',').Select(int.Parse).ToArray();

            line += 3;
            var validValues = new List<int[]>();
            while (line<input.Length)
            {
                var values = input[line++].Split(',').Select(int.Parse).ToArray();
                if (values.All(v=> fields.Any(f => f.IsValid(v))))
                    validValues.Add(values);
            }
            var nValidTickets = validValues.Count;

            // we have all valid tickets values now.

            // make a transposed matrix I am convinced there is a better way.
            // allocate
            var validInputs = new int[nFields][];
            for (var vi = 0; vi<nFields;vi++)
            {
                validInputs[vi] = new int[nValidTickets];
            }
            // fill
            for (var t = 0; t < nValidTickets; t++)
                for (var f = 0; f < nFields; f++)
                    validInputs[f][t] = validValues[t][f];

 
            // now find a row of values that are all valid for only 1 field.
            // do this nFields times and remember the row index.

            var fieldIndex = new int[nFields];
            Array.Fill(fieldIndex,-1);

            var rowAssigned = new bool[nFields];
            var fieldsFitted = 0;
            // need to fit nFields
            while (fieldsFitted < nFields)
            {
                for (var r = 0; r < nFields; r++)
                {
                    if (rowAssigned[r]) continue;   // only do rows that were not assigned yet!

                    var fitsFields = 0;
                    var matchingField = -1;

                    for (var f = 0; f < nFields && fitsFields<2; f++)
                    {
                        // if field already has an index, or a value is not valid, this field does
                        // not match the current row
                        if (fieldIndex[f] != -1 || !fields[f].IsValid(validInputs[r])) continue;

                        fitsFields++;
                        matchingField = f;
                    }
                    // if the row doesn't match exactly one field, keep searching
                    if (fitsFields != 1) continue;
                    // flag row as assigned
                    rowAssigned[r] = true;
                    // save index (flag field as assigned)
                    fieldIndex[matchingField] = r;
                    // one more done
                    fieldsFitted++;
                }
            }
            // now we know the index of each field.
            // find the ones that start with departure!
            // optimization: can be incorporated in the main loop above.
            long answer = 1;
            for (var f = 0; f < nFields; f++)
            {
                if (fields[f].Name.StartsWith("departure"))
                {
                    answer *= myTicketValues[fieldIndex[f]];
                }
            }
            Console.WriteLine(answer);
        }
    }

    internal class Field
    {
        public string Name { get; }
        private readonly int[][] _validRanges;

        public Field(string initString)
        {
            Name = initString.Split(':')[0];
            var ranges = initString.Split(':')[1].Split(" or ");
            _validRanges = new int[ranges.Length][];
            var ri = 0;
            foreach (var r in ranges)
            {
                var delimiters = r.Split('-');
                var start = int.Parse(delimiters[0]);
                _validRanges[ri++] = Enumerable.Range(start,int.Parse(delimiters[1])-start+1).ToArray();
            }
        }
        public bool IsValid(int value)
        {
            return _validRanges.Any(r => r.Contains(value));
        }
        public bool IsValid(int[] values)
        {
            return values.All(IsValid);
        }
    }
}
