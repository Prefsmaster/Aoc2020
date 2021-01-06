using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day19
{
    internal class AocDay19
    {
        private static List<Rule> _rules;
        private static List<string> _messages;

        private static void Main()
        {
            for (var part = 1; part < 3; part++)
            {
                _rules = new List<Rule>();
                _messages = new List<string>();

                var input = File.ReadAllLines($"input{part}.txt");
                foreach (var l in input)
                {
                    if (string.IsNullOrEmpty(l)) continue;
                    if (l[0] >= '0' && l[0] <= '9')
                    {
                        _rules.Add(new Rule(l));
                    }
                    else
                    {
                        _messages.Add(l);
                    }
                }

                _rules = _rules.OrderBy(r => r.Id).ToList();

                Console.WriteLine($"Part {part} has {_rules.Count} rules and {_messages.Count} messages");

                var expression = GetExpressionForPart1(_rules[0]);
                Console.WriteLine($"expr: {expression}");

                var regex = new Regex($"^{expression}$");
                var count = 0;

                foreach (var s in _messages.Where(s => regex.IsMatch(s)))
                {
                    Console.WriteLine(s);
                    count++;
                }

                Console.WriteLine($"Part {part}: {count} messages match");
            }
        }

        private static string GetExpressionForPart1(Rule r)
        {
            const string open = "(";
            const string cat = "|";
            const string close = ")";
            if (r.IsLeaf) return new string(r.Letter,1);

            var sb = new StringBuilder();
            sb.Append(open);
            for (var childSetIndex = 0; childSetIndex < r.ChildIds.Count; childSetIndex++)
            {
                for (var index = 0; index < r.ChildIds[childSetIndex].Count; index++)
                {
                    var cId = r.ChildIds[childSetIndex][index];
                    sb.Append(GetExpressionForPart1(_rules[cId]));
                }
                if (childSetIndex != (r.ChildIds.Count - 1)) sb.Append(cat);
            }
            sb.Append(close);
            return sb.ToString();
        }
    }
}
