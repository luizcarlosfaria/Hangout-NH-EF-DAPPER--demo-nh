// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>A9DB7F39-68F3-4D7F-91ED-BA3A09AE46E7</id>
		/// <summary>
		///     A DateTime extension method that return a DateTime with the time set to "23:59:59:999". The last moment of the
		///     day.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>A DateTime of the day with the time set to "23:59:59".</returns>
		public static void Reduce(this List<DateRange> dateRanges)
		{
			var reducedList = new List<DateRange>();

			var orderedDateRanges = dateRanges.OrderBy(dr => dr.StartDate).ToList();

			reducedList.Add(orderedDateRanges.First());

			foreach (DateRange dateRange in orderedDateRanges)
			{
				if (dateRange.StartDate.In(reducedList.Last()))
				{
					if (reducedList.Last().EndDate < dateRange.EndDate)
					{
						reducedList.Last().EndDate = dateRange.EndDate;
					}
				}
				else
				{
					reducedList.Add(dateRange);
				}
			}

			dateRanges.Clear();

			if (reducedList.Any())
			{
				dateRanges.AddRange(reducedList);
			}
		}
	}
}