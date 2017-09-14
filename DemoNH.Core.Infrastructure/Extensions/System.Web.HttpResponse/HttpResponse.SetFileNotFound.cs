// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>1671D1DB-35BD-410B-9CAC-AC02F06810F7</id>
		/// <summary>
		///     A HttpResponse extension method that sets the response to status code 404 (Not Found).
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		public static void SetFileNotFound(this HttpResponse @this)
		{
			@this.StatusCode = 404;
			@this.StatusDescription = "Not Found";
		}
	}
}