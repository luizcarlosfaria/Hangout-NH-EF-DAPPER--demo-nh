﻿using DemoNH.Core.Infrastructure.MessageQueuing;

namespace DemoNH.Core.Infrastructure.Workflow.QueuedWorkFlow
{
	public class QueuedTransition : DemoNH.Core.Infrastructure.Workflow.Facility.StringTransition
	{
		public string LogicalQueueName { get; set; }

		public string ExchangeName { get; set; }

		public object Service { get; set; }

		public string ServiceMethod { get; set; }

		public ExceptionStrategy ErrorFlowStrategy { get; set; }

		public IQueueConsumer QueueConsumer { get; set; }

		public ConsumerCountManager ConsumerCountManager { get; set; }

		//TODO: Aidionar o Listener Aqui;

		public string BuildRoutingKey()
		{
			var origin = this.Origin != null ? this.Origin : string.Empty;
			var destination = this.Destination != null ? this.Destination : string.Empty;

			string returnValue = string.Format("{0}->{1}", origin, destination);
			return returnValue;
		}

		public string BuildFailureRoutingKey()
		{
			var origin = this.Origin != null ? this.Origin : string.Empty;
			var destination = this.Destination != null ? this.Destination : string.Empty;

			string returnValue = string.Format("{0}->{1}#Failure#", origin, destination);
			return returnValue;
		}
	}
}