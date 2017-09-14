namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public interface IMessageFeedbackSender
	{
		ulong DeliveryTag { get; }

		bool MessageAcknoledged { get; }

		void Ack();

		void Nack(bool requeue);
	}
}