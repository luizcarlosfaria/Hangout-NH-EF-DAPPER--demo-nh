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
		/// <id>25F8D0EB-2F45-4520-A921-028048E04DD5</id>
		/// <summary>
		///     A SqlBulkCopy extension method that return the SqlConnection from the SqlBulkCopy.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The SqlConnection from the SqlBulkCopy.</returns>
		public static SqlConnection GetSqlConnection(this SqlBulkCopy @this)
		{
			Type type = @this.GetType();
			FieldInfo field = type.GetField("_connection", BindingFlags.NonPublic | BindingFlags.Instance);
			return field.GetValue(@this) as SqlConnection;
		}
	}
}