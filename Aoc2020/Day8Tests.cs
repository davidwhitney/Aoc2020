using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;

namespace Aoc2020
{
    [TestFixture]
    public class Day8Tests
    {
        [Test]
        public void Example()
        {
            var program = new Compiler().Compile(new[]
            {
                "nop +0", "acc +1", "jmp +4", "acc +3", "jmp -3", "acc -99", "acc +1", "jmp -4", "acc +6"
            });

            var sut = new Day8Vm();
            var result = sut.Execute(program);

            Assert.That(result, Is.EqualTo(-1));
            Assert.That(sut.Memory.Accumulator, Is.EqualTo(5));
        }

        [Test]
        public void Part1()
        {
            var texts = File.ReadAllLines("Day8.txt").Select(s => s.Trim());
            var program = new Compiler().Compile(texts);

            var sut = new Day8Vm();
            var result = sut.Execute(program);

            Assert.That(result, Is.EqualTo(-1));
            Assert.That(sut.Memory.Accumulator, Is.EqualTo(1489));
        }
    }

    [TestFixture]
    public class CompilerTests
    {
        private Compiler _sut;

        [SetUp] public void SetUp() => _sut = new Compiler();

        [Test]
        public void Compile_AccPositive_ReturnsAccOpCode()
        {
            var result = _sut.Compile("acc +1");

            Assert.That(result, Is.TypeOf<AccOpCode>());
            Assert.That(((AccOpCode)result).Value, Is.EqualTo(1));
        }

        [Test]
        public void Compile_AccNegative_ReturnsAccOpCode()
        {
            var result = _sut.Compile("acc -1");

            Assert.That(result, Is.TypeOf<AccOpCode>());
            Assert.That(((AccOpCode)result).Value, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class AccOpCodeTests : OpCodeTestBase
    {
        [Test]
        public void Execute_AccWithPositiveNumber_IncrementsAccumulator()
        {
            var sut = new AccOpCode(1);
            sut.Execute(Memory);

            Assert.That(Memory.Accumulator, Is.EqualTo(1));
        }

        [Test]
        public void Execute_AccWithNegativeNumber_DecrementsAccumulator()
        {
            var sut = new AccOpCode(-1);
            sut.Execute(Memory);

            Assert.That(Memory.Accumulator, Is.EqualTo(-1));
        }
    }

    [TestFixture]
    public class NopOpCodeTests : OpCodeTestBase
    {
        [Test]
        public void Execute_NoOp_DoesNothing()
        {
            var sut = new NopOpCode();
            sut.Execute(Memory);

            Assert.That(Memory, Is.EqualTo(new MemoryState()));
        }
    }

    [TestFixture]
    public abstract class OpCodeTestBase
    {
        protected MemoryState Memory;

        [SetUp]
        public void SetUp()
        {
            Memory = new MemoryState();
        }
    }
}