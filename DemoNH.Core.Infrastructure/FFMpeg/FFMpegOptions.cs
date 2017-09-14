//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DemoNH.Core.Infrastructure.Extensions;

//namespace DemoNH.Core.Infrastructure.FFMpeg
//{
//	internal static class FFMpegOptions
//	{
//		internal static class GlobalOptions
//		{
//			/*
//			-loglevel loglevel  set libav* logging level
//			-v loglevel         set libav* logging level
//						-report             generate a report
//						-max_alloc bytes    set maximum size of a single allocated block
//						-y                  overwrite output sourceFiles
//						-n                  do not overwrite output sourceFiles
//						-stats              print progress report during encoding
//						-bits_per_raw_sample number  set the number of bits per raw sample
//						-vol volume         change audio volume (256=normal)
//			 */
//			internal static string ImputFile() { return "-i"; }
//			internal static string OverwriteOutputFiles() { return "-y"; }
//			internal static string DoNotOverwriteOutputFiles() { return "-n"; }
//			internal static string GenerateReport() { return "-report"; }
//			internal static string ChangeAudioVolume(int audioValue) { return "-vol {0}".Formatar(audioValue); }
//			internal static string BitsPerRawSample(int bitsPerRaw) { return "-bits_per_raw_sample {0}".Formatar(bitsPerRaw); }
//			internal static string Stats() { return "-stats"; }
//			internal static string MaxAlloc(long maximumBlockSize) { return "-max_alloc {0}".Formatar(maximumBlockSize); }
//		}

//		internal static class VideoOptions
//		{
//			/*
//						-vframes number     set the number of video frames to record
//						-r rate             set frame rate (Hz value, fraction or abbreviation)
//						-s size             set frame size (WxH or abbreviation)
//						-aspect aspect      set aspect ratio (4:3, 16:9 or 1.3333, 1.7777)
//						-bits_per_raw_sample number  set the number of bits per raw sample
//						-vn                 disable video
//						-b bitrate          video bitrate (please use -b:v)
//			-vcodec codec       force video codec ('copy' to copy stream)
//			-timecode hh:mm:ss[:;.]ff  set initial TimeCode value.
//			-pass n             select the pass number (1 to 3)
//			-vf filter_graph    set video filters
//			-dn                 disable data
//			 */

//			internal static string FramesToRecord(long framesToRecord) { return "-vframes {0}".Formatar(framesToRecord); }
//			internal static string Rate(string rate) { return "-r {0}".Formatar(rate); }
//			internal static string Size(string size) { return "-s {0}".Formatar(size); }
//			internal static string AspectRation(AscpectRation aspectRation) { return "-aspect {0}".Formatar(aspectRation.Value); }
//			internal static string DisableVideo() { return "-vn"; }
//			internal static string BitRate(long bitRate) { return "-b:v {0}".Formatar(bitRate); }
//			internal static string DisabledData() { return "-dn"; }
//		}

//		internal static class AudioOptions
//		{
//			/*
//						-aframes number     set the number of audio frames to record
//						-ar rate            set audio sampling rate (in Hz)
//						-an                 disable audio
//						-vol volume         change audio volume (256=normal)
//				-aq quality         set audio quality (codec-specific)
//				-ac channels        set number of audio channels
//				-acodec codec       force audio codec ('copy' to copy stream)
//				-af filter_graph    set audio filters
//			 */
//			internal static string FramesToRecord(long framesToRecord) { return "-aframes {0}".Formatar(framesToRecord); }
//			internal static string Rate(long rate) { return "-ar {0}".Formatar(rate); }
//			internal static string DisabledAudio() { return "-an"; }
//			internal static string ChangeAudioVolume(int audioValue) { return "-vol {0}".Formatar(audioValue); }

//			internal static class Advanced
//			{
//				/*
//				 Advanced Audio options:
//				-atag fourcc/tag    force audio tag/fourcc
//				-sample_fmt format  set sample format
//				-channel_layout layout  set channel layout
//				-guess_layout_max   set the maximum number of channels to try to guess the channel layout
//				-absf audio bitstream_filters  deprecated
//				-apre preset        set the audio options to the indicated preset
//				 */

//			}
//		}
//	}

//}