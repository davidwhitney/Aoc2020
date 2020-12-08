using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020
{
    public class Day8Vm
    {
        public MemoryState Memory { get; set; } = new();
        
        public ExitCode Execute(IEnumerable<OpCode> opCodes)
        {
            var programCounter = 0;
            var program = new List<OpCode>(opCodes);
            var executed = new List<OpCode>();

            while (true)
            {
                if (programCounter >= program.Count)
                {
                    return ExitCode.Success;
                }

                var opCode = program[programCounter];
                if (executed.Contains(opCode))
                {
                    return ExitCode.StackOverflow;
                }

                var movement = opCode.Execute(Memory);

                programCounter += movement.Offset;
                executed.Add(opCode);
            }
        }
    }

    public static class Compiler
    {
        public static List<OpCode> Compile(IEnumerable<string> opCode) 
            => opCode.Select(Compile).ToList();

        public static OpCode Compile(string opCode) =>
            opCode.Split(" ").First() switch
            {
                "acc" => new AccOpCode(opCode),
                "jmp" => new JmpOpCode(opCode),
                "nop" => new NopOpCode(opCode),
                _ => throw new Exception("Invalid OpCode")
            };
    }

    public abstract class OpCode
    {
        public int Value { get; set; }
        public abstract OpCodeResult Execute(MemoryState memory);
        protected OpCode(int value) => Value = value;
    }

    public class JmpOpCode : OpCode
    {
        public JmpOpCode(int value): base(value) { }
        public JmpOpCode(string text): this(int.Parse(text.Split(' ')[1])) { }
        public override OpCodeResult Execute(MemoryState memory) => OpCodeResult.Jump(Value);
    }

    public class AccOpCode : OpCode
    {
        public AccOpCode(int value) : base(value) { }
        public AccOpCode(string text) : this(int.Parse(text.Split(' ')[1])) { }

        public override OpCodeResult Execute(MemoryState memory)
        {
            memory.Accumulator += Value;
            return OpCodeResult.MoveNext();
        }
    }

    public class NopOpCode : OpCode
    {
        public NopOpCode(int value = 0) : base(value) { }
        public NopOpCode(string text) : this(int.Parse(text.Split(' ')[1])) { }
        public override OpCodeResult Execute(MemoryState memory) => OpCodeResult.MoveNext();
    }

    public class OpCodeResult
    {
        public int Offset { get; }
        public OpCodeResult(int offset) => Offset = offset;
        public static OpCodeResult MoveNext() => new(1);
        public static OpCodeResult Jump(int delta) => new(delta);
    }

    public class MemoryState :  Dictionary<string, int>
    {
        public int Accumulator
        {
            get => this[nameof(Accumulator).ToLower()];
            set => this[nameof(Accumulator).ToLower()] = value;
        }

        public MemoryState() => Accumulator = 0;
    }

    public enum ExitCode
    {
        StackOverflow = -1,
        Success = 0,
    }
}