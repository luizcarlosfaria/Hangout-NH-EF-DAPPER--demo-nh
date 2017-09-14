using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using DemoNH.Core.Infrastructure.Data.ConnectionStrings;
using System;

namespace DemoNH.Core.Infrastructure.AOP.Data.Redis
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class RedisContextAttribute : AbstractContextAttribute
	{
		public RedisContextAttribute(string contextKey)
		{
			this.ContextKey = contextKey;
		}

		internal RedisConnectionString RedisConnectionString { get; set; }
	}
}