using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Aoc2020
{
    [TestFixture]
    public class Day9Tests 
    {
        [Test]
        public void Example1()
        {
            var preamble = Enumerable.Range(1, 25).Select(i => (long)i).ToList();

            Assert.That(IsValid(25, preamble.Concat(new long[] {26})).Single(kvp => kvp.Item1 == 26).Item2, Is.True);
            Assert.That(IsValid(25, preamble.Concat(new long[] {49})).Single(kvp => kvp.Item1 == 49).Item2, Is.True);
            Assert.That(IsValid(25, preamble.Concat(new long[] {100})).Single(kvp => kvp.Item1 == 100).Item2, Is.False);
            Assert.That(IsValid(25, preamble.Concat(new long[] {50})).Single(kvp => kvp.Item1 == 50).Item2, Is.False);

            var preamble2 = new long[] {20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 21, 22, 23, 24, 25};
            Assert.That(IsValid(25, preamble2.Concat(new long[] {45, 26})).Single(kvp => kvp.Item1 == 26).Item2, Is.True);
            Assert.That(IsValid(25, preamble2.Concat(new long[] {45, 65})).Single(kvp => kvp.Item1 == 65).Item2, Is.False);
            Assert.That(IsValid(25, preamble2.Concat(new long[] {45, 64})).Single(kvp => kvp.Item1 == 64).Item2, Is.True);
            Assert.That(IsValid(25, preamble2.Concat(new long[] {45, 66})).Single(kvp => kvp.Item1 == 66).Item2, Is.True);
            
            // Larger example

            var preamble3 = new long[]
            {
                35,
                20,
                15,
                25,
                47,
                40,
                62,
                55,
                65,
                95,
                102,
                117,
                150,
                182,
                127,
                219,
                299,
                277,
                309,
                576
            };


            var isValid = IsValid(5, preamble3).Single(kvp => kvp.Item1 == 127).Item2;
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void Part1()
        {
            var values = File.ReadAllLines("Day9.txt").Select(long.Parse).ToList();

            var isValid = IsValid(25, values);
            var firstInvalid = isValid.First(kvp => !kvp.Item2);
            
            Assert.That(firstInvalid.Item1, Is.EqualTo(373803594));
        }

        [Test]
        public void Example2()
        {
            var numbers = new long[]
            {
                35,
                20,
                15,
                25,
                47,
                40,
                62,
                55,
                65,
                95,
                102,
                117,
                150,
                182,
                127,
                219,
                299,
                277,
                309,
                576
            }.ToList();

            var result = FindNumbersThatTotalTarget(numbers, 127);
            result.Sort();
            
            Assert.That(result.First(), Is.EqualTo(15));
            Assert.That(result.Last(), Is.EqualTo(47));
            Assert.That(result.First() + result.Last(), Is.EqualTo(62));
        }

        [Test]
        public void Part2()
        {
            var values = File.ReadAllLines("Day9.txt").Select(long.Parse).ToList();
            
            var result = FindNumbersThatTotalTarget(values, 373803594);
            result.Sort();
            
            Assert.That(result.First() + result.Last(), Is.EqualTo(51152360));
        }

        public static List<(long, bool)> IsValid(int preambleLength, IEnumerable<long> numbers)
        {
            var resolvedNumbers = numbers.ToList();
            var numbersToProcess = new Queue<long>(resolvedNumbers.Skip(preambleLength));

            var results = new List<(long, bool)>();
            var validationRangeStart = 0;

            while (numbersToProcess.Count > 0)
            {
                var number = numbersToProcess.Dequeue();
                var validationSet = resolvedNumbers.Skip(validationRangeStart).Take(preambleLength).ToList();
                
                var set1 = new List<long>(validationSet);
                var set2 = new List<long>(validationSet);

                var isValid = set1.Any(s1 => set2.Except(new[] {s1}).Any(s2 => s1 + s2 == number));
                
                results.Add((number, isValid));
                validationRangeStart++;
            }

            return results;
        }

        public static List<long> FindNumbersThatTotalTarget(List<long> range, long target)
        {
            for (var startPos = 0; startPos < range.Count; startPos++)
            {
                var slice = range.Skip(startPos).ToList();

                for (var i = 0; i < slice.Count; i++)
                {
                    var subset = slice.Take(i).ToList();
                    var sum = subset.Sum();
                    if (sum == target)
                    {
                        return subset;
                    }
                }
            }
            
            return new List<long>();
        }
    }
}