using System;

namespace DemoNH.Core.Infrastructure.AOP.Data.Abstractions
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public abstract class AbstractContextAttribute : Attribute
	{
		public string ContextKey { get; set; }
	}
}