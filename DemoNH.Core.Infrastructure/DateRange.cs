using DemoNH.Core.Infrastructure.Extensions;
using System;
using System.Diagnostics;

namespace DemoNH.Core.Infrastructure
{
	[Serializable]
	public class DateRange : IEquatable<DateRange>
	{
		private Nullable<DateTime> startDate, endDate;

		public DateRange()
			: this(new Nullable<DateTime>(), new Nullable<DateTime>())
		{
		}

		public DateRange(int ano)
			: this(new DateTime(ano, 1, 1), new DateTime(ano, 12, 31).EndOfDay())
		{
		}

		[DebuggerStepThrough]
		public DateRange(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
		{
			AssertStartDateFollowsEndDate(startDate, endDate);
			this.startDate = startDate;
			this.endDate = endDate;
		}

		public Nullable<TimeSpan> TimeSpan
		{
			get { return endDate - startDate; }
		}

		public Nullable<DateTime> StartDate
		{
			get { return startDate; }
			set
			{
				AssertStartDateFollowsEndDate(value, this.endDate);
				startDate = value;
			}
		}

		public Nullable<DateTime> EndDate
		{
			get { return endDate; }
			set
			{
				AssertStartDateFollowsEndDate(this.startDate, value);
				endDate = value;
			}
		}

		[DebuggerStepThrough]
		private void AssertStartDateFollowsEndDate(Nullable<DateTime> startDate, Nullable<DateTime> endDate)
		{
			if ((startDate.HasValue && endDate.HasValue) &&
				(endDate.Value < startDate.Value))
				throw new InvalidOperationException("Start Date must be less than or equal to End Date");
		}

		[DebuggerStepThrough]
		public DateRange GetIntersection(DateRange other)
		{
			if (!Intersects(other)) throw new InvalidOperationException("DateRanges do not intersect");
			return new DateRange(GetLaterStartDate(other.StartDate), GetEarlierEndDate(other.EndDate));
		}

		private Nullable<DateTime> GetLaterStartDate(Nullable<DateTime> other)
		{
			return Nullable.Compare<DateTime>(startDate, other) >= 0 ? startDate : other;
		}

		private Nullable<DateTime> GetEarlierEndDate(Nullable<DateTime> other)
		{
			//!endDate.HasValue == +infinity, not negative infinity
			//as is the case with !startDate.HasValue
			if (Nullable.Compare<DateTime>(endDate, other) == 0) return other;
			if (endDate.HasValue && !other.HasValue) return endDate;
			if (!endDate.HasValue && other.HasValue) return other;
			return (Nullable.Compare<DateTime>(endDate, other) >= 0) ? other : endDate;
		}

		public bool Intersects(DateRange other)
		{
			if ((this.startDate.HasValue && other.EndDate.HasValue &&
				other.EndDate.Value < this.startDate.Value) ||
				(this.endDate.HasValue && other.StartDate.HasValue &&
				other.StartDate.Value > this.endDate.Value) ||
				(other.StartDate.HasValue && this.endDate.HasValue &&
				this.endDate.Value < other.StartDate.Value) ||
				(other.EndDate.HasValue && this.startDate.HasValue &&
				this.startDate.Value > other.EndDate.Value))
			{
				return false;
			}
			return true;
		}

		public bool Equals(DateRange other)
		{
			if (object.ReferenceEquals(other, null)) return false;
			return ((startDate == other.StartDate) && (endDate == other.EndDate));
		}
	}

	public static class DateRangeExtensions
	{
		public static bool IsIn(this DateTime date, DateRange interval)
		{
			bool returnValue = true;

			if (interval.StartDate.HasValue)
				returnValue &= (date >= interval.StartDate);

			if (interval.EndDate.HasValue)
				returnValue &= (date <= interval.EndDate);

			return returnValue;
		}
	}
}