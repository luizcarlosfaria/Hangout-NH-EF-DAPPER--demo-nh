using DemoNH.Core.Infrastructure.Extensions;

namespace DemoNH.Core.Infrastructure.FFMpeg.Formats
{
	public class MediaFormat
	{
		public string Arg { get { return "-f {0}".FormatWith(this.Name); } }

		public string Name { get; private set; }

		public string Extension { get; private set; }

		protected MediaFormat(string name, string extension)
		{
			this.Name = name;
			this.Extension = extension;
		}
	}
}