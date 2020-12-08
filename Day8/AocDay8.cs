using System;
using System.Collections.Generic;
using System.IO;

namespace Day8
{
    internal class AocDay8
    {
        private static void Main()
        {
            bool loops;
            int pc;
            int acc;

            var instructions = ReadData("input.txt");

            // Part 1
            (loops, pc, acc) = TestProgram(instructions);
            Console.WriteLine(loops
                ? $"Program loops from PC {pc} when ACC is {acc}"
                : $"Program out of bounds with PC at {pc} and ACC is {acc}");

            // Part 2, brute forced
            foreach (var i in instructions)
            {
                switch (i.OpCode)
                {
                    case "acc":
                        continue;
                    case "jmp":
                        i.OpCode = "nop";
                        (loops, pc, acc) = TestProgram(instructions);
                        i.OpCode = "jmp";
                        break;
                    case "nop":
                        i.OpCode = "jmp";
                        (loops, pc, acc) = TestProgram(instructions);
                        i.OpCode = "nop";
                        break;
                }
                if (!loops && pc == instructions.Length)
                    break;
            }

            Console.WriteLine($"Program counter at {pc} is out of bounds, with ACC at {acc}.");
        }

        private static (bool loops, int pc, int acc) TestProgram(Instruction[] instructions)
        {
            foreach (var i in instructions)
                i.Visited = false;

            var pc = 0;
            var acc = 0;
            while (!instructions[pc].Visited)
            {
                var i = instructions[pc];
                i.Visited = true;
                switch (i.OpCode)
                {
                    case "nop":
                        pc++;
                        break;
                    case "acc":
                        acc += i.Operand;
                        pc++;
                        break;
                    case "jmp":
                        pc += i.Operand;
                        break;
                    default:
                        throw new Exception("WUT?!!");
                }
                if (pc < 0 || pc >= instructions.Length)
                {
                    return (false, pc, acc);
                }
            }
            return (true, pc, acc);
        }
        private static Instruction[] ReadData(string filename)
        {
            var instructions = new List<Instruction>();

            var file = new StreamReader(filename);
            string line;
            do
            {
                line = file.ReadLine();
                
                if (!string.IsNullOrEmpty(line))
                    instructions.Add(new Instruction(line));
            } while (line != null);

            return instructions.ToArray();
        }


        internal class Instruction
        {
            public Instruction(string assemblyCode)
            {
                var parts = assemblyCode.Split();
                OpCode = parts[0];
                Operand = int.Parse(parts[1]);
            }
            public string OpCode { get; set; }
            public int Operand { get; }
            public bool Visited { get; set;}
        }
    }
}
