using System;
using System.IO;
using System.Linq;

namespace Day18
{
    internal class AocDay18
    {
        private static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            var sum = input.Aggregate(0L, (current, i) => current + EvaluatePart1(i, 0).Item1);
            Console.WriteLine($"sum of answers = {sum}");
        }

        private static (long, int) EvaluatePart1(string expression, int index)
        {
            long? a = null;
            long? n = null;
            char? o = null;
            while (index <= expression.Length)
            {
                // hack to force evaluation at end of string
                var c = index == expression.Length ? ')': expression[index++];
                switch (c)
                {
                    case '0': case '1': case '2': case '3': case '4':
                    case '5': case '6': case '7': case '8': case '9':
                        n ??= 0;
                        n = n * 10 + c - '0';
                        break;
                    case ' ':
                    case ')':
                        if (n!= null && a == null && o==null)
                        {
                            a = n;
                            n = null;
                        }
                        if (a!=null && o != null && n!=null)
                        {
                            if (o == '+')
                            {
                                a +=n;
                            }
                            if (o == '*')
                            {
                                a *= n;
                            }
                            n = null;
                            o = null;
                        }
                        if (c == ')')
                            return (a.Value, index);
                        break;
                    case '(':
                        (n, index) = EvaluatePart1(expression, index);
                        break;
                    case '+':
                    case '*':
                        o = c;
                        break;
                }
            }
            return (a.Value, index);
        }
    }
}
