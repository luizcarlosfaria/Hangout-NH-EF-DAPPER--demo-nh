using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DemoNH.Core.Domain.Support
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class Range<T>
		where T : struct, IComparable
	{
		[DataMember]
		public T Start { get; private set; }

		[DataMember]
		public T End { get; private set; }

		public Range()
		{
			this.Start = default(T);
			this.End = default(T);
		}

		public Range(T start, T end)
		{
			if (start.CompareTo(end) > 0)
				throw new ArgumentOutOfRangeException("The constructor parameter 'start' must be less then 'end' parameter");

			this.Start = start;
			this.End = end;
		}

		public bool IsIn(T itemToCompare)
		{
			int startComparisonResult = this.Start.CompareTo(itemToCompare);
			int endComparisonResult = this.End.CompareTo(itemToCompare);
			return (startComparisonResult <= 0) && (endComparisonResult >= 0);
		}

		public bool Contains(Range<T> itemToCompare)
		{
			return (this.IsIn(itemToCompare.Start) && this.IsIn(itemToCompare.End));
		}

	}

	[Serializable]
	[DataContract(IsReference = true)]
	public class TimeSpanRange : Range<TimeSpan>
	{
		public TimeSpanRange() : base() { }
		public TimeSpanRange(TimeSpan start, TimeSpan end) : base(start, end) { }
	}

    [Serializable]
    [DataContract(IsReference = true)]
    public class DateTimeRange : Range<DateTime>
    {
        public DateTimeRange() : base() { }
        public DateTimeRange(DateTime start, DateTime end) : base(start, end) { }
    }
	[Serializable]
	[DataContract(IsReference = true)]
	public class LongRange : Range<long>
	{
		public LongRange() : base() { }
		public LongRange(long start, long end) : base(start, end) { }
	}
	[Serializable]
	[DataContract(IsReference = true)]
	public class IntRange : Range<int>
	{
		public IntRange() : base() { }
		public IntRange(int start, int end) : base(start, end) { }
	}

}
