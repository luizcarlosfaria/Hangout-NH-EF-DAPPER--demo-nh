// Copyright (c) 2013 Jonathan Magnan (http://zzzportal.com)
// All rights reserved.
// Licensed under MIT License (MIT)
// License can be found here: https://zextensionmethods.codeplex.com/license

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <id>6EFD4348-77DF-49D9-AB9B-A838F9921B18</id>
		/// <summary>
		///     An Image extension method that scales an image to the specific ratio.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <param name="ratio">The ratio.</param>
		/// <returns>The scaled image to the specific ratio.</returns>
		public static Image Scale(this Image @this, double ratio)
		{
			int width = Convert.ToInt32(@this.Width * ratio);
			int height = Convert.ToInt32(@this.Height * ratio);

			var r = new Bitmap(width, height);

			using (Graphics g = Graphics.FromImage(r))
			{
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;

				g.DrawImage(@this, 0, 0, width, height);
			}

			return r;
		}

		/// <id>D1432151-54D8-4F0F-AADF-FC2B0F78DA5E</id>
		/// <summary>
		///     An Image extension method that scales an image to a specific with and height.
		/// </summary>
		/// <param name="this">The @this to act on.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The scaled image to the specific width and height.</returns>
		public static Image Scale(this Image @this, int width, int height)
		{
			var r = new Bitmap(width, height);

			using (Graphics g = Graphics.FromImage(r))
			{
				g.CompositingQuality = CompositingQuality.HighQuality;
				g.SmoothingMode = SmoothingMode.HighQuality;
				g.InterpolationMode = InterpolationMode.HighQualityBicubic;

				g.DrawImage(@this, 0, 0, width, height);
			}

			return r;
		}
	}
}