using System;
using System.IO;

namespace Day18
{
    internal class AocDay18
    {
        private static void Main()
        {
            var input = File.ReadAllLines("input.txt");
            long sum1 = 0;
            ulong sum2 = 0;
            foreach (var expression in input)
            {
                var index = 0;
                var r1 = EvaluatePart1(expression, ref index);
                sum1 += r1;

                index = 0;
                var r2 = Eval2(expression, ref index);
                sum2 += r2;
            }
            Console.WriteLine($"sum of answers 1 = {sum1}");
            Console.WriteLine($"sum of answers 2 = {sum2}");
        }

        private static long EvaluatePart1(string expression, ref int index)
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
                            switch (o)
                            {
                                case '+':
                                    a +=n;
                                    break;
                                case '*':
                                    a *= n;
                                    break;
                            }
                            n = null;
                            o = null;
                        }
                        if (c == ')')
                            // ReSharper disable once PossibleInvalidOperationException
                            return (a.Value);
                        break;
                    case '(':
                        n = EvaluatePart1(expression, ref index);
                        break;
                    case '+':
                    case '*':
                        o = c;
                        break;
                }
            }
            // should not happen
            return (-1);
        }

        private static ulong Eval2(string expression, ref int index)
        {
            // get first value
            var acc = NextVal(expression, ref index);

            while (true)
            {
                if (index >= expression.Length) return acc;

                var c = expression[index];

                if (c == ')') return acc;

                index++;
                switch (c)
                {
                    case '+':
                        acc += NextVal(expression, ref index);
                        break;
                    case '*':
                        acc *= Eval2(expression, ref index);
                        break;
                }
            }
        }

        private static ulong NextVal(string expression, ref int index)
        {
            ulong val = 0;
            while (index < expression.Length)
            {
                var c = expression[index];

                switch (c)
                {
                    case ' ':
                        index++;
                        break;
                    case '(':
                    {
                        index++;
                        val = Eval2(expression, ref index);
                        // skip closing brace
                        index++;
                        return val;
                    }
                    default:
                    {
                        if (c >= '0' && c <= '9')
                        {
                            index++;
                            val = val * 10 + c - '0';
                            if (index>=expression.Length || expression[index] < '0' || expression[index] > '9') return val; 
                        }
                        break;
                    }
                }
            }
            return val;
        }
    }
}
