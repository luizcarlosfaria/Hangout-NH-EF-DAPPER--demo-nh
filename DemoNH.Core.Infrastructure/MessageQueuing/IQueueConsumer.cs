using System;

namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public interface IQueueConsumer : IDisposable
	{
		ConsumerCountManager ConsumerCountManager { get; }

		void Start();

		void Stop();

		uint GetMessageCount();

		uint GetConsumerCount();

		IMessageProcessingWorker MessageProcessingWorker { get; set; }
	}
}