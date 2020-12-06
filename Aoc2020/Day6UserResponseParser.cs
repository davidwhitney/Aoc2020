using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc2020
{
    public class Day6UserResponseParser
    {
        public static IEnumerable<IEnumerable<IEnumerable<char>>> AnswersForGroups(string data)
        {
            return data
                .Replace("\r\n", "\n")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(groupInput => groupInput.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .Select(AnswersForGroup);
        }

        public static IEnumerable<IEnumerable<char>> AnswersForGroup(IEnumerable<string> groupedUserResponses) 
            => groupedUserResponses.Select(AnswersForSingleUser);

        public static IEnumerable<char> AnswersForSingleUser(string singleUsersResponses) 
            => new Range('a', 'z').AsChars().Where(singleUsersResponses.Contains);
    }

    public static class CountingExtensions
    {
        public static ISet<char> YesAnswers(this IEnumerable<IEnumerable<char>> src) =>
            src.SelectMany(x => x).ToHashSet();

        public static IEnumerable<char> UnanimousYesAnswers(this IEnumerable<IEnumerable<char>> userAnswers) =>
            new Range('a', 'z').AsChars().Where(letter => userAnswers.All(x => x.Contains(letter)));
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