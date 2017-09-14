// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>46D812BA-C9CE-476A-B1E6-C3E6E59B5630</id>
		/// <summary>
		///     A DataColumnCollection extension method that converts an obj to a list.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>obj as a List&lt;DataColumn&gt;</returns>
		public static List<DataColumn> ToList(this DataColumnCollection @this)
		{
			return @this.Cast<DataColumn>().Select(x => x).ToList();
		}
	}
}