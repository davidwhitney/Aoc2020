using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using NUnit.Framework;

namespace Aoc2020
{
    [TestFixture]
    public class Day5Tests
    {
        [Test]
        public void Example1()
        {
            var result = Solve("FBFBBFFRLR");
            Assert.That(result.Row, Is.EqualTo(44));
            Assert.That(result.Col, Is.EqualTo(5));
            Assert.That(result.SeatId, Is.EqualTo(357));

            var result2 = Solve("BFFFBBFRRR");
            Assert.That(result2.Row, Is.EqualTo(70));
            Assert.That(result2.Col, Is.EqualTo(7));
            Assert.That(result2.SeatId, Is.EqualTo(567));

            var result3 = Solve("FFFBBBFRRR");
            Assert.That(result3.Row, Is.EqualTo(14));
            Assert.That(result3.Col, Is.EqualTo(7));
            Assert.That(result3.SeatId, Is.EqualTo(119));

            var result4 = Solve("BBFFBBFRLL");
            Assert.That(result4.Row, Is.EqualTo(102));
            Assert.That(result4.Col, Is.EqualTo(4));
            Assert.That(result4.SeatId, Is.EqualTo(820));
        }

        [Test]
        public void Part1()
        {
            var inputLines = File.ReadAllLines("Day5Input.txt").Select(x => x.Trim());
            var highestId =
                inputLines.Select(Solve)
                    .Select(boardingPass => boardingPass.SeatId)
                    .Max();

            Assert.That(highestId, Is.EqualTo(970));
        }

        [Test]
        public void Part2()
        {
            var inputLines = File.ReadAllLines("Day5Input.txt").Select(x => x.Trim());
            var boardingPasses = inputLines.Select(Solve).ToList();

            var mySeat = FindMissingSeatId(boardingPasses);

            Assert.That(mySeat, Is.EqualTo(587));
        }

        private static int FindMissingSeatId(List<Seat> boardingPasses)
        {
            boardingPasses = boardingPasses.OrderBy(x => x.SeatId).ToList();

            var previous = 0;

            foreach (var pass in boardingPasses)
            {
                var expected = previous + 1;
                if (pass.SeatId != expected && previous != 0)
                {
                    return pass.SeatId - 1;
                }

                previous = pass.SeatId;
            }

            return -1;
        }

        private static Seat Solve(string boardPassCode)
        {
            var rowCodes = boardPassCode.Substring(0, boardPassCode.Length - 3);
            var colCodes = boardPassCode.Substring(boardPassCode.Length - 3);

            var validRows = Enumerable.Range(0, 128).ToList();
            var validCols = Enumerable.Range(0, 8).ToList();

            foreach (var character in rowCodes)
            {
                var halfRows = validRows.Count / 2;
                validRows = character switch
                {
                    'F' => validRows.Take(halfRows).ToList(),
                    'B' => validRows.Skip(halfRows).ToList(),
                    _ => validRows
                };
            }

            foreach (var character in colCodes)
            {
                var halfCols = validCols.Count / 2;
                validCols = character switch
                {
                    'L' => validCols.Take(halfCols).ToList(),
                    'R' => validCols.Skip(halfCols).ToList(),
                    _ => validCols
                };
            }

            var row = validRows.Single();
            var col = validCols.Single();
            return new Seat(row, col);
        }
    }

    public record Seat
    {
        public int Row { get; init; }
        public int Col { get; init; }
        public int SeatId => Row * 8 + Col;
        public Seat(int row, int col) => (Row, Col) = (row, col);
    } 

}