using System.IO;
using NUnit.Framework;

namespace Aoc2020
{
    [TestFixture]
    public class Day7Tests
    {
        [Test]
        public void Parse_RuleDependOnNoOtherBagTypes_ReturnsEmptyRequirements()
        {
            var result = Day7Answers.Parse("dotted black bags contain no other bags.");

            Assert.That(result.Key, Is.EqualTo("dotted black"));
            Assert.That(result.Value.Count, Is.EqualTo(0));
        }

        [Test]
        public void Parse_RuleDependOnOneTypeOfBag_ReturnsOneRequirement()
        {
            var result = Day7Answers.Parse("light red bags contain 1 bright white bag.");

            Assert.That(result.Key, Is.EqualTo("light red"));

            Assert.That(result.Value[0].Target, Is.EqualTo("bright white"));
            Assert.That(result.Value[0].Minimum, Is.EqualTo(1));
        }

        [Test]
        public void Parse_RuleDependOnMultipleTypesOfBag_ReturnsOneRequirement()
        {
            var result = Day7Answers.Parse("light red bags contain 1 bright white bag, 2 muted yellow bags.");

            Assert.That(result.Key, Is.EqualTo("light red"));

            Assert.That(result.Value[0].Target, Is.EqualTo("bright white"));
            Assert.That(result.Value[0].Minimum, Is.EqualTo(1));

            Assert.That(result.Value[1].Target, Is.EqualTo("muted yellow"));
            Assert.That(result.Value[1].Minimum, Is.EqualTo(2));
        }

        [Test]
        public void Parse_MultipleRules_BothParsed()
        {
            var result = Day7Answers.Parse(new []
            {
                "dotted black bags contain no other bags.",
                "light red bags contain 1 bright white bag."
            });

            Assert.That(result.Count, Is.EqualTo(2));
        }
    }

    [TestFixture]
    public class PolicyTests
    {
        private Policy _examplePolicy;
        private string[] _exampleInput;

        [SetUp]
        public void SetUp()
        {
            _exampleInput = new[]
            {
                "light red bags contain 1 bright white bag, 2 muted yellow bags.",
                "dark orange bags contain 3 bright white bags, 4 muted yellow bags.",
                "bright white bags contain 1 shiny gold bag.",
                "muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.",
                "shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.",
                "dark olive bags contain 3 faded blue bags, 4 dotted black bags.",
                "vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.",
                "faded blue bags contain no other bags.",
                "dotted black bags contain no other bags."
            };

            _examplePolicy = Day7Answers.Parse(_exampleInput);
        }

        [Test]
        public void Example1()
        {
            var result = _examplePolicy.CountCanContain("shiny gold");
            var totalBags = _examplePolicy.SumContents("shiny gold");

            Assert.That(result, Is.EqualTo(4));
            Assert.That(totalBags, Is.EqualTo(32));
        }

        [Test]
        public void Part1()
        {
            var inputs = File.ReadAllLines("Day7.txt");
            var policy = Day7Answers.Parse(inputs);

            var result = policy.CountCanContain("shiny gold");
            var totalBags = policy.SumContents("shiny gold");

            Assert.That(result, Is.EqualTo(128));
            Assert.That(totalBags, Is.EqualTo(20189));
        }

        [Test]
        public void AllRequirementsOf_ReturnsRecursiveSetOfRequirements()
        {
            var requirements = _examplePolicy.AllRequirementsOf("bright white");

            Assert.That(requirements.Count, Is.EqualTo(7));
            Assert.That(requirements[0].Target, Is.EqualTo("shiny gold"));
        }
    }
}