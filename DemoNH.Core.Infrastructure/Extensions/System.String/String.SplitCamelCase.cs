// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		public static string SplitCamelCase(this string text)
		{
			string returnValue =
				System.Text.RegularExpressions.Regex.Replace(text, "([A-Z])", " $1",
					System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
			return returnValue;
		}
	}
}