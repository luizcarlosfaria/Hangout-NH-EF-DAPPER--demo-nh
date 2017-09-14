using ServiceStack.Redis;

namespace DemoNH.Core.Infrastructure.AtomicCounter
{
	public class SimpleAtomicCounter : IAtomicCounter
	{
		public PooledRedisClientManager PooledRedisClientManager { get; set; }

		public string CounterKeyPrefix { get; set; }

		public string CounterName { get; set; }

		public string UniqueIdentifier { get; set; }

		private string CounterKey { get { return CounterKeyPrefix + ":" + CounterName + ":" + UniqueIdentifier; } }

		private string CounterTargetKey { get { return CounterKey + ".Target"; } }

		public SimpleAtomicCounter()
		{
		}

		public SimpleAtomicCounter(string counterName, string uniqueIdentifier, string counterKeyPrefix, PooledRedisClientManager pooledRedisClientManager)
		{
			this.CounterName = counterName;
			this.UniqueIdentifier = uniqueIdentifier;
			this.CounterKeyPrefix = counterKeyPrefix;
			this.PooledRedisClientManager = pooledRedisClientManager;
		}

		public long Increment()
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				return redisClient.IncrementValue(this.CounterKey);
			}
		}

		public long Decrement()
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				return redisClient.DecrementValue(this.CounterKey);
			}
		}

		public void SetTargetCount(long targetCount)
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				redisClient.Set(this.CounterTargetKey, targetCount);
			}
		}

		public long GetTargetCount()
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				return redisClient.Get<long>(this.CounterTargetKey);
			}
		}

		public bool IsFinished()
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				using (redisClient.AcquireLock(this.CounterKey + ".lock"))
				{
					using (redisClient.AcquireLock(this.CounterTargetKey + ".lock"))
					{
						var currentValue = redisClient.Get<long>(this.CounterKey);

						var targetValue = redisClient.Get<long>(this.CounterTargetKey);

						return currentValue == targetValue;
					}
				}
			}
		}

		public long GetCurrentCount()
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				var currentValue = redisClient.Get<long>(this.CounterKey);

				return currentValue;
			}
		}

		public bool ClearIfFinished()
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				using (redisClient.AcquireLock(this.CounterKey + ".lock"))
				{
					using (redisClient.AcquireLock(this.CounterTargetKey + ".lock"))
					{
						var currentValue = redisClient.Get<long>(this.CounterKey);

						var targetValue = redisClient.Get<long>(this.CounterTargetKey);

						if (currentValue == targetValue)
						{
							redisClient.Remove(this.CounterTargetKey);

							redisClient.Remove(this.CounterKey);

							return true;
						}

						return false;
					}
				}
			}
		}

		public void Clear()
		{
			using (IRedisClient redisClient = this.PooledRedisClientManager.GetClient())
			{
				redisClient.Remove(this.CounterTargetKey);

				redisClient.Remove(this.CounterKey);
			}
		}
	}
}
