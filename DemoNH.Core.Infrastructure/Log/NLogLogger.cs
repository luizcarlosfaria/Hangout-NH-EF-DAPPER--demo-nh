using DemoNH.Core.Infrastructure.Log.Model;
using Newtonsoft.Json;

namespace DemoNH.Core.Infrastructure.Log
{
	public class NLogLogger : AbstractLogger
	{
		private NLog.Logger logger;

		public NLogLogger()
		{
			this.logger = NLog.LogManager.GetLogger("DemoNH.Core.Infrastructure.Log.NLogLogger");
		}

		protected override void SendLog(LogEntryTransferObject logEntry)
		{
			switch (logEntry.LogLevel)
			{
				case LogLevel.Trace:
					this.logger.Trace(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
					break;

				case LogLevel.Warn:
					this.logger.Warn(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
					break;

				case LogLevel.Error:
					this.logger.Error(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
					break;

				case LogLevel.Fatal:
					this.logger.Fatal(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
					break;

				case LogLevel.Audit:
					this.logger.Info(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
					break;

				default:
					this.logger.Debug(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
					break;
			}
		}
	}
}