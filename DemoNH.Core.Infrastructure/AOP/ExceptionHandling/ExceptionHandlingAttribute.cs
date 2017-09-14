using System;

namespace DemoNH.Core.Infrastructure.AOP.ExceptionHandling
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ExceptionHandlingAttribute : Attribute
	{
		public ExceptionHandlingStrategy Strategy { get; set; }

		public ExceptionHandlingAttribute(ExceptionHandlingStrategy strategy)
		{
			this.Strategy = strategy;
		}
	}
}