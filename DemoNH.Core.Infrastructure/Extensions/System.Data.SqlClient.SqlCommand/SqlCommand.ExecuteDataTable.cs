// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Data;
using System.Data.SqlClient;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>D7EBF08E-DD54-4E87-BB26-9E04F7614257</id>
		/// <summary>
		///     Executes the query, and returns the first result set as DataTable.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>A DataTable that is equivalent to the first result set.</returns>
		public static DataTable ExecuteDataTable(this SqlCommand @this)
		{
			var ds = new DataSet();
			using (var sqlDataAdapter = new SqlDataAdapter(@this))
			{
				sqlDataAdapter.Fill(ds);
			}

			return ds.Tables[0];
		}
	}
}