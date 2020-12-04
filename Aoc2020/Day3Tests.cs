using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            var result = CountTrees(terrain,3, 1);

            Assert.That(result, Is.EqualTo(7));
        }

        [Test]
        public void Part1()
        {
            var terrain = File.ReadAllLines("Day3Input.txt").Select(x=>x.Trim()).ToList();

            var result = CountTrees(terrain, 3, 1);

            Assert.That(result, Is.EqualTo(262));
        }

        [Test]
        public void Part2()
        {
            var terrain = File.ReadAllLines("Day3Input.txt").Select(x=>x.Trim()).ToList();

            var slopes = new List<int[]>
            {
                new[] {1, 1},
                new[] {3, 1},
                new[] {5, 1},
                new[] {7, 1},
                new[] {1, 2},
            };

            var counts = slopes.Select(slope => CountTrees(terrain, slope[0], slope[1])).ToList();
            var total = counts.Aggregate<int, long>(1, (current, count) => current * count);

            Assert.That(total, Is.EqualTo(2698900776));
        }

        public int CountTrees(List<string> terrain, int slopeX, int slopeY)
        {
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

                currentX += slopeX;
                currentY += slopeY;

                currentX = currentX >= terrain[0].Length
                    ? currentX - terrain[0].Length
                    : currentX;
            }

            return trees;
        }
    }

}