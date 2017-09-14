using System;

namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public interface IQueueConsumerWorker : IDisposable
	{
		bool ModelIsClosed { get; }

		void DoConsume();

		void Ack(ulong deliveryTag);

		void Nack(ulong deliveryTag, bool requeue = false);
	}
}