// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		public static Dictionary<string, string> ToDictionary(this string[] stringArray)
		{
			Dictionary<string, string> returnDic = new Dictionary<string, string>();
			if (stringArray.Length > 0)
			{
				if (stringArray.Length % 2 != 0)
					throw new InvalidOperationException("Tags não possui uma quantidade de valores par;");
				else
				{
					int keyIndex = 0;
					int valueIndex = 1;
					for (; valueIndex < stringArray.Length; keyIndex += 2, valueIndex += 2)
					{
						returnDic.Add(stringArray[keyIndex], stringArray[valueIndex]);
					}
				}
			}
			return returnDic;
		}
	}
}