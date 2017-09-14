// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		public static string Replace(this string text, string pattern, string replace,
			System.Text.RegularExpressions.RegexOptions options)
		{
			return System.Text.RegularExpressions.Regex.Replace(text, pattern, replace, options);
		}
	}
}