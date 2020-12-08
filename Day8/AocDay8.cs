using System;
using System.IO;
using System.Linq;

namespace Day8
{
    internal class AocDay8
    {
        private static void Main()
        {
            var instructions = File.ReadAllLines("input.txt").Select(l => new Instruction(l)).ToArray();

            bool loops;
            int pc;
            int acc;

            // Part 1
            (loops, pc, acc) = TestProgram(instructions);
            Console.WriteLine(loops
                ? $"Program loops. PC = {pc}, ACC = {acc}"
                : $"Program out of bounds. PC = {pc}, ACC = {acc}");

            // Part 2, brute forced
            foreach (var i in instructions.Where(o => o.OpCode!="acc"))
            {
                switch (i.OpCode)
                {
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

            Console.WriteLine($"Program counter at {pc} is out of bounds, ACC = {acc}.");
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
