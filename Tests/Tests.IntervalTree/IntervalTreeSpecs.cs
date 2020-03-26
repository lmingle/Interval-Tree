using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IntervalTree;
using NUnit.Framework;

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
namespace Tests.IntervalTree.IntervalTreeSpecs
{
    [TestFixture]
    public class If_the_user_searches_for_overlapping_entries_in_an_interval_tree : Spec {
        private static IEnumerable<Interval<int>> TestEntries() {
            yield return new Interval<int>(1400, 1500);
            yield return new Interval<int>(0100, 0130);
            yield return new Interval<int>(1700, 1800);
            yield return new Interval<int>(0230, 0240);
            yield return new Interval<int>(0530, 0540);
            yield return new Interval<int>(2330, 2400);
            yield return new Interval<int>(0700, 0800);
            yield return new Interval<int>(0900, 1000);
            yield return new Interval<int>(0000, 0100);
            yield return new Interval<int>(0540, 0700);
            yield return new Interval<int>(1800, 2130);
            yield return new Interval<int>(2130, 2131);
            yield return new Interval<int>(0200, 0230);
        }
        
        private static IEnumerable TestCases {
            get {
                yield return new TestCaseData(new Interval<int>(2000,2300)).Returns(2);
                yield return new TestCaseData(new Interval<int>(0000, 0100)).Returns(2);
                yield return new TestCaseData(new Interval<int>(0000, 0000)).Returns(1);
                yield return new TestCaseData(new Interval<int>(0100, 0100)).Returns(2);
                yield return new TestCaseData(new Interval<int>(1000, 1100)).Returns(1);
                yield return new TestCaseData(new Interval<int>(1030, 1400)).Returns(1);
                yield return new TestCaseData(new Interval<int>(0150, 0155)).Returns(0);
                yield return new TestCaseData(new Interval<int>(2132, 2133)).Returns(0);
                yield return new TestCaseData(new Interval<int>(1030, 1350)).Returns(0);
                yield return new TestCaseData(new Interval<int>(0000, 2359)).Returns(13);
            }
        }

        [Test, TestCaseSource("TestCases")]
        public int Shall_the_result_be_correct_for_an_interval_that_has_been_built_in_order(Interval<int> value) {
            var tree = CreateTree(TestEntries().OrderBy(interval => interval.Start));
            return tree
                .Search(value)
                .Count();
        }

        [Test, TestCaseSource("TestCases")]
        public int Shall_the_result_be_correct_for_an_interval_that_has_been_built_in_reverse_order(Interval<int> value) {
            var tree = CreateTree(TestEntries().OrderBy(interval => interval.Start).Reverse());
            return tree
                .Search(value)
                .Count();
        }

        [Test, TestCaseSource("TestCases")]
        public int Shall_the_result_be_correct_for_an_interval_that_has_been_built_in_unsorted_order(Interval<int> value) {
            var tree = CreateTree(TestEntries());
            return tree
                .Search(value)
                .Count();
        }

        private static IntervalTree<int> CreateTree(IEnumerable<Interval<int>> entries) {
            var tree = new IntervalTree<int>();

            foreach (var interval in entries) {
                tree.Add(interval);
            }

            return tree;
        }
    }

    [TestFixture]
    public class If_the_user_compares_two_intervals : Spec {
        Interval<int> openInterval = new Interval<int>(2, 10, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
        Interval<int> leftOpenInterval = new Interval<int>(2, 10, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Closed);
        Interval<int> rightOpenInterval = new Interval<int>(2, 10, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
        Interval<int> closedInterval = new Interval<int>(2, 10, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> openSmallerInterval = new Interval<int>(4, 8, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
        Interval<int> openBelowInterval = new Interval<int>(0, 1, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
        Interval<int> openAboveInterval = new Interval<int>(12, 15, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
        Interval<int> openLargerInterval = new Interval<int>(1, 11, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
        Interval<int> leftOpenSmallerInterval = new Interval<int>(4, 8, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Closed);
        Interval<int> leftOpenLargerInterval = new Interval<int>(1, 11, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Closed);
        Interval<int> rightOpenSmallerInterval = new Interval<int>(4, 8, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
        Interval<int> rightOpenLargerInterval = new Interval<int>(1, 11, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
        Interval<int> closedSmallerInterval = new Interval<int>(4, 8, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> closedBelowInterval = new Interval<int>(0, 1, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> closedAboveInterval = new Interval<int>(12, 15, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> closedLargerInterval = new Interval<int>(1, 11, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> closedHighInterval = new Interval<int>(10, 15, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> closedLowInterval = new Interval<int>(0, 2, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> open0_5Interval = new Interval<int>(0, 5, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
        Interval<int> closed0_5Interval = new Interval<int>(0, 5, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
        Interval<int> open9_12Interval = new Interval<int>(9, 12, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
        Interval<int> closed9_12Interval = new Interval<int>(9, 12, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);

        private static IEnumerable<Interval<int>> TestEntries() {
            yield return new Interval<int>(0, 60, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            yield return new Interval<int>(0, 25, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            yield return new Interval<int>(60, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            yield return new Interval<int>(50, 70, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            yield return new Interval<int>(140, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
        }

        [Test]
        public void TestCompareTo() {
            // Same
            //L       ○--------○
            //R       ○--------○
            Assert.AreEqual(0, openInterval.CompareTo(openInterval));
            //L       ○--------•
            //R       ○--------•
            Assert.AreEqual(0, leftOpenInterval.CompareTo(leftOpenInterval));
            //L       •--------○
            //R       •--------○
            Assert.AreEqual(0, rightOpenInterval.CompareTo(rightOpenInterval));
            //L       •--------•
            //R       •--------•
            Assert.AreEqual(0, closedInterval.CompareTo(closedInterval));

            // Below
            //L ○--○
            //R       ○--------○
            Assert.AreEqual(-1, openBelowInterval.CompareTo(openInterval));
            //L ○--○
            //R       •--------•
            Assert.AreEqual(-1, openBelowInterval.CompareTo(closedInterval));
            //L •--•
            //R       ○--------○
            Assert.AreEqual(-1, closedBelowInterval.CompareTo(openInterval));
            //L •--•
            //R       •--------•
            Assert.AreEqual(-1, closedBelowInterval.CompareTo(closedInterval));

            // Above
            //L                  ○--○
            //R       ○--------○
            Assert.AreEqual(1, openAboveInterval.CompareTo(openInterval));
            //L                  ○--○
            //R       •--------•
            Assert.AreEqual(1, openAboveInterval.CompareTo(closedInterval));
            //L                  •--•
            //R       ○--------○
            Assert.AreEqual(1, closedAboveInterval.CompareTo(openInterval));
            //L                  •--•
            //R       •--------•
            Assert.AreEqual(1, closedAboveInterval.CompareTo(closedInterval));

            // Different endpoint bound types
            //L       ○--------○
            //R       ○--------•
            Assert.AreEqual(-1, openInterval.CompareTo(leftOpenInterval));
            //L       ○--------○
            //R       •--------○
            Assert.AreEqual(1, openInterval.CompareTo(rightOpenInterval));
            //L       ○--------○
            //R       •--------•
            Assert.AreEqual(1, openInterval.CompareTo(closedInterval));
            //L       •--------•
            //R       ○--------•
            Assert.AreEqual(-1, closedInterval.CompareTo(leftOpenInterval));
            //L       •--------•
            //R       •--------○
            Assert.AreEqual(1, closedInterval.CompareTo(rightOpenInterval));
            //L       •--------•
            //R       ○--------○
            Assert.AreEqual(-1, closedInterval.CompareTo(openInterval));
        }

        [Test]
        public void TestOverlaps() {
            //L   ○--------○
            //R   ○--------○
            Assert.True(openInterval.Overlaps(openInterval));
            //L   ○--------○
            //R •-•
            Assert.False(openInterval.Overlaps(closedLowInterval));
            //L   •--------○
            //R •-•
            Assert.True(rightOpenInterval.Overlaps(closedLowInterval));
            //L   ○--------○
            //R            •-----•
            Assert.False(openInterval.Overlaps(closedHighInterval));
            //L   ○--------•
            //R            •-----•
            Assert.True(leftOpenInterval.Overlaps(closedHighInterval));
            //L     ○----○
            //R   ○--------○
            Assert.True(openSmallerInterval.Overlaps(openInterval));
        }

        [Test]
        public void TestContains() {
            //Points
            //   ○--------○
            // ^ ^    ^   ^  ^
            // 0 2    7   1  1
            //            0  2
            Assert.False(openInterval.Contains(0));
            Assert.False(openInterval.Contains(2));
            Assert.True(openInterval.Contains(7));
            Assert.False(openInterval.Contains(10));
            Assert.False(openInterval.Contains(12));

            //   ○--------•
            // ^ ^    ^   ^  ^
            // 0 2    7   1  1
            //            0  2
            Assert.False(leftOpenInterval.Contains(0));
            Assert.False(leftOpenInterval.Contains(2));
            Assert.True(leftOpenInterval.Contains(7));
            Assert.True(leftOpenInterval.Contains(10));
            Assert.False(leftOpenInterval.Contains(12));
            //   •--------○
            // ^ ^    ^   ^  ^
            // 0 2    7   1  1
            //            0  2
            Assert.False(rightOpenInterval.Contains(0));
            Assert.True(rightOpenInterval.Contains(2));
            Assert.True(rightOpenInterval.Contains(7));
            Assert.False(rightOpenInterval.Contains(10));
            Assert.False(rightOpenInterval.Contains(12));
            //   •--------•
            // ^ ^    ^   ^  ^
            // 0 2    7   1  1
            //            0  2
            Assert.False(closedInterval.Contains(0));
            Assert.True(closedInterval.Contains(2));
            Assert.True(closedInterval.Contains(7));
            Assert.True(closedInterval.Contains(10));
            Assert.False(closedInterval.Contains(12));

            //Intervals below (completely to left)
            //    ○-------○
            // ○-○
            Assert.False(openInterval.Contains(openBelowInterval));
            //    ○-------○
            // •-•
            Assert.False(openInterval.Contains(closedBelowInterval));
            //    •-------•
            // ○-○
            Assert.False(closedInterval.Contains(openBelowInterval));
            //    •-------•
            // •-•
            Assert.False(closedInterval.Contains(closedBelowInterval));

            //Intervals above (completely to right)
            //    ○-------○
            //               ○--○
            Assert.False(openInterval.Contains(openAboveInterval));
            //    ○-------○
            //               •--•
            Assert.False(openInterval.Contains(closedAboveInterval));
            //    •-------•
            //               ○--○
            Assert.False(closedInterval.Contains(openAboveInterval));
            //    •-------•
            //               •--•
            Assert.False(closedInterval.Contains(closedAboveInterval));

            //Intervals partially overlapping
            //    ○-------○
            //  ○----○
            Assert.False(openInterval.Contains(open0_5Interval));
            //    ○-------○
            //  •----•
            Assert.False(openInterval.Contains(closed0_5Interval));

            //    •-------•
            //           ○--○
            Assert.False(closedInterval.Contains(open9_12Interval));
            //    •-------•
            //           •--•
            Assert.False(closedInterval.Contains(closed9_12Interval));

            //Intervals larger (completely overshadowing)
            //    ○-------○
            //   ○---------○
            Assert.False(openInterval.Contains(openLargerInterval));
            //    ○-------○
            //   ○---------•
            Assert.False(openInterval.Contains(leftOpenLargerInterval));
            //    ○-------○
            //   •---------○
            Assert.False(openInterval.Contains(rightOpenLargerInterval));
            //    ○-------○
            //   •---------•
            Assert.False(openInterval.Contains(closedLargerInterval));
            //    ○-------•
            //   ○---------○
            Assert.False(leftOpenInterval.Contains(openLargerInterval));
            //    ○-------•
            //   ○---------•
            Assert.False(leftOpenInterval.Contains(leftOpenLargerInterval));
            //    ○-------•
            //   •---------○
            Assert.False(leftOpenInterval.Contains(rightOpenLargerInterval));
            //    ○-------•
            //   •---------•
            Assert.False(leftOpenInterval.Contains(closedLargerInterval));
            //    •-------○
            //   ○---------○
            Assert.False(rightOpenInterval.Contains(openLargerInterval));
            //    •-------○
            //   ○---------•
            Assert.False(rightOpenInterval.Contains(leftOpenLargerInterval));
            //    •-------○
            //   •---------○
            Assert.False(rightOpenInterval.Contains(rightOpenLargerInterval));
            //    •-------○
            //   •---------•
            Assert.False(rightOpenInterval.Contains(closedLargerInterval));
            //    •-------•
            //   ○---------○
            Assert.False(closedInterval.Contains(openLargerInterval));
            //    •-------•
            //   ○---------•
            Assert.False(closedInterval.Contains(leftOpenLargerInterval));
            //    •-------•
            //   •---------○
            Assert.False(closedInterval.Contains(rightOpenLargerInterval));
            //    •-------•
            //   •---------•
            Assert.False(closedInterval.Contains(closedLargerInterval));

            //Intervals smaller (completely within)
            //    ○-------○
            //      ○---○
            Assert.True(openInterval.Contains(openSmallerInterval));
            //    ○-------○
            //      ○---•
            Assert.True(openInterval.Contains(leftOpenSmallerInterval));
            //    ○-------○
            //      •---○
            Assert.True(openInterval.Contains(rightOpenSmallerInterval));
            //    ○-------○
            //      •---•
            Assert.True(openInterval.Contains(closedSmallerInterval));
            //    ○-------•
            //      ○---○
            Assert.True(leftOpenInterval.Contains(openSmallerInterval));
            //    ○-------•
            //      ○---•
            Assert.True(leftOpenInterval.Contains(leftOpenSmallerInterval));
            //    ○-------•
            //      •---○
            Assert.True(leftOpenInterval.Contains(rightOpenSmallerInterval));
            //    ○-------•
            //      •---•
            Assert.True(leftOpenInterval.Contains(closedSmallerInterval));
            //    •-------○
            //      ○---○
            Assert.True(rightOpenInterval.Contains(openSmallerInterval));
            //    •-------○
            //      ○---•
            Assert.True(rightOpenInterval.Contains(leftOpenSmallerInterval));
            //    •-------○
            //      •---○
            Assert.True(rightOpenInterval.Contains(rightOpenSmallerInterval));
            //    •-------○
            //      •---•
            Assert.True(rightOpenInterval.Contains(closedSmallerInterval));
            //    •-------•
            //      ○---○
            Assert.True(closedInterval.Contains(openSmallerInterval));
            //    •-------•
            //      ○---•
            Assert.True(closedInterval.Contains(leftOpenSmallerInterval));
            //    •-------•
            //      •---○
            Assert.True(closedInterval.Contains(rightOpenSmallerInterval));
            //    •-------•
            //      •---•
            Assert.True(closedInterval.Contains(closedSmallerInterval));

            //Intervals that are the same size
            //    ○-------○
            //    ○-------○
            Assert.True(openInterval.Contains(openInterval));
            //    ○-------○
            //    ○-------•
            Assert.False(openInterval.Contains(leftOpenInterval));
            //    ○-------○
            //    •-------○
            Assert.False(openInterval.Contains(rightOpenInterval));
            //    ○-------○
            //    •-------•
            Assert.False(openInterval.Contains(closedInterval));
            //    ○-------•
            //    ○-------○
            Assert.False(leftOpenInterval.Contains(openInterval));
            //    ○-------•
            //    ○-------•
            Assert.True(leftOpenInterval.Contains(leftOpenInterval));
            //    ○-------•
            //    •-------○
            Assert.False(leftOpenInterval.Contains(rightOpenInterval));
            //    ○-------•
            //    •-------•
            Assert.False(leftOpenInterval.Contains(closedInterval));
            //    •-------○
            //    ○-------○
            Assert.False(rightOpenInterval.Contains(openInterval));
            //    •-------○
            //    ○-------•
            Assert.False(rightOpenInterval.Contains(leftOpenInterval));
            //    •-------○
            //    •-------○
            Assert.True(rightOpenInterval.Contains(rightOpenInterval));
            //    •-------○
            //    •-------•
            Assert.False(rightOpenInterval.Contains(closedInterval));
            //    •-------•
            //    ○-------○
            Assert.True(closedInterval.Contains(openInterval));
            //    •-------•
            //    ○-------•
            Assert.True(closedInterval.Contains(leftOpenInterval));
            //    •-------•
            //    •-------○
            Assert.True(closedInterval.Contains(rightOpenInterval));
            //    •-------•
            //    •-------•
            Assert.True(closedInterval.Contains(closedInterval));
        }

        [Test]
        public void TestTree() {
            var tree = CreateTree(TestEntries().OrderBy(interval => interval.Start));

            // Left-Closed with Right-Closed
            var value = new Interval<int>(0, 25, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(2, tree.Search(value).Count());

            value = new Interval<int>(25, 50, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(3, tree.Search(value).Count());

            value = new Interval<int>(50, 70, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(3, tree.Search(value).Count());

            value = new Interval<int>(70, 140, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(3, tree.Search(value).Count());

            value = new Interval<int>(140, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(2, tree.Search(value).Count());

            value = new Interval<int>(0, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(5, tree.Search(value).Count());

            value = new Interval<int>(0, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(5, tree.Search(value).Count());

            value = new Interval<int>(0, 60, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(4, tree.Search(value).Count());

            value = new Interval<int>(60, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Closed);
            Assert.AreEqual(4, tree.Search(value).Count());


            // Left-Closed with Right-Open
            value = new Interval<int>(0, 25, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(2, tree.Search(value).Count());

            value = new Interval<int>(25, 50, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(2, tree.Search(value).Count());

            value = new Interval<int>(50, 70, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(3, tree.Search(value).Count());

            value = new Interval<int>(70, 140, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(2, tree.Search(value).Count());

            value = new Interval<int>(140, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(2, tree.Search(value).Count());

            value = new Interval<int>(0, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(5, tree.Search(value).Count());

            value = new Interval<int>(0, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(5, tree.Search(value).Count());

            value = new Interval<int>(0, 60, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(3, tree.Search(value).Count());

            value = new Interval<int>(60, 560, Interval<int>.IntervalType.Closed, Interval<int>.IntervalType.Open);
            Assert.AreEqual(4, tree.Search(value).Count());

            value = new Interval<int>(60, 560, Interval<int>.IntervalType.Open, Interval<int>.IntervalType.Open);
            Assert.AreEqual(3, tree.Search(value).Count());

            Assert.AreEqual(2, tree.Search(0).Count());
            Assert.AreEqual(2, tree.Search(15).Count());
            Assert.AreEqual(1, tree.Search(30).Count());
            Assert.AreEqual(2, tree.Search(57).Count());
            Assert.AreEqual(2, tree.Search(60).Count());
            Assert.AreEqual(2, tree.Search(62).Count());
            Assert.AreEqual(1, tree.Search(100).Count());
            Assert.AreEqual(2, tree.Search(500).Count());
        }

        private static IntervalTree<int> CreateTree(IEnumerable<Interval<int>> entries)
        {
            var tree = new IntervalTree<int>();

            foreach (var interval in entries)
            {
                tree.Add(interval);
            }

            return tree;
        }
    }
}