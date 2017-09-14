using DemoNH.Core.Infrastructure.FFMpeg.Formats;

namespace DemoNH.Core.Infrastructure.FFMpeg.Files
{
	public abstract class MediaFile
	{
		public string FileName { get; private set; }

		public MediaFile(string fileName)
		{
			this.FileName = fileName;
		}

		private string extension;

		public string Extension
		{
			get
			{
				if (this.extension == null)
					this.extension = System.IO.Path.GetExtension(this.FileName).ToLower();
				return this.extension;
			}
		}

		public bool Exists
		{
			get
			{
				return System.IO.File.Exists(this.FileName);
			}
		}

		public abstract MediaFormat Format { get; }
	}
}