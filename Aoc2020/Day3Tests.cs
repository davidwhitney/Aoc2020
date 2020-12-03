using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Aoc2020
{
    [TestFixture]
    public class Day3Tests
    {
        [Test]
        public void Example()
        {
            var terrain = new List<string>
            {
                "..##.......",
                "#...#...#..",
                ".#....#..#.",
                "..#.#...#.#",
                ".#...##..#.",
                "..#.##.....",
                ".#.#.#....#",
                ".#........#",
                "#.##...#...",
                "#...##....#",
                ".#..#...#.#",
            };

            var result = CountTrees(terrain);

            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        public void Part1()
        {
            var terrain = File.ReadAllLines("Day3Input.txt").Select(x=>x.Trim()).ToList();

            var result = CountTrees(terrain);

            Assert.That(result, Is.EqualTo(262));
        }

        public int CountTrees(List<string> terrain)
        {
            var slopeAcross = 3;
            var slopeDown = 1;

            var currentY = 0;
            var currentX = 0;

            var trees = 0;

            while (currentY < terrain.Count)
            {
                var currentLocValue = terrain[currentY][currentX];
                if (currentLocValue == '#')
                {
                    trees++;
                }

                currentX += slopeAcross;
                currentY += slopeDown;

                currentX = currentX >= terrain[0].Length
                    ? currentX - terrain[0].Length
                    : currentX;
            }

            return trees;
        }
    }

}