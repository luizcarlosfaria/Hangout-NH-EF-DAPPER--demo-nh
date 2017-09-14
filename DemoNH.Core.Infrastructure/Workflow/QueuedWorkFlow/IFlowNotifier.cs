using System;
using System.Collections;
using System.ComponentModel;
using System.ServiceModel;

namespace DemoNH.Core.Infrastructure.Workflow.QueuedWorkFlow
{
	[ServiceContract]
	public interface IFlowNotifier
	{
		[OperationContract]
		void OnStart(object messageObject, CancelEventArgs cancellation);

		[OperationContract]
		void OnCancel(object messageObject);

		[OperationContract]
		void OnProcessingSuccess(object messageObject, IEnumerable responses);

		[OperationContract]
		void OnSuccess(object messageObject, IEnumerable responses);

		[OperationContract]
		void OnFailure(object request, Exception exception, ExceptionStrategy defaultExceptionStrategy, ExceptionStrategy usedExceptionStrategy);
	}
}