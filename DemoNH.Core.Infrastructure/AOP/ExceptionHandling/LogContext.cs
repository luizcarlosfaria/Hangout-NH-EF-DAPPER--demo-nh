using DemoNH.Core.Infrastructure.Extensions;
using System;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.AOP.ExceptionHandling
{
	public class LogContext : IDisposable
	{
		protected Dictionary<string, string> LogTags { get; private set; }

		public LogContext Parent { get; private set; }

		public bool Enlist { get; private set; }

		public LogContext(bool enlist = false)
		{
			this.LogTags = new Dictionary<string, string>();
			this.Enlist = enlist;
			if (this.Enlist)
			{
				this.Parent = Spring.Threading.LogicalThreadContext.GetData("LogContext") as LogContext;
				Spring.Threading.LogicalThreadContext.SetData("LogContext", this);
			}
		}

		public void SetValue(string key, string value)
		{
			this.LogTags.AddOrUpdate(key, value);
		}

		public void Remove(string key)
		{
			this.LogTags.Remove(key);
		}

		public Dictionary<string, string> GetDictionary()
		{
			Dictionary<string, string> returnValue = new Dictionary<string, string>();
			foreach (var item in this.LogTags)
				returnValue.Add(item.Key, item.Value);
			return returnValue;
		}

		public void Dispose()
		{
			if (this.Enlist)
			{
				LogContext itemToSet = (this.Parent != null) ? this.Parent : null;
				Spring.Threading.LogicalThreadContext.SetData("LogContext", itemToSet);
			}
		}

		public static LogContext Current
		{
			get
			{
				LogContext logContext = Spring.Threading.LogicalThreadContext.GetData("LogContext") as LogContext;
				if (logContext == null)
					logContext = new LogContext(false);
				return logContext;
			}
		}
	}
}