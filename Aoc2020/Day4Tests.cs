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
    }

    public class Passport
    {
        public Dictionary<string, string> Fields { get; }
        private readonly List<Rule> _validators;

        public Passport(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            Fields = new Dictionary<string, string>(pairs);
            _validators = new List<Rule>
            {
                new("byr", (t) => { return true; }),
                new("iyr", (t) => { return true; }),
                new("eyr", (t) => { return true; }),
                new("hgt", (t) => { return true; }),
                new("hcl", (t) => { return true; }),
                new("ecl", (t) => { return true; }),
                new("pid", (t) => { return true; }),
                new("cid", (t) => { return true; }, true)
            };
        }

        public bool IsValid => _validators.All(v => v.Validate(Fields));

        public static IEnumerable<Passport> FromFile(string passportFile)
        {
            var passportsLines = passportFile.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            return passportsLines.Select(FromLine).ToList();
        }

        public static Passport FromLine(string passportFields)
        {
            var fieldsAndValues =
                passportFields.Split(new[] {" ", "\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);

            var dic = fieldsAndValues.Select(pair => pair.Split(':'))
                .ToDictionary(parts => parts[0], parts => parts[1]);

            return new Passport(dic);
        }
    }

    public class Rule
    {
        public string FieldName { get; }
        public Func<Dictionary<string, string>, bool> Validator { get; }
        public bool Optional { get; }

        public Rule(string fieldName, Func<Dictionary<string, string>, bool> validator, bool optional = false)
        {
            FieldName = fieldName;
            Validator = validator;
            Optional = optional;
        }

        public bool Validate(Dictionary<string, string> allFields)
        {
            if (Optional && !allFields.ContainsKey(FieldName))
            {
                return true;
            }

            if (!allFields.ContainsKey(FieldName))
            {
                return false;
            }

            return Validator(allFields);
        }
    }
}