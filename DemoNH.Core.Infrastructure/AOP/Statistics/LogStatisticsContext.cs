using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.AOP.Statistics
{
	public class LogStatisticsContext : AbstractContext<LogStatisticsAttribute>
	{
		public LogStatisticsContext(LogStatisticsAttribute contextAttribute, Stack<AbstractContext<LogStatisticsAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{
		}

		protected override void Initialize()
		{

		}

		protected override void DisposeContext()
		{
			base.DisposeContext();
		}

		protected override void DisposeFields()
		{
			base.DisposeFields();
		}
	}
}
