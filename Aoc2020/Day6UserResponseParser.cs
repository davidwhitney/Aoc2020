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

        public IEnumerable<char> DistinctYesAnswers =>
            UserResponses.SelectMany(x => x.YesAnswers)
                .ToHashSet();
    }

    public class Questionnaire
    {
        public int Count => _values.Count;
        public Dictionary<char, bool>.ValueCollection Values => _values.Values;

        private readonly Dictionary<char, bool> _values;

        public Questionnaire()
        {
            _values = Enumerable.Range(0, 26)
                .Select(i => (char)(i + 97))
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
}