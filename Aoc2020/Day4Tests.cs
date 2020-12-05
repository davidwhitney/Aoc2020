using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Aoc2020
{
    [TestFixture]
    public class Day4Tests
    {
        [Test]
        public void FromLine_PassportWithSingleField_CorrectNumberOfFieldsDetected()
        {
            const string singleLineWithOneEntry = "ecl:gry";
            var passport = Passport.FromLine(singleLineWithOneEntry);
            Assert.That(passport.Fields.Count, Is.EqualTo(1));
        }

        [Test]
        public void FromLine_PassportWithSingleField_Parses()
        {
            const string singleLineWithOneEntry = "ecl:gry";
            var passport = Passport.FromLine(singleLineWithOneEntry);
            Assert.That(passport.Fields["ecl"], Is.EqualTo("gry"));
        }

        [Test]
        public void FromLine_PassportWithMultipleFields_Parses()
        {
            const string multipleValues = "ecl:gry pid:860033327";
            var passport = Passport.FromLine(multipleValues);
            Assert.That(passport.Fields.Count, Is.EqualTo(2));
        }

        [Test]
        public void FromLine_PassportWithMultipleFieldsWithNewLineSplitter_Parses()
        {
            string multipleValues;
            multipleValues = "ecl:gry\npid:860033327";
            var passport = Passport.FromLine(multipleValues);
            Assert.That(passport.Fields.Count, Is.EqualTo(2));
        }

        [Test]
        public void FromFile_GivenOneEntry_Parses()
        {
            const string singleLineWithOneEntry = "ecl:gry";
            var passport = Passport.FromFile(singleLineWithOneEntry);

            Assert.That(passport.Count(), Is.EqualTo(1));
        }

        [Test]
        public void FromFile_GivenMultipleEntry_Parses()
        {
            const string fileWithTwoEntries = "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd\r\n" +
                                              "byr:1937 iyr:2017 cid:147 hgt:183cm\r\n\r\n" +
                                              "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884\r\n" +
                                              "hcl:#cfa07d byr:1929";

            var passport = Passport.FromFile(fileWithTwoEntries);

            Assert.That(passport.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Example1_ContainsAllFields()
        {
            const string input = "ecl:gry pid:860033327 eyr:2020 hcl:#fffffd\r\nbyr:1937 iyr:2017 cid:147 hgt:183cm\r\n";
            var passport = Passport.FromFile(input).First();

            Assert.That(passport.IsValid, Is.True);
        }

        [Test]
        public void Example2_MissingHgt()
        {
            const string input = "iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884\r\nhcl:#cfa07d byr:1929";
            var passport = Passport.FromFile(input).First();

            Assert.That(passport.IsValid, Is.False);
        }

        [Test]
        public void Example2_OnlyMissingCID()
        {
            const string input = "hcl:#ae17e1 iyr:2013\r\neyr:2024\r\necl:brn pid:760753108 byr:1931\r\nhgt:179cm";
            var passport = Passport.FromFile(input).First();

            Assert.That(passport.IsValid, Is.True);
        }

        [Test]
        public void Part1()
        {
            var input = File.ReadAllText("Day4.txt");
            var passports = Passport.FromFile(input);

            var validCount = passports.Count(p => p.IsValid);

            Assert.That(validCount, Is.EqualTo(200));
        }

        [Test]
        public void Part2_Examples()
        {
            var rule = Passport.ContentValidators.Single(x => x.FieldName == "byr");
            Assert.That(rule.Validate("2002"), Is.True);
            Assert.That(rule.Validate("2003"), Is.False);

            var rule2 = Passport.ContentValidators.Single(x => x.FieldName == "iyr");
            Assert.That(rule2.Validate("2015"), Is.True);
            Assert.That(rule2.Validate("2025"), Is.False);

            var rule3 = Passport.ContentValidators.Single(x => x.FieldName == "eyr");
            Assert.That(rule3.Validate("2025"), Is.True);
            Assert.That(rule3.Validate("2035"), Is.False);

            var rule4 = Passport.ContentValidators.Single(x => x.FieldName == "hgt");
            Assert.That(rule4.Validate("60in"), Is.True);
            Assert.That(rule4.Validate("190cm"), Is.True);
            Assert.That(rule4.Validate("190in"), Is.False);
            Assert.That(rule4.Validate("190"), Is.False);

            var rule5 = Passport.ContentValidators.Single(x => x.FieldName == "hcl");
            Assert.That(rule5.Validate("#123abc"), Is.True);
            Assert.That(rule5.Validate("#123abz"), Is.False);
            Assert.That(rule5.Validate("123abc"), Is.False);

            var rule6 = Passport.ContentValidators.Single(x => x.FieldName == "ecl");
            Assert.That(rule6.Validate("blah"), Is.False);
            Assert.That(rule6.Validate("amb"), Is.True);

            var rule7 = Passport.ContentValidators.Single(x => x.FieldName == "pid");
            Assert.That(rule7.Validate("000000001"), Is.True);
            Assert.That(rule7.Validate("0123456789"), Is.False);
        }

        [Test]
        public void Part2()
        {
            var input = File.ReadAllText("Day4.txt");
            var passports = Passport.FromFile(input, Passport.ContentValidators);

            var validCount = passports.Count(p => p.IsValid);

            Assert.That(validCount, Is.EqualTo(116));
        }

    }

    public class Passport
    {
        public Dictionary<string, string> Fields { get; }
        private readonly List<Rule> _validators;

        public Passport(IEnumerable<KeyValuePair<string, string>> pairs, IEnumerable<Rule> withValidators = null)
        {
            Fields = new Dictionary<string, string>(pairs);
            _validators = (withValidators ?? PresenceValidators).ToList();
        }

        public bool IsValid => _validators.All(v => v.Validate(Fields));

        public static IEnumerable<Passport> FromFile(string passportFile, IEnumerable<Rule> withValidators = null)
        {
            var passportsLines = passportFile.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            return passportsLines.Select(l => FromLine(l, withValidators)).ToList();
        }

        public static Passport FromLine(string passportFields, IEnumerable<Rule> withValidators = null)
        {
            var fieldsAndValues =
                passportFields.Split(new[] {" ", "\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var dic = fieldsAndValues.Select(pair => pair.Split(':'))
                .ToDictionary(parts => parts[0], parts => parts[1]);

            return new Passport(dic, withValidators);
        }

        public static IEnumerable<Rule> ContentValidators => new List<Rule>
        {
            new("byr", t => int.TryParse(t, out var value) && value >= 1920 && value <= 2002),
            new("iyr", t => int.TryParse(t, out var value) && value >= 2010 && value <= 2020),
            new("eyr", t => int.TryParse(t, out var value) && value >= 2020 && value <= 2030),
            new("hgt", t =>
            {
                var regex = new Regex("^([0-9]+)(cm|in)$");
                var matches = regex.Match(t);

                int.TryParse(matches.Groups[1].Value, out var numeric);
                var unit = matches.Groups[2].Value.ToLower();

                return unit switch
                {
                    "cm" => numeric >= 150 && numeric <= 193,
                    "in" => numeric >= 59 && numeric <= 76,
                    _ => false
                };
            }),
            new("hcl", t => new Regex("^#[0-9a-fA-F]{6,6}$").IsMatch(t)),
            new("ecl", t =>
            {
                return new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(t);
            }),
            new("pid", t => new Regex("^[0-9]{9,9}$").IsMatch(t)),
            new("cid", t => true, true)
        };

        public static IEnumerable<Rule> PresenceValidators => new List<Rule>
        {
            new("byr", t => true),
            new("iyr", t => true),
            new("eyr", t => true),
            new("hgt", t => true),
            new("hcl", t => true),
            new("ecl", t => true),
            new("pid", t => true),
            new("cid", t => true, true)
        };
    }

    public record Rule
    {
        public string FieldName { get; init; }

        private readonly Func<string, bool> _validator;
        private readonly bool _optional;

        public Rule(string fieldName, Func<string, bool> validator, bool optional = false) 
            => (FieldName, _validator, _optional) = (fieldName, validator, optional);

        public bool Validate(string value) => _validator(value);

        public bool Validate(Dictionary<string, string> allFields)
        {
            if (_optional && !allFields.ContainsKey(FieldName))
            {
                return true;
            }

            if (!allFields.ContainsKey(FieldName))
            {
                return false;
            }

            var value = allFields[FieldName];
            return Validate(value);
        }
    }
}