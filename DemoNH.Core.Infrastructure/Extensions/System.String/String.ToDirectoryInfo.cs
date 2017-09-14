// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.IO;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>39E6258C-FDDD-4F5B-8C5E-CB62BBBBD017</id>
		/// <summary>
		///     A string extension method that converts the @this to a directory information.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <returns>@this as a DirectoryInfo.</returns>
		/// <example>
		///     <code>
		///            var directoryName = @"c:\";
		///
		///             var directoryInfo = directoryName.ToDirectoryInfo();
		///     </code>
		/// </example>
		public static DirectoryInfo ToDirectoryInfo(this string @this)
		{
			return new DirectoryInfo(@this);
		}
	}
}