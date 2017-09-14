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
		/// <id>169EFAF3-CEDD-4598-9682-DFF96A04CB6F</id>
		/// <summary>
		///     Executes the query, and returns the result set as DataSet.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>A DataSet that is equivalent to the result set.</returns>
		public static DataSet ExecuteDataSet(this SqlCommand @this)
		{
			var ds = new DataSet();
			using (var sqlDataAdapter = new SqlDataAdapter(@this))
			{
				sqlDataAdapter.Fill(ds);
			}

			return ds;
		}
	}
}