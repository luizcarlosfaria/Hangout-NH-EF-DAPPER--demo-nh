using DemoNH.Core.Infrastructure.Extensions;
using DemoNH.Core.Infrastructure.FFMpeg.Files;
using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoNH.Core.Infrastructure.FFMpeg
{
	public class FFMpegService
	{
		[Required]
		public string FFMpegPath { get; set; }

		[Required]
		public TimeSpan TimeOut { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public List<string> InitialParameters { get; set; }

		[Required]
		public List<string> MiddleParameters { get; set; }

		[Required]
		public WaitMode WaitMode { get; set; }

		public void Encode(MediaFile originalFileName, MediaFile targetFileName)
		{
			if (originalFileName.Exists == false)
				throw new System.IO.FileNotFoundException(originalFileName.FileName);

			List<string> arguments = new List<string>();
			this.FillArguentList(originalFileName, targetFileName, arguments);

			string inLineArguments = this.BuildArguments(arguments);
			System.Diagnostics.Process process = this.BuildFFMPegProcess(inLineArguments);

			if (this.WaitMode == FFMpeg.WaitMode.Sync)
			{
				this.RunSync(process);
			}
			else if (this.WaitMode == FFMpeg.WaitMode.Async)
			{
				this.RunAsync(process);
			}
		}

		protected virtual void FillArguentList(MediaFile originalFileName, MediaFile targetFileName, List<string> arguments)
		{
			this.AddParameters(arguments, this.InitialParameters);

			arguments.Add(originalFileName.FileName.Quotation());

			this.AddParameters(arguments, this.MiddleParameters);

			arguments.Add(targetFileName.FileName.Quotation());
		}

		protected virtual void AddParameters(List<string> arguments, List<string> defaultParameters)
		{
			if (defaultParameters != null && defaultParameters.Count > 0)
				arguments.AddRange(defaultParameters);
		}

		protected virtual void RunAsync(System.Diagnostics.Process process)
		{
			bool processStarted = process.Start();
		}

		protected virtual void RunSync(System.Diagnostics.Process process)
		{
			StringBuilder output = new StringBuilder();
			StringBuilder error = new StringBuilder();
			using (System.Threading.AutoResetEvent outputWaitHandle = new System.Threading.AutoResetEvent(false))
			using (System.Threading.AutoResetEvent errorWaitHandle = new System.Threading.AutoResetEvent(false))
			{
				process.OutputDataReceived += (sender, e) =>
				{
					if (e.Data == null)
					{
						outputWaitHandle.Set();
					}
					else
					{
						output.AppendLine(e.Data);
					}
				};
				process.ErrorDataReceived += (sender, e) =>
				{
					if (e.Data == null)
					{
						errorWaitHandle.Set();
					}
					else
					{
						error.AppendLine(e.Data);
					}
				};
				bool processStarted = process.Start();
				System.Diagnostics.Debug.WriteLine("Process Start: " + processStarted.ToString());
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();

				int timeoutInMilliseconds = Convert.ToInt32(this.TimeOut.TotalMilliseconds);
				if (
					process.WaitForExit(timeoutInMilliseconds) &&
					outputWaitHandle.WaitOne(timeoutInMilliseconds) &&
					errorWaitHandle.WaitOne(timeoutInMilliseconds)
				)
				{
					// Process completed. Check process.ExitCode here.
					System.Diagnostics.Debug.WriteLine("FFMPEG Output Begin ============");
					System.Diagnostics.Debug.WriteLine(output.ToString());
					System.Diagnostics.Debug.WriteLine("FFMPEG Output End   ============");
					System.Diagnostics.Debug.WriteLine("FFMPEG Error Begin ============");
					System.Diagnostics.Debug.WriteLine(error.ToString());
					System.Diagnostics.Debug.WriteLine("FFMPEG Error End   ============");
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("FFMPEG Output Begin ============");
					System.Diagnostics.Debug.WriteLine(output.ToString());
					System.Diagnostics.Debug.WriteLine("FFMPEG Output End   ============");
					System.Diagnostics.Debug.WriteLine("FFMPEG Error Begin ============");
					System.Diagnostics.Debug.WriteLine(error.ToString());
					System.Diagnostics.Debug.WriteLine("FFMPEG Error End   ============");
					throw new TimeoutException("A execução do process excedeu o limite de tempo definido para o processamento. O processamento não foi concluído.");
				}
			}
		}

		protected virtual string BuildArguments(List<string> arguments)
		{
			string inLineArguments = string.Join(" ", arguments.ToArray());
			return inLineArguments;
		}

		protected virtual System.Diagnostics.Process BuildFFMPegProcess(string inLineArguments)
		{
			System.Diagnostics.Process process = new System.Diagnostics.Process()
			{
				StartInfo = new System.Diagnostics.ProcessStartInfo(this.FFMpegPath, inLineArguments)
				{
					CreateNoWindow = false,
					UseShellExecute = false,
					RedirectStandardError = true,
					RedirectStandardOutput = true,
					WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
				}
			};
			return process;
		}
	}
}