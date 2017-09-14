// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>EB712E90-40C6-4DE0-96D8-67FE863C559B</id>
		/// <summary>
		///     A string extension method that URL decode a string and returns the decoded string..
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The decoded string.</returns>
		public static string UrlDecode(this string @this)
		{
			return HttpUtility.UrlDecode(@this);
		}
	}
}