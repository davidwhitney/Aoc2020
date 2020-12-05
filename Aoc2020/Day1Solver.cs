using System;
using System.Collections.Generic;

namespace Aoc2020
{
    public class Day1Solver
    {
        public int FindEntriesThatSumTo2020(List<int> expenseReport)
        {
            var list1 = new List<int>(expenseReport);
            var list2 = new List<int>(expenseReport);

            foreach (var item in list1)
            {
                foreach(var item2 in list2)
                {
                    if (item + item2 == 2020)
                    {
                        return item * item2;
                    }
                }
            }

            throw new Exception("Oh no");
        }

        public int Find3EntriesThatSumTo2020(List<int> expenseReport)
        {
            var list1 = new List<int>(expenseReport);
            var list2 = new List<int>(expenseReport);
            var list3 = new List<int>(expenseReport);

            foreach (var item in list1)
            {
                foreach(var item2 in list2)
                {
                    foreach (var item3 in list3)
                    {
                        if (item + item2 + item3 == 2020)
                        {
                            return item * item2 * item3;
                        }
                    }
                }
            }

            throw new Exception("Oh no");
        }
    }
}