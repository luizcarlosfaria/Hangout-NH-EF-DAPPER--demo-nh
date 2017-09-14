// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>42AB63B0-0D36-46FF-942F-2B10AE4517BD</id>
		/// <summary>
		///     A bool extension method that execute an Action if the value is true.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <param name="action">The action to execute.</param>
		public static void IfTrue(this bool @this, Action action)
		{
			if (@this)
			{
				action();
			}
		}
	}
}