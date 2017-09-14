// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Net;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>DDF8DE4A-A808-43FA-80FC-7BD1373F1A23</id>
		/// <summary>
		///     A WebRequest extension method that gets the WebRequest response or the WebException response.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The WebRequest response or WebException response.</returns>
		public static WebResponse GetResponseSafe(this WebRequest @this)
		{
			try
			{
				return @this.GetResponse();
			}
			catch (WebException e)
			{
				return e.Response;
			}
		}
	}
}