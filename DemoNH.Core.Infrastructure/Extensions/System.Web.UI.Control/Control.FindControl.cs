// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web.UI;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>F04DDC17-7827-4684-82F2-06CA6ED182C6</id>
		/// <summary>
		///     Searches the current naming container for a server control with the specified id parameter.
		/// </summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="this">The @this to act on.</param>
		/// <param name="id">The identifier for the control to be found.</param>
		/// <returns>The specified control, or a null reference if the specified control does not exist.</returns>
		public static T FindControl<T>(this Control @this, string id) where T : class
		{
			return (@this.FindControl(id) as T);
		}
	}
}