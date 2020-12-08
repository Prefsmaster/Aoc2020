using System;
using System.Collections.Generic;
using System.IO;

namespace Day8
{
    internal class AocDay8
    {
        private static void Main()
        {
            var instructions = ReadData("input.txt");

            // Part 1
            TestProgram(instructions);

            // Part 2, brute forced
            foreach (var i in instructions)
            {
                if (i.OpCode == "jmp")
                {
                    i.OpCode = "nop";
                    if (TestProgram(instructions) == instructions.Length)
                    {
                        break;
                    }
                    i.OpCode = "jmp";
                }

                if (i.OpCode == "nop")
                {

                    i.OpCode = "jmp";
                    if (TestProgram(instructions) == instructions.Length)
                    {
                        break;
                    }

                    i.OpCode = "nop";
                }
            }
        }

        private static int TestProgram(Instruction[] instructions)
        {
            // returns -1 when it loops, otherwise the PC value that is out of bounds.
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
                    Console.WriteLine($"Program counter {pc} is out of bounds with ACC at {acc}.");
                    return pc;
                }
            }
            Console.WriteLine($"Program loops from PC {pc} when ACC is {acc}");
            return 0;
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
                Visited = false;
            }
            public string OpCode { get; set; }
            public int Operand { get; }
            public bool Visited { get; set;}
        }
    }
}
