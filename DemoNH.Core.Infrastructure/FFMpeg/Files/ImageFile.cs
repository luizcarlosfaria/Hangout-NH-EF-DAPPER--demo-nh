using System;

namespace DemoNH.Core.Infrastructure.FFMpeg.Files
{
	public class ImageFile : MediaFile
	{
		public ImageFile(string fileName)
			: base(fileName)
		{
		}

		public override Formats.MediaFormat Format
		{
			get { throw new NotImplementedException(); }
		}
	}
}