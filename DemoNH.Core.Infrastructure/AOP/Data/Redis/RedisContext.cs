using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using DemoNH.Core.Infrastructure.Cache.Redis;
using ServiceStack.Redis;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.AOP.Data.Redis
{
	public class RedisContext : AbstractContext<RedisContextAttribute>
	{
		public RedisContext(RedisContextAttribute contextAttribute, Stack<AbstractContext<RedisContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)

		{ }

		protected override void Initialize()
		{
			BasicRedisClientManager manager = new BasicRedisClientManager(this.ContextAttribute.RedisConnectionString.ConnectionString);
			var nativeClient = manager.GetClient();
			this.Client = new RedisClientForSpring(nativeClient, this.ContextAttribute.RedisConnectionString.IsolationKey);
		}

		public RedisClientForSpring Client { get; private set; }

		protected override void DisposeContext()
		{
			base.DisposeContext();
		}

		protected override void DisposeFields()
		{
			this.Client.Dispose();
			this.Client = null;
			base.DisposeFields();
		}
	}
}