using DemoNH.Core.Infrastructure.Cache.Interfaces;
using ServiceStack.Redis;

namespace DemoNH.Core.Infrastructure.Cache
{
	public class CacheProviderPool : ICacheProviderPool
	{
		private readonly PooledRedisClientManager _redisClientPool;

		public CacheProviderPool(string host)
		{
			_redisClientPool = new PooledRedisClientManager(100, 500, host);
		}

		public ICacheProvider GetClient()
		{
			return new CacheProvider(_redisClientPool.GetClient());
		}
	}
}