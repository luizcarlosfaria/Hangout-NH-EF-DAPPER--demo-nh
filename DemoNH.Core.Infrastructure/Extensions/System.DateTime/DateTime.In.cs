// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		public static bool In(this DateTime? date, DateRange dateRange)
		{
			if (date >= dateRange.StartDate && date <= dateRange.EndDate)
			{
				return true;
			}

			return false;
		}
	}
}