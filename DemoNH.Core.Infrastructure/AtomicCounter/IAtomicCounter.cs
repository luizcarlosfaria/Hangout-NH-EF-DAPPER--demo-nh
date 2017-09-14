using ServiceStack.Redis;

namespace DemoNH.Core.Infrastructure.AtomicCounter
{
	public interface IAtomicCounter
	{
		PooledRedisClientManager PooledRedisClientManager { get; set; }
		string CounterKeyPrefix { get; set; }
		string CounterName { get; set; }
		string UniqueIdentifier { get; set; }
		long Increment();
		long Decrement();
		void SetTargetCount(long targetCount);
		long GetTargetCount();
		bool IsFinished();
		long GetCurrentCount();
		bool ClearIfFinished();
		void Clear();
	}
}
