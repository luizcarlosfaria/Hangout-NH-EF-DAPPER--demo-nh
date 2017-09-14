// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>7DAD6857-0E4B-4B9B-99FD-1C764755831B</id>
		/// <summary>
		///     A string extension method that Html encode a string and returns the encoded string.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The encoded string.</returns>
		public static string HtmlEncode(this string @this)
		{
			return HttpUtility.HtmlEncode(@this);
		}
	}
}