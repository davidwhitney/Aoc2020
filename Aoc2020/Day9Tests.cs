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

            Assert.That(IsValid(25, preamble.Concat(new long[] {26})).Single(kvp => kvp.Key == 26).Value, Is.True);
            Assert.That(IsValid(25, preamble.Concat(new long[] { 49})).Single(kvp => kvp.Key == 49).Value, Is.True);
            Assert.That(IsValid(25, preamble.Concat(new long[] {100})).Single(kvp => kvp.Key == 100).Value, Is.False);
            Assert.That(IsValid(25, preamble.Concat(new long[] {50})).Single(kvp => kvp.Key == 50).Value, Is.False);

            var preamble2 = new long[] {20, 19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 21, 22, 23, 24, 25};
            Assert.That(IsValid(25, preamble2.Concat(new long[] { 45, 26 })).Single(kvp => kvp.Key == 26).Value, Is.True);
            Assert.That(IsValid(25, preamble2.Concat(new long[] { 45, 65 })).Single(kvp => kvp.Key == 65).Value, Is.False);
            Assert.That(IsValid(25, preamble2.Concat(new long[] { 45, 64 })).Single(kvp => kvp.Key == 64).Value, Is.True);
            Assert.That(IsValid(25, preamble2.Concat(new long[] { 45, 66 })).Single(kvp => kvp.Key == 66).Value, Is.True);
            
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


            var isValid = IsValid(5, preamble3).Single(kvp => kvp.Key == 127).Value;
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void Part1()
        {
            var values = File.ReadAllLines("Day9.txt").Select(long.Parse).ToList();

            var isValid = IsValid(25, values);
            var firstInvalid = isValid.First(kvp => !kvp.Value);
            
            Assert.That(firstInvalid.Key, Is.EqualTo(373803594));
        }

        public List<KeyValuePair<long, bool>> IsValid(int preambleLength, IEnumerable<long> numbers)
        {
            var resolvedNumbers = numbers.ToList();
            var numbersToProcess = new Queue<long>(resolvedNumbers.Skip(preambleLength));

            var results = new List<KeyValuePair<long, bool>>();
            var validationRangeStart = 0;

            while (numbersToProcess.Count > 0)
            {
                var number = numbersToProcess.Dequeue();
                var validationSet = resolvedNumbers.Skip(validationRangeStart).Take(preambleLength).ToList();
                
                var set1 = new List<long>(validationSet);
                var set2 = new List<long>(validationSet);

                var isValid = set1.Any(s1 =>
                {
                    return set2.Except(new[] {s1}).Any(s2 => s1 + s2 == number);
                });

                var result = new KeyValuePair<long, bool>(number, isValid);
                results.Add(result);
                validationRangeStart++;
            }

            return results;
        }
    }
}