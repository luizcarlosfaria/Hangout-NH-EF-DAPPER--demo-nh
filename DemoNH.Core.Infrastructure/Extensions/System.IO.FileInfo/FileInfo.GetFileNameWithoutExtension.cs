// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.IO;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>03F50E06-048A-4D8E-B503-3DD5C420EB2C</id>
		/// <summary>
		///     A FileInfo extension method that gets file name without extension.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>The file name without extension.</returns>
		public static string GetFileNameWithoutExtension(this FileInfo @this)
		{
			return Path.GetFileNameWithoutExtension(@this.Name);
		}
	}
}