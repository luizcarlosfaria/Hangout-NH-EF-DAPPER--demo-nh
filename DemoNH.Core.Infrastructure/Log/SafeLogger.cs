using System;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Log
{
	public class SafeLogger : AbstractLogger
	{
		public ILogger PrimaryLogger { get; set; }

		public ILogger SecondaryLogger { get; set; }

		protected override void SendLog(Model.LogEntryTransferObject logEntry)
		{
			List<string> values = new List<string>();
			try
			{
				this.PrimaryLogger.Log(logEntry.Context, logEntry.Content, logEntry.LogLevel, logEntry.Tags);
			}
			catch (Exception ex)
			{
				this.SecondaryLogger.Log("SafeLogger - ", ex.ToString(), LogLevel.Error, logEntry.Tags);
				this.SecondaryLogger.Log(logEntry.Context, logEntry.Content, logEntry.LogLevel, logEntry.Tags);
			}
		}
	}
}