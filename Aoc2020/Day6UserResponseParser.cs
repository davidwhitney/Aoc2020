using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020
{
    public class Day6UserResponseParser
    {
        public static List<GroupResponses> AnswersForGroups(string data)
        {
            var fixedLineEndings = data.Replace("\r\n", "\n");
            var groups = fixedLineEndings.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);

            return groups.Select(groupInput => groupInput.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .Select(AnswersForGroup)
                .ToList();
        }

        public static GroupResponses AnswersForGroup(IEnumerable<string> groupedUserResponses)
        {
            var result = new GroupResponses();
            foreach (var userResponseText in groupedUserResponses)
            {
                result.UserResponses.Add(AnswersForSingleUser(userResponseText));
            }

            return result;
        }

        public static Questionnaire AnswersForSingleUser(string singleUsersResponses)
        {
            var questionnaire = new Questionnaire();
            foreach (var ch in singleUsersResponses)
            {
                questionnaire.Set(ch, true);
            }
            return questionnaire;
        }
    }

    public class GroupResponses
    {
        public List<Questionnaire> UserResponses { get; } = new();

        public ISet<char> DistinctYesAnswers =>
            UserResponses.SelectMany(x => x.YesAnswers)
                .ToHashSet();

        public IEnumerable<char> UnanimousYesAnswers
        {
            get
            {
                var alpha = new Range('a', 'z').AsChars();
                return alpha.Where(letter => UserResponses.All(x => x.YesAnswers.Contains(letter)));
            }
        }
    }

    public class Questionnaire
    {
        public int Count => _values.Count;
        public Dictionary<char, bool>.ValueCollection Values => _values.Values;

        private readonly Dictionary<char, bool> _values;

        public Questionnaire()
        {
            _values = new Range('a', 'z').AsChars()
                .ToDictionary(c => c, c => false);
        }

        public Questionnaire Set(char questionKey, bool value)
        {
            _values[questionKey] = value;
            return this;
        }

        public bool For(char questionKey) => _values[questionKey];

        public IEnumerable<char> YesAnswers => _values.Where(kvp => kvp.Value).Select(x=>x.Key);
    }

    public static class RangeExtensions
    {
        public static IEnumerable<char> AsChars(this Range src)
        {
            for (var i = src.Start.Value; i <= src.End.Value; i++)
            {
                yield return (char)i;
            }
        }
    }
}