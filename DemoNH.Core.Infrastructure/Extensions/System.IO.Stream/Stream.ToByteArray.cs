// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.IO;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>635D9852-FF20-4DF8-B511-4F3B08103596</id>
		/// <summary>
		///     A Stream extension method that converts the Stream to a byte array.
		/// </summary>
		/// <param name="this">The Stream to act on.</param>
		/// <returns>The Stream as a byte[].</returns>
		public static byte[] ToByteArray(this Stream @this)
		{
			using (var ms = new MemoryStream())
			{
				@this.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}