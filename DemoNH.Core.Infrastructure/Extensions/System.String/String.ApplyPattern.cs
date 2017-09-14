// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <summary>
		/// Aplica padrões diferenciados para primeira letra e resto em uma determinada senteñça string
		/// </summary>
		/// <param name="text"></param>
		/// <param name="first"></param>
		/// <param name="rest"></param>
		/// <returns></returns>
		public static System.String ApplyPattern(this System.String text, Func<string, string> first,
			Func<string, string> rest)
		{
			System.String firstLetter = first(text.Substring(0, (1) - (0)));
			System.String restLetters = rest(text.Substring(1));
			return firstLetter + restLetters;
		}
	}
}