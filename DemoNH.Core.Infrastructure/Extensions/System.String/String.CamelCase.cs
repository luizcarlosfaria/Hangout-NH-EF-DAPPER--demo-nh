// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <summary>
		/// Aplica Camel Case em uma determinada string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static System.String CamelCase(this System.String text)
		{
			return OragonExtensions.ApplyPattern(text, it => it.ToUpper(), it => it.ToLower());
		}
	}
}