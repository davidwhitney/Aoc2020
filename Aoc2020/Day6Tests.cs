using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Aoc2020
{
    [TestFixture]
    public class Day6Tests
    {
        [Test]
        public void AnswersForManyQuestionnaires_ParsesOutGroupsCorrectly()
        {
            var data = "abc\r\n\r\na\r\nb\r\nc";

            var result = Day6UserResponseParser.AnswersForGroups(data).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].ToList().YesAnswers().Count, Is.EqualTo(3));
            Assert.That(result[1].ToList().YesAnswers().Count, Is.EqualTo(3));
        }

        [Test]
        public void AnswersForSingleUser_NoInputs_AllAnswersFalse()
        {
            var result = Day6UserResponseParser.AnswersForSingleUser("");

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void AnswersForSingleUser_SingleRecognisedAnswer_FlagsAsTrue()
        {
            var result = Day6UserResponseParser.AnswersForSingleUser("a");

            Assert.That(result.ToList().Contains('a'));
        }

        [Test]
        public void AnswersForSingleUser_LineWithMultipleYesAnswers_FlagsAllYesAnswersAsTrue()
        {
            var result = Day6UserResponseParser.AnswersForSingleUser("abc");

            Assert.That(result.ToList().Contains('a'));
            Assert.That(result.ToList().Contains('b'));
            Assert.That(result.ToList().Contains('c'));
        }

        [Test]
        public void AnswersForGroup_CalledWithJustOneUser_OneSetOfAnswerReturned()
        {
            var result = Day6UserResponseParser.AnswersForGroup(new []
            {
                "abc"
            });

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void AnswersForGroup_CalledWithMoreThanOneUser_CorrectSetOfAnswerReturned()
        {
            var result = Day6UserResponseParser.AnswersForGroup(new []
            {
                "abc",
                "a",
                ""
            });

            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void Example1()
        {
            var input = "abc\r\n\r\na\r\nb\r\nc\r\n\r\nab\r\nac\r\n\r\na\r\na\r\na\r\na\r\n\r\nb";

            var result = Day6UserResponseParser.AnswersForGroups(input);
            var sum = result.Sum(x => x.YesAnswers().Count);

            Assert.That(sum, Is.EqualTo(11));
        }

        [Test]
        public void Parts()
        {
            var input = File.ReadAllText("Day6Input.txt");

            var result = Day6UserResponseParser.AnswersForGroups(input);

            var part1 = result.Sum(x => x.YesAnswers().Count);
            var part2 = result.Sum(x => x.UnanimousYesAnswers().Count());

            Assert.That(part1, Is.EqualTo(6742));
            Assert.That(part2, Is.EqualTo(3447));
        }


    }

    [TestFixture]
    public class GroupResponsesTests
    {
        [Test]
        public void DistinctYesAnswers_WhenNoAnswersAreYes_IsEmpty()
        {
            var sut = new List<List<char>>();

            var distinctYeses = sut.YesAnswers();

            Assert.That(distinctYeses, Is.Empty);
        }

        [Test]
        public void DistinctYesAnswers_SingleAAnsweredYes_AReturned()
        {
            var sut = new List<List<char>>
            {
                new List<char> {'a'}
            };

            var distinctYeses = sut.YesAnswers();

            Assert.That(distinctYeses.Count, Is.EqualTo(1));
        }

        [Test]
        public void DistinctYesAnswers_MultipleAAnsweredYes_AReturned()
        {
            var sut = new List<List<char>>
            {
                new List<char> {'a'}, 
                new List<char> {'a'}
            };

            var distinctYeses = sut.YesAnswers();

            Assert.That(distinctYeses.Count, Is.EqualTo(1));
            Assert.That(distinctYeses.First(), Is.EqualTo('a'));
        }

        [Test]
        public void DistinctYesAnswers_MultipleLettersAnsweredYes_AllReturnedReturned()
        {
            var sut = new List<List<char>>
            {
                new List<char> {'a'}, 
                new List<char> {'b'}
            };

            var distinctYeses = sut.YesAnswers().ToList();

            Assert.That(distinctYeses.Count, Is.EqualTo(2));
            Assert.That(distinctYeses[0], Is.EqualTo('a'));
            Assert.That(distinctYeses[1], Is.EqualTo('b'));
        }
    }
}