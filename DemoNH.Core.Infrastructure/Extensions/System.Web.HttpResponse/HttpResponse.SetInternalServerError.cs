// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>B2F67928-6EEB-42E2-A984-020FF1CB65E8</id>
		/// <summary>
		///     A HttpResponse extension method that sets the response to status code 500 (Internal Server Error).
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		public static void SetInternalServerError(this HttpResponse @this)
		{
			@this.StatusCode = 500;
			@this.StatusDescription = "Internal Server Error";
		}
	}
}