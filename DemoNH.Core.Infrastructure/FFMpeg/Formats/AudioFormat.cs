using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.FFMpeg.Formats
{
	public class AudioFormat : MediaFormat
	{
		protected AudioFormat(string name, string extension)
			: base(name, extension)
		{
		}

		public static AudioFormat Mp3 = new AudioFormat("MP3", ".mp3");
		public static AudioFormat Wav = new AudioFormat("WAV", ".wav");
		public static AudioFormat Wma = new AudioFormat("WMA", ".wma");
		public static AudioFormat Flac = new AudioFormat("Flac", ".flac");

		public static List<AudioFormat> Formats { get; private set; }

		static AudioFormat()
		{
			AudioFormat.Formats = new List<AudioFormat>(){
				AudioFormat.Mp3,
				AudioFormat.Wav,
				AudioFormat.Wma,
				AudioFormat.Flac,
			};
		}
	}
}