using System.Collections.Generic;
using System.Text;

namespace Day19
{
    class Rule
    {
        public List<List<int>> ChildIds { get; }
        public int Id { get; }
        public char Letter { get; }

        public Rule(string input)
        {
            var parts = input.Split(':');
            Id = int.Parse(parts[0]);

            ChildIds = new List<List<int>>();
            if (parts[1][1] == '"') Letter = parts[1][2];
            else
            {
                var children = parts[1][1..].Split();
                var childSet = new List<int>();
                ChildIds.Add(childSet);
                foreach (var c in children)
                {
                    if (c.StartsWith('|')) 
                    {
                        childSet = new List<int>();
                        ChildIds.Add(childSet);
                    }
                    else
                    {
                        childSet.Add(int.Parse(c));
                    }
                }
            }
        }

        internal bool IsLeaf => ChildIds.Count == 0;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{Id}:");
            if (IsLeaf)
            {
                sb.Append($" \"{Letter}\"");
            }
            else
            {
                for (var childSetIndex = 0; childSetIndex < ChildIds.Count; childSetIndex++)
                {
                    foreach (var cId in ChildIds[childSetIndex]) sb.Append($" {cId}");
                    if (childSetIndex != (ChildIds.Count - 1)) sb.Append(" |");
                }
            }
            return sb.ToString();
        }
    }
}