namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public interface IMessageRejectionHandler
	{
		void OnRejection(RejectionException exception);
	}
}