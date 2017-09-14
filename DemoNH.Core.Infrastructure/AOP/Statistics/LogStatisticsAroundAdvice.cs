using System;
using System.Collections.Generic;
using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoNH.Core.Infrastructure.Log;
using System.Diagnostics;
using DemoNH.Core.Infrastructure.AOP.ExceptionHandling;

namespace DemoNH.Core.Infrastructure.AOP.Statistics
{
	public class LogStatisticsAroundAdvice : AbstractContextAroundAdvice<LogStatisticsAttribute, LogStatisticsContext>
	{
		public ILogger Logger { get; set; }

		protected override string ContextStackListKey
		{
			get { return "DemoNH.Core.Infrastructure.AOP.Statistics.LogStatisticsAroundAdvice.Contexts"; }
		}

		protected override Func<LogStatisticsAttribute, bool> AttributeQueryFilter
		{
			get { return it => true; }
		}

		protected override object Invoke(AopAlliance.Intercept.IMethodInvocation invocation, IEnumerable<LogStatisticsAttribute> contextAttributes)
		{
			object returnValue = null;
			using (var logContext = new LogContext(enlist: true))
			{

				var stopwatch = new Stopwatch();
				stopwatch.Start();
				try
				{
					returnValue = invocation.Proceed();
				}
				finally
				{
					stopwatch.Stop();

					Type targetType = invocation.TargetType;
					string targetTypeFullName = string.Concat(targetType.Namespace, ".", targetType.Name);
					string targetMethod = string.Concat(targetTypeFullName, ".", invocation.Method.Name);

					var timeSpent = stopwatch.Elapsed;
					logContext.SetValue("Type", targetTypeFullName);
					logContext.SetValue("Method", targetMethod);

					string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
						timeSpent.Hours, timeSpent.Minutes, timeSpent.Seconds,
						timeSpent.Milliseconds / 10);

					logContext.SetValue("ElapsedTime", elapsedTime);
					Logger.Log(targetTypeFullName, invocation.Method + " executado", LogLevel.Audit, logContext.GetDictionary());
				}
			}
			return returnValue;
		}
	}
}
