using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Aoc2020
{
    public class Day7Answers
    {
        public static Policy Parse(string[] rules)
        {
            return new(rules.Select(Parse).ToDictionary(x => x.Key, x => x.Value));
        }

        public static KeyValuePair<string, List<Requirement>> Parse(string rule)
        {
            const string keyToRuleDelimiter = "bags contain";
            const string noRequirements = "no other bags";

            var parts = rule.Split(keyToRuleDelimiter).Select(x => x.Trim()).ToList();
            
            var key = parts[0];
            var requirementString = parts[1].Trim().Trim('.').ToLower();

            if (requirementString == noRequirements)
            {
                return Requirement.None(key);
            }
            
            var requirementsPreParsed = requirementString.Split(",").Select(x => x.Trim());
            var ruleMatcher = new Regex("^([0-9]+) (.+) bag(s)?$");
            var requirements = new List<Requirement>();

            foreach (var req in requirementsPreParsed)
            {
                var parsed = ruleMatcher.Match(req);
                var count = parsed.Groups[1].Value;
                var type = parsed.Groups[2].Value;
                int.TryParse(count, out var minimum);

                var requirement = new Requirement(type, minimum);
                requirements.Add(requirement);
            }

            return new KeyValuePair<string, List<Requirement>>(key, requirements);
        }
    }

    public class Policy : Dictionary<string, List<Requirement>>
    {
        public Policy(IEnumerable<KeyValuePair<string, List<Requirement>>> items = null)
        {
            items ??= new List<KeyValuePair<string, List<Requirement>>>();
            foreach (var (key, value) in items)
            {
                Add(key, value);
            }
        }

        public int CountCanContain(string target)
        {
            var supports = 0;
            foreach (var (key, _) in this)
            {
                var dependencies = AllRequirementsOf(key).Select(d => d.Target);
                if (dependencies.Contains(target))
                {
                    supports++;
                }
            }

            return supports;
        }

        public int SumContents(string target)
        {
            var current = 0;
            var requirements = this[target];

            current += requirements.Sum(x => x.Minimum);

            foreach (var (key, minimum) in requirements)
            {
                current += minimum * SumContents(key);
            }

            return current;
        }

        public List<Requirement> AllRequirementsOf(string target, List<Requirement> all = null)
        {
            all ??= new List<Requirement>();

            var requirements = this[target];
            all.AddRange(requirements);

            foreach (var requirement in requirements)
            {
                AllRequirementsOf(requirement.Target, all);
            }

            return all;
        }
    }

    public record Requirement(string Target, int Minimum)
    {
        public static List<Requirement> Empty => new();
        public static KeyValuePair<string, List<Requirement>> None(string forKey) => new(forKey, Empty);
    }
}