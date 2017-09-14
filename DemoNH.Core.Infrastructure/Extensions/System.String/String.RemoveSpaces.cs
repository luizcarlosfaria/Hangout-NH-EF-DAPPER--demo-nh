// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <summary>
		/// remove white space, not line end
		/// Useful when parsing user input such phone,
		/// price int.Parse("1 000 000".RemoveSpaces(),.....
		/// </summary>
		/// <param name="s"></param>
		/// <param name="value">string without spaces</param>
		public static string RemoveSpaces(this string s)
		{
			return s.Replace(" ", "");
		}
	}
}