using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Aoc2020
{
    [TestFixture]
    public class Day2Tests
    {
        [Test]
        public void PasswordPolicyCtor_UnpacksCorrectly()
        {
            var policy = PasswordPolicy.FromEntry("1-3 a: abcde").ToList();

            Assert.That(policy.First().Value, Is.EqualTo('a'));
            Assert.That(policy.First().MinOccurs, Is.EqualTo(1));
            Assert.That(policy.First().MaxOccurs, Is.EqualTo(3));
        }

        [Test]
        public void PasswordPolicy_IsValid_TrueWhenValid()
        {
            var policy = PasswordPolicy.FromEntry("1-3 a: abcde").ToList();

            var result = policy.First().IsValid("abcde");

            Assert.That(result, Is.True);
        }

        [Test]
        public void Day2Part1()
        {
            var inputText = File.ReadAllLines("Day2Input1.txt");
            var passwordsAndPolicies = inputText.Select(x =>
            {
                var password = x.Split(':')[1].Trim();
                var policy = PasswordPolicy.FromEntry(x).First();
                return new { password, policy };
            });

            var valid = passwordsAndPolicies.Count(item => item.policy.IsValid(item.password));

            Assert.That(valid, Is.EqualTo(422));
        }

        [Test]
        public void Day2Part2Example()
        {
            var policy = new Part2PasswordPolicy('a', 1, 3);
            
            var result = policy.IsValid("abcde");

            Assert.That(result, Is.True);
        }

        [Test]
        public void Day2Part2()
        {
            var inputText = File.ReadAllLines("Day2Input1.txt");
            var passwordsAndPolicies = inputText.Select(x =>
            {
                var password = x.Split(':')[1].Trim();
                var policy = Part2PasswordPolicy.FromEntry(x).First();
                return new { password, policy };
            });

            var valid = passwordsAndPolicies.Count(item => item.policy.IsValid(item.password));

            Assert.That(valid, Is.EqualTo(451));
        }
    }

    public record Part2PasswordPolicy
    {
        public char Value { get; init; }
        public int Position1 { get; init; }
        public int Position2 { get; init; }

        public Part2PasswordPolicy(char value, int position1, int position2)
        {
            Value = value;
            Position1 = position1;
            Position2 = position2;
        }

        public bool IsValid(string value)
        {
            var position1 = Position1 - 1;
            var position2 = Position2 - 1;

            var match = 0;
            if (value.Length >= position1)
            {
                if (value[position1] == Value)
                {
                    match++;
                }
            }

            if (value.Length >= position2)
            {
                if (value[position2] == Value)
                {
                    match++;
                }
            }
            
            return match == 1;
        }

        public static IEnumerable<Part2PasswordPolicy> FromEntry(string passwordFileString)
        {
            var items = new List<Part2PasswordPolicy>();

            var regex = new Regex("([0-9]+)-([0-9]+)\\s([a-zA-Z]):.+");
            var results = regex.Match(passwordFileString);
            var min = int.Parse(results.Groups[1].Value);
            var max = int.Parse(results.Groups[2].Value);
            var value = results.Groups[3].Value[0];

            var policy = new Part2PasswordPolicy(value, min, max);
            items.Add(policy);

            return items;
        }
    }

    public record PasswordPolicy
    {
        public char Value { get; init; }
        public int MinOccurs { get; init; }
        public int MaxOccurs { get; init; }

        private PasswordPolicy(char value, int minOccurs, int maxOccurs)
        {
            Value = value;
            MinOccurs = minOccurs;
            MaxOccurs = maxOccurs;
        }

        public bool IsValid(string value)
        {
            var countOfValue = value.Count(x => x == Value);

            return countOfValue >= MinOccurs && countOfValue <= MaxOccurs;
        }

        public static IEnumerable<PasswordPolicy> FromEntry(string passwordFileString)
        {
            var items = new List<PasswordPolicy>();

            var regex = new Regex("([0-9]+)-([0-9]+)\\s([a-zA-Z]):.+");
            var results = regex.Match(passwordFileString);
            var min = int.Parse(results.Groups[1].Value);
            var max = int.Parse(results.Groups[2].Value);
            var value = results.Groups[3].Value[0];

            var policy = new PasswordPolicy(value, min, max);
            items.Add(policy);

            return items;
        }
    }
}