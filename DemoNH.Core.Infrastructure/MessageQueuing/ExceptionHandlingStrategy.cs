namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public enum ExceptionHandlingStrategy
	{
		Retry, Requeue, Discard
	}
}
