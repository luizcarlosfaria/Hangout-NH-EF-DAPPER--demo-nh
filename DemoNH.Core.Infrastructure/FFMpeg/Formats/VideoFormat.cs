using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.FFMpeg.Formats
{
	public class VideoFormat : MediaFormat
	{
		protected VideoFormat(string name, string extension)
			: base(name, extension)
		{
		}

		public static VideoFormat Wmv;
		public static VideoFormat Mp4;
		public static VideoFormat Avi;

		public static List<VideoFormat> Formats { get; private set; }

		static VideoFormat()
		{
			VideoFormat.Wmv = new VideoFormat("WMV", ".wmv");
			VideoFormat.Mp4 = new VideoFormat("MP4", ".mp4");
			VideoFormat.Avi = new VideoFormat("AVI", ".avi");
			VideoFormat.Formats = new List<VideoFormat>(){
				VideoFormat.Wmv,
				VideoFormat.Mp4,
				VideoFormat.Avi,
			};
		}
	}
}