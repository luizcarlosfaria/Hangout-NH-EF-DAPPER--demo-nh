namespace DemoNH.Core.Infrastructure.Workflow.QueuedWorkFlow
{
	public enum ExceptionStrategy
	{
		SendToErrorQueue,
		SendToNextStepQueue,
		Requeue
	}
}