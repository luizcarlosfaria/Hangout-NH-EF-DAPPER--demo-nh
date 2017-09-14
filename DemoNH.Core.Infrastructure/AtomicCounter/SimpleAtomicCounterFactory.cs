using ServiceStack.Redis;

namespace DemoNH.Core.Infrastructure.AtomicCounter
{
	public class SimpleAtomicCounterFactory : IAtomicCounterFactory
	{
		public PooledRedisClientManager PooledRedisClientManager { get; set; }

		public string CounterKeyPrefix { get; set; }

		public IAtomicCounter GetCounter(string counterName, string uniqueIdentifier = null)
		{
			IAtomicCounter counter = new SimpleAtomicCounter(counterName, uniqueIdentifier, this.CounterKeyPrefix, this.PooledRedisClientManager);

			return counter;
		}
	}
}
