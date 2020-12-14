using System;
using System.IO;

namespace Day12
{
    internal class AocDay12
    {
        private static void Main()
        {
            var commands = File.ReadAllLines("input.txt");
            Part1(commands);
            Part2(commands);
        }

        private static void Part1(string[] commands)
        {
            var x = 0;
            var y = 0;

            var dx = new [] {1, 0, -1, 0};
            var dy = new []{0, -1, 0, 1};
            var heading = 0;

            foreach (var line in commands)
            {
                var command = line[0];
                var operand = int.Parse(line[1..]);
                switch (command)
                {
                    case 'N':
                        y += operand;
                        break;
                    case 'E':
                        x += operand;
                        break;
                    case 'S':
                        y -= operand;
                        break;
                    case 'W':
                        x -= operand;
                        break;
                    case 'F':
                        x += dx[heading] * operand;
                        y += dy[heading] * operand;
                        break;
                    case 'R':
                        heading = (heading + operand / 90)%4;
                        break;
                    case 'L':
                        // three times right == left :-)
                        heading = (heading + 3 * operand / 90)%4;
                        break;
                }
            }
            Console.WriteLine(Math.Abs(x)+Math.Abs(y));
        }

        private static void Part2(string[] commands)
        {
            
            var x = 0;
            var y = 0;

            var dx = 10;
            var dy = 1;

            foreach (var line in commands)
            {
                var command = line[0];
                var operand = int.Parse(line[1..]);
                switch (command)
                {
                    case 'N':
                        dy += operand;
                        break;
                    case 'E':
                        dx += operand;
                        break;
                    case 'S':
                        dy -= operand;
                        break;
                    case 'W':
                        dx -= operand;
                        break;
                    case 'F':
                        x += dx * operand;
                        y += dy * operand;
                        break;
                    case 'R':
                        while (operand > 0)
                        {
                            var t = dx;
                            dx = dy;
                            dy = -t;
                            operand -= 90;
                        }
                        break;
                    case 'L':
                        while (operand > 0)
                        {
                            var t = dx;
                            dx = -dy;
                            dy = t;
                            operand -= 90;
                        }
                        break;
                }
            }
            Console.WriteLine(Math.Abs(x)+Math.Abs(y));
        }
    }
}
