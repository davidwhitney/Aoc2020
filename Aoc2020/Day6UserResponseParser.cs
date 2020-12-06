using System;
using System.Collections.Generic;
using System.Linq;
using SelectedAnswers = System.Collections.Generic.IEnumerable<char>;
using GroupedResults = System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<char>>;
using FoldedResults = System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<char>>>;

namespace Aoc2020
{
    public static class Day6UserResponseParser
    {
        private const string AToZ = "abcdefghijklmnopqrstuvwxyz";

        public static FoldedResults AnswersForGroups(string data)
            => data.Replace("\r\n", "\n")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(groupInput => groupInput.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .Select(AnswersForGroup);

        public static GroupedResults AnswersForGroup(IEnumerable<string> groupedUserResponses) 
            => groupedUserResponses.Select(AnswersForSingleUser);
        public static SelectedAnswers AnswersForSingleUser(string singleUsersResponses) 
            => AToZ.Where(singleUsersResponses.Contains);
        public static ISet<char> YesAnswers(this GroupedResults src) 
            => src.SelectMany(x => x).ToHashSet();
        public static SelectedAnswers UnanimousYesAnswers(this GroupedResults userAnswers) 
            => AToZ.Where(letter => userAnswers.All(x => x.Contains(letter)));
    }
}