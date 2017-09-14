﻿// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.IO;
using System.Text;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>998BC005-0960-410F-A0E3-D069141BAA3C</id>
		/// <summary>
		///     A FileInfo extension method that reads the file to the end.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <param name="position">(Optional) the position.</param>
		/// <returns>
		///     The rest of the stream as a string, from the current position to the end. If the current position is at the
		///     end of the stream, returns an empty string ("").
		/// </returns>
		public static string ReadToEnd(this FileInfo @this, long position = 0)
		{
			using (FileStream stream = File.Open(@this.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				stream.Position = position;

				using (var reader = new StreamReader(stream, Encoding.Default))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}