using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using DemoNH.Core.Infrastructure.Data.ConnectionStrings;
using System;

namespace DemoNH.Core.Infrastructure.AOP.Data.MongoDB
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class MongoDBContextAttribute : AbstractContextAttribute
	{
		public MongoDBContextAttribute(string contextKey)
		{
			this.ContextKey = contextKey;
		}

		internal MongoDBConnectionString MongoDBConnectionString { get; set; }
	}
}