namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public interface IMessageProcessingWorker
	{
		void OnMessage(object message, IMessageFeedbackSender feedbackSender);
	}
}