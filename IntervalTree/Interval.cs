using System;

namespace IntervalTree
{
    /// <summary>
    /// Representation of bounded interval
    /// </summary>
    /// <typeparam name="T">type of interval bounds</typeparam>
    public struct Interval<T> : IComparable<Interval<T>> where T: struct, IComparable<T>
    {
        public T Start
        {
            get;
            set;
        }

        public T End
        {
            get;
            set;
        }

        public IntervalType LowerBoundIntervalType
        {
            get;
            set;
        }

        public IntervalType UpperBoundIntervalType
        {
            get;
            set;
        }

        /// <summary>
        /// Represents vectorless interval of the form [start, end], [start, end), (start, end] or (start, end).
        /// </summary>
        /// <param name="start">start bound</param>
        /// <param name="end">end bound</param>
        /// <param name="lowerboundIntervalType">open or closed interval</param>
        /// <param name="upperboundIntervalType">open or closed interval</param>
        /// <remarks>Vectorless interval, if end is larger than start, the interval will swap the two so that start %lt; end.</remarks>
        public Interval(T start, T end, IntervalType lowerboundIntervalType = IntervalType.Closed, IntervalType upperboundIntervalType = IntervalType.Closed)
            : this()
        {
            var a = start;
            var b = end;
            var comparison = a.CompareTo(b);

            if (comparison > 0) {
                a = end;
                b = start;
            }

            Start = a;
            End = b;

            LowerBoundIntervalType = lowerboundIntervalType;
            UpperBoundIntervalType = upperboundIntervalType;
        }

        /// <summary>
        /// Tests if interval contains given interval
        /// </summary>
        /// <param name="interval">interval to check</param>
        /// <returns>True when interval is contained in the other interval</returns>
        public bool Contains(Interval<T> interval)
        {
            if (Start.GetType() != typeof(T) || End.GetType() != typeof(T)) {
                throw new ArgumentException("Type mismatch", "interval");
            }

            if (this.Equals(interval)) {
                return true;
            }

            var lower = LowerBoundIntervalType == IntervalType.Open
                ? Start.CompareTo(interval.Start) < 0
                : Start.CompareTo(interval.Start) <= 0;
            var upper = UpperBoundIntervalType == IntervalType.Open
                ? End.CompareTo(interval.End) > 0
                : End.CompareTo(interval.End) >= 0;

            return lower && upper;
        }

        /// <summary>
        /// Tests if interval contains given value
        /// </summary>
        /// <param name="val">value to check</param>
        /// <returns>True when value is contained in the interval</returns>
        public bool Contains(T val)
        {
            if (Start.GetType() != typeof(T) || End.GetType() != typeof(T)) {
                throw new ArgumentException("Type mismatch", "val");
            }

            var lower = LowerBoundIntervalType == IntervalType.Open
                ? Start.CompareTo(val) < 0
                : Start.CompareTo(val) <= 0;
            var upper = UpperBoundIntervalType == IntervalType.Open
                ? End.CompareTo(val) > 0
                : End.CompareTo(val) >= 0;

            return lower && upper;
        }

        /// <summary>
        /// Check if the intervals overlap at any point
        /// </summary>
        /// <param name="interval">interval to check overlap</param>
        /// <returns>True when intervals overlap</returns>
        public bool Overlaps(Interval<T> interval)
        {
            if (Start.GetType() != typeof(T) || End.GetType() != typeof(T)) {
                throw new ArgumentException("Type mismatch", "interval");
            }

            var lower = LowerBoundIntervalType == IntervalType.Open
                ? Start.CompareTo(interval.End) < 0
                : Start.CompareTo(interval.End) <= 0;
            var upper = UpperBoundIntervalType == IntervalType.Open
                ? End.CompareTo(interval.Start) > 0
                : End.CompareTo(interval.Start) >= 0;

            return lower && upper;
        }

        /// <summary>
        /// Compare two intervals
        /// </summary>
        /// <param name="i">interval to compare</param>
        /// <returns></returns>
        public int CompareTo(Interval<T> i)
        {
            if (LowerBoundIntervalType == i.LowerBoundIntervalType && UpperBoundIntervalType == i.UpperBoundIntervalType) {
                // Both pairs of lower and upper intervaltypes are the same as their partner ()-(), []-[], (]-(], [)-[)
                //    L         R
                // ○-----○ & ○-----○
                // ○-----• & ○-----•
                // •-----○ & •-----○
                // •-----• & •-----•
                return (Start.CompareTo(i.Start) == -1) ? -1 :
                (Start.CompareTo(i.Start) == 1) ? 1 :
                       End.CompareTo(i.End);
            } else if (LowerBoundIntervalType != i.LowerBoundIntervalType && UpperBoundIntervalType == i.UpperBoundIntervalType) {
                // Lower interval types are opposite their partner ()-[), [)-(), (]-[], []-(]
                //    L         R
                // ○-----○ & •-----○
                // ○-----• & •-----•
                // •-----○ & ○-----○
                // •-----• & ○-----•
                return (LowerBoundIntervalType == IntervalType.Open && i.LowerBoundIntervalType == IntervalType.Closed && Start.CompareTo(i.Start) == 0) ? 1 :
                       (LowerBoundIntervalType == IntervalType.Closed && i.LowerBoundIntervalType == IntervalType.Open && Start.CompareTo(i.Start) == 0) ? -1 :
                       (Start.CompareTo(i.Start) == -1) ? -1 :
                       (Start.CompareTo(i.Start) == 1) ? 1 :
                       End.CompareTo(i.End);
            } else if (LowerBoundIntervalType == i.LowerBoundIntervalType && UpperBoundIntervalType != i.UpperBoundIntervalType) {
                // Upper interval types are opposite their partner ()-(], [)-[], (]-(), []-[)
                //    L         R
                // ○-----○ & ○-----•
                // ○-----• & ○-----○
                // •-----○ & •-----•
                // •-----• & •-----○
                return (Start.CompareTo(i.Start) == -1) ? -1 :
                       (Start.CompareTo(i.Start) == 1) ? 1 :
                       (UpperBoundIntervalType == IntervalType.Open && i.UpperBoundIntervalType == IntervalType.Closed && End.CompareTo(i.End) == 0) ? -1 :
                       (UpperBoundIntervalType == IntervalType.Closed && i.UpperBoundIntervalType == IntervalType.Open && End.CompareTo(i.End) == 0) ? 1 : 0;
            } else if (LowerBoundIntervalType != i.LowerBoundIntervalType && UpperBoundIntervalType != i.UpperBoundIntervalType) {
                // Both pairs of lower and upper intervaltypes are opposite of their partner ()-[], (]-[), [)-(], []-()
                //    L         R
                // ○-----○ & •-----•
                // ○-----• & •-----○
                // •-----○ & ○-----•
                // •-----• & ○-----○
                return (LowerBoundIntervalType == IntervalType.Open && i.LowerBoundIntervalType == IntervalType.Closed && Start.CompareTo(i.Start) == 0) ? 1 :
                       (LowerBoundIntervalType == IntervalType.Closed && i.LowerBoundIntervalType == IntervalType.Open && Start.CompareTo(i.Start) == 0) ? -1 :
                       (Start.CompareTo(i.Start) == -1) ? -1 :
                       (Start.CompareTo(i.Start) == 1) ? 1 :
                       (UpperBoundIntervalType == IntervalType.Open && i.UpperBoundIntervalType == IntervalType.Closed && End.CompareTo(i.End) == 0) ? -1 :
                       (UpperBoundIntervalType == IntervalType.Closed && i.UpperBoundIntervalType == IntervalType.Open && End.CompareTo(i.End) == 0) ? 1 :
                       End.CompareTo(i.End);
            }

            // Identical interval
            return 0;
        }

        /// <summary>
        /// Display using mathematical notation (, ), [, and ].
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}{1}, {2}{3}",
                LowerBoundIntervalType == IntervalType.Open ? "(" : "[",
                Start.ToString(),
                End.ToString(),
                UpperBoundIntervalType == IntervalType.Open ? ")" : "]"
            );
        }

        /// <summary>
        /// An interval could be open and closed or combination of both at either end.
        /// </summary>
        public enum IntervalType
        {
            Open,
            Closed
        }
    }
}
