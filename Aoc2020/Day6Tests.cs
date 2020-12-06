using System.IO;
using System.Linq;
using System.Net.WebSockets;
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

            var result = Day6UserResponseParser.AnswersForGroups(data);

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].DistinctYesAnswers.Count(), Is.EqualTo(3));
            Assert.That(result[1].DistinctYesAnswers.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AnswersForSingleUser_NoInputs_AllAnswersFalse()
        {
            var result = Day6UserResponseParser.AnswersForSingleUser("");

            Assert.That(result.Count, Is.EqualTo(26));
            Assert.That(result.Values.All(x=>x == false));
        }

        [Test]
        public void AnswersForSingleUser_SingleRecognisedAnswer_FlagsAsTrue()
        {
            var result = Day6UserResponseParser.AnswersForSingleUser("a");

            Assert.That(result.For('a'), Is.True);
        }

        [Test]
        public void AnswersForSingleUser_LineWithMultipleYesAnswers_FlagsAllYesAnswersAsTrue()
        {
            var result = Day6UserResponseParser.AnswersForSingleUser("abc");

            Assert.That(result.For('a'), Is.True);
            Assert.That(result.For('b'), Is.True);
            Assert.That(result.For('c'), Is.True);
        }

        [Test]
        public void AnswersForGroup_CalledWithJustOneUser_OneSetOfAnswerReturned()
        {
            var result = Day6UserResponseParser.AnswersForGroup(new []
            {
                "abc"
            });

            Assert.That(result.UserResponses.Count, Is.EqualTo(1));
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

            Assert.That(result.UserResponses.Count, Is.EqualTo(3));
        }

        [Test]
        public void Example1()
        {
            var input = "abc\r\n\r\na\r\nb\r\nc\r\n\r\nab\r\nac\r\n\r\na\r\na\r\na\r\na\r\n\r\nb";

            var result = Day6UserResponseParser.AnswersForGroups(input);
            var sum = result.Sum(x => x.DistinctYesAnswers.Count());

            Assert.That(sum, Is.EqualTo(11));
        }

        [Test]
        public void Part1()
        {
            var input = File.ReadAllText("Day6Input.txt");

            var result = Day6UserResponseParser.AnswersForGroups(input);
            var sum = result.Sum(x => x.DistinctYesAnswers.Count());

            Assert.That(sum, Is.EqualTo(11));
        }


    }

    [TestFixture]
    public class GroupResponsesTests
    {
        [Test]
        public void DistinctYesAnswers_WhenNoAnswersAreYes_IsEmpty()
        {
            var sut = new GroupResponses();

            var distinctYeses = sut.DistinctYesAnswers.ToList();

            Assert.That(distinctYeses, Is.Empty);
        }

        [Test]
        public void DistinctYesAnswers_SingleAAnsweredYes_AReturned()
        {
            var sut = new GroupResponses();
            sut.UserResponses.Add(new Questionnaire().Set('a', true));

            var distinctYeses = sut.DistinctYesAnswers;

            Assert.That(distinctYeses.Count, Is.EqualTo(1));
        }

        [Test]
        public void DistinctYesAnswers_MultipleAAnsweredYes_AReturned()
        {
            var sut = new GroupResponses();
            sut.UserResponses.Add(new Questionnaire().Set('a', true));
            sut.UserResponses.Add(new Questionnaire().Set('a', true));

            var distinctYeses = sut.DistinctYesAnswers.ToList();

            Assert.That(distinctYeses.Count, Is.EqualTo(1));
            Assert.That(distinctYeses[0], Is.EqualTo('a'));
        }

        [Test]
        public void DistinctYesAnswers_MultipleLettersAnsweredYes_AllReturnedReturned()
        {
            var sut = new GroupResponses();
            sut.UserResponses.Add(new Questionnaire().Set('a', true));
            sut.UserResponses.Add(new Questionnaire().Set('b', true));

            var distinctYeses = sut.DistinctYesAnswers.ToList();

            Assert.That(distinctYeses.Count, Is.EqualTo(2));
            Assert.That(distinctYeses[0], Is.EqualTo('a'));
            Assert.That(distinctYeses[1], Is.EqualTo('b'));
        }
    }
}