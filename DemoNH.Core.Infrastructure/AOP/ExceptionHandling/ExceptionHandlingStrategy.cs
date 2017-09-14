using System;

namespace DemoNH.Core.Infrastructure.AOP.ExceptionHandling
{
	[Flags]
	public enum ExceptionHandlingStrategy
	{
		BreakOnException,
		ContinueRunning
	}
}