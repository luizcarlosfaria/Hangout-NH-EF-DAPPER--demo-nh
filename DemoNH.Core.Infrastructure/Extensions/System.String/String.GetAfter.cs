﻿// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>EB254584-EAAA-4D73-A819-D38A2D41A35E</id>
		/// <summary>
		///     A string extension method that get the string after the specified string.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <param name="value">The value to search.</param>
		/// <returns>The string after the specified value.</returns>
		public static string GetAfter(this string @this, string value)
		{
			int startIndex = @this.IndexOf(value);
			return startIndex == -1 ? String.Empty : @this.Substring(startIndex + value.Length);
		}
	}
}