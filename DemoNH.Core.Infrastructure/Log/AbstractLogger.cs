using DemoNH.Core.Infrastructure.Extensions;
using DemoNH.Core.Infrastructure.Log.Model;
using System;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Log
{
	public abstract class AbstractLogger : DemoNH.Core.Infrastructure.Log.ILogger
	{
		protected Dictionary<string, string> AdditionalMetadata { get; set; }

		public void Debug(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Debug, tags);
		}

		public void Trace(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Trace, tags);
		}

		public void Warn(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Warn, tags);
		}

		public void Error(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Error, tags);
		}

		public void Fatal(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Fatal, tags);
		}

		public void Audit(string context, string content, params string[] tags)
		{
			this.Log(context, content, LogLevel.Audit, tags);
		}

		public void Log(string context, string content, LogLevel logLevel, params string[] tags)
		{
			this.Log(context, content, logLevel, tags.ToDictionary());
		}

		public void Log(string context, string content, LogLevel logLevel, Dictionary<string, string> tags)
		{
			LogEntryTransferObject logEntry = new LogEntryTransferObject()
			{
				LogEntryID = 0,
				Context = context,
				Content = content,
				Date = DateTime.Now,
				LogLevel = logLevel,
				Tags = new Dictionary<string, string>(tags)
			};
			if (this.AdditionalMetadata != null)
			{
				foreach (KeyValuePair<string,string> additionalMetadataItem in this.AdditionalMetadata)
				{
					string key = string.Concat("Meta:", additionalMetadataItem.Key);
					logEntry.Tags.AddOrUpdate(key, additionalMetadataItem.Value);
				}
			}
			this.SendLog(logEntry);
		}

		protected abstract void SendLog(LogEntryTransferObject logEntry);
	}
}