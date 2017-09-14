// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Linq;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <summary>
		/// Aplica Camel Case em uma determinada string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static System.String CamelCaseSpecial(this System.String text)
		{
			char[] textChars = text.ToArray();
			char[] outputList = new char[textChars.Length];
			bool findNextItem = false;
			for (int i = 0; i < textChars.Length; i++)
			{
				char currentChar = textChars[i];
				if (findNextItem == false && char.IsLetter(currentChar))
				{
					outputList[i] = char.ToUpper(currentChar);
					findNextItem = true;
				}
				else
					outputList[i] = char.ToLower(currentChar);
			}
			return string.Concat(outputList);
		}
	}
}