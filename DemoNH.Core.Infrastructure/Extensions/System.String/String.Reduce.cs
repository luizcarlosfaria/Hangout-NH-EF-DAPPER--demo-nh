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
		/// Reduce string to shorter preview which is optionally ended by some string (...).
		/// </summary>
		/// <param name="s">string to reduce</param>
		/// <param name="count">Length of returned string including endings.</param>
		/// <param name="endings">optional edings of reduced text</param>
		/// <example>
		/// string description = "This is very long description of something";
		/// string preview = description.Reduce(20,"...");
		/// produce -> "This is very long..."
		/// </example>
		/// <returns></returns>
		public static string Reduce(this string s, int count, string endings)
		{
			if (count < endings.Length)
				throw new Exception("Failed to reduce to less then endings length.");
			int sLength = s.Length;
			int len = sLength;
			if (endings != null)
				len += endings.Length;
			if (count > sLength)
				return s; //it's too short to reduce
			s = s.Substring(0, sLength - len + count);
			if (endings != null)
				s += endings;
			return s;
		}
	}
}