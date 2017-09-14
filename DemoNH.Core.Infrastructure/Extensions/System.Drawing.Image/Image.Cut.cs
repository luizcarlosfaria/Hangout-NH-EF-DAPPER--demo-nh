// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System.Drawing;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>15F029B4-36FB-49E7-A7DC-EA063AC4C6A3</id>
		/// <summary>
		///     An Image extension method that cuts an image.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The cutted image.</returns>
		public static Image Cut(this Image @this, int width, int height, int x, int y)
		{
			var r = new Bitmap(width, height);
			var destinationRectange = new Rectangle(0, 0, width, height);
			var sourceRectangle = new Rectangle(x, y, width, height);

			using (Graphics g = Graphics.FromImage(r))
			{
				g.DrawImage(@this, destinationRectange, sourceRectangle, GraphicsUnit.Pixel);
			}

			return r;
		}
	}
}