// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>E5F43DD5-F2AC-40CF-8CCC-B876DEFAC29A</id>
		/// <summary>
		///     A string extension method that URL encode a string and returns the encoded string.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The encoded string.</returns>
		public static string UrlEncode(this string @this)
		{
			return HttpUtility.UrlEncode(@this);
		}
	}
}