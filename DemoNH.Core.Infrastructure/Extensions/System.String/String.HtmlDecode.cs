// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>499D9E16-3964-4077-A8F9-288E7F3362A9</id>
		/// <summary>
		///     A string extension method that Html decode a string and returns the decoded string..
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The decoded string.</returns>
		public static string HtmlDecode(this string @this)
		{
			return HttpUtility.HtmlDecode(@this);
		}
	}
}