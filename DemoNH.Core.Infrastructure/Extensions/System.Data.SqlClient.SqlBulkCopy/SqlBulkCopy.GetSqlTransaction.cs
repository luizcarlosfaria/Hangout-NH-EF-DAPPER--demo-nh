// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;
using System.Data.SqlClient;
using System.Reflection;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>42B25882-5BC7-4550-8451-551AEFE517DC</id>
		/// <summary>
		///     A SqlBulkCopy extension method that return the SqlTransaction from the SqlBulkCopy.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The SqlTransaction from the SqlBulkCopy.</returns>
		public static SqlTransaction GetTransaction(this SqlBulkCopy @this)
		{
			Type type = @this.GetType();
			FieldInfo field = type.GetField("_externalTransaction", BindingFlags.NonPublic | BindingFlags.Instance);
			return field.GetValue(@this) as SqlTransaction;
		}
	}
}