using DemoNH.Core.Infrastructure.Log.Model;
using DemoNH.Core.Infrastructure.MessageQueuing;

namespace DemoNH.Core.Infrastructure.Log
{
	public class QueueLogger : AbstractLogger //REFACTORING: Era "MSMQLogger"
	{
		protected IQueueClient QueueClient { get; set; } //REFACTORING: Era "Template"

		public string ExchangeName { get; set; }

		public string RoutingKey { get; set; }

		public string ProjectKey { get; set; }

		protected override void SendLog(LogEntryTransferObject logEntry)
		{
			logEntry.ProjectKey = this.ProjectKey;

			this.QueueClient.Publish(this.ExchangeName, this.RoutingKey, logEntry);
		}
	}
}