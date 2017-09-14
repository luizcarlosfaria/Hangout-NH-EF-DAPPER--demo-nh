using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoNH.Core.Infrastructure.AOP.Statistics
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class LogStatisticsAttribute : AbstractContextAttribute
	{
		
	}
}
