using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020
{
    public class Day8Vm
    {
        public MemoryState Memory { get; set; } = new();
        
        public int Execute(IEnumerable<IOpCode> opCodes)
        {
            var programCounter = 0;
            var program = new List<IOpCode>(opCodes);
            var executed = new List<IOpCode>();

            while (true)
            {
                if (programCounter >= program.Count)
                {
                    return 0;
                }

                var opCode = program[programCounter];
                if (executed.Contains(opCode))
                {
                    return -1;
                }

                var movement = opCode.Execute(Memory);

                programCounter += movement.Offset;
                executed.Add(opCode);
            }
        }
    }

    public class Compiler
    {
        public List<IOpCode> Compile(IEnumerable<string> opCode) 
            => opCode.Select(Compile).ToList();

        public IOpCode Compile(string opCode)
        {
            if (opCode.StartsWith("acc"))
            {
                return AccOpCode.Parse(opCode);
            }
            if (opCode.StartsWith("jmp"))
            {
                return JmpOpCode.Parse(opCode);
            }
            if (opCode.StartsWith("nop"))
            {
                return NopOpCode.Parse(opCode);
            }

            throw new Exception("Invalid OpCode");
        }
    }

    public interface IOpCode
    {
        public OpCodeResult Execute(MemoryState memory);
    }

    public class OpCodeResult
    {
        public int Offset { get; }

        public OpCodeResult(int offset)
        {
            Offset = offset;
        }

        public static OpCodeResult MoveNext() => new(1);
        public static OpCodeResult Jump(int delta) => new(delta);
    }

    public class JmpOpCode : IOpCode
    {
        public int Value { get; set; }
        public JmpOpCode(int value) => Value = value;
        public static JmpOpCode Parse(string text) => new(int.Parse(text.Split(' ')[1]));

        public OpCodeResult Execute(MemoryState memory)
        {
            return OpCodeResult.Jump(Value);
        }
    }

    public class AccOpCode : IOpCode
    {
        public int Value { get; set; }
        public AccOpCode(int value) => Value = value;
        public static AccOpCode Parse(string text) => new(int.Parse(text.Split(' ')[1]));

        public OpCodeResult Execute(MemoryState memory)
        {
            memory.Accumulator += Value;
            return OpCodeResult.MoveNext();
        }
    }

    public class NopOpCode : IOpCode
    {
        public static NopOpCode Parse(string text) => new();
        public OpCodeResult Execute(MemoryState memory) => OpCodeResult.MoveNext();
    }

    public class MemoryState :  Dictionary<string, int>
    {
        public int Accumulator
        {
            get => this[nameof(Accumulator).ToLower()];
            set => this[nameof(Accumulator).ToLower()] = value;
        }

        public MemoryState()
        {
            Accumulator = 0;
        }

    }

}