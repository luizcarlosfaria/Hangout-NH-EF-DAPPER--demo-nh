// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Web.UI;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>2D165574-6416-44FB-990E-CEABB6D0059A</id>
		/// <summary>
		///     Searches recursively in the container and child container for a server control with the specified id parameter.
		/// </summary>
		/// <typeparam name="T">Generic type parameter.</typeparam>
		/// <param name="this">The @this to act on.</param>
		/// <param name="id">The identifier for the control to be found.</param>
		/// <returns>The specified control, or a null reference if the specified control does not exist.</returns>
		public static T FindControlRecursive<T>(this Control @this, string id) where T : class
		{
			Control rControl = @this.FindControl(id);

			if (rControl == null)
			{
				foreach (Control control in @this.Controls)
				{
					rControl = control.FindControl(id);
					if (rControl != null)
					{
						break;
					}
				}
			}

			return rControl as T;
		}

		/// <id>FAC21F43-0B83-4437-8187-65888AE16187</id>
		/// <summary>
		///     Searches recursively in the container and child container for a server control with the specified id
		///     parameter.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <param name="id">The identifier for the control to be found.</param>
		/// <returns>The specified control, or a null reference if the specified control does not exist.</returns>
		public static Control FindControlRecursive(this Control @this, string id)
		{
			Control rControl = @this.FindControl(id);

			if (rControl == null)
			{
				foreach (Control control in @this.Controls)
				{
					rControl = control.FindControl(id);
					if (rControl != null)
					{
						break;
					}
				}
			}

			return rControl;
		}
	}
}