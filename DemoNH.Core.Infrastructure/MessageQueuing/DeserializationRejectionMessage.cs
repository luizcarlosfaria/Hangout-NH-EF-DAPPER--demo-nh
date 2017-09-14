using System;

namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	public class DeserializationRejectionMessage
	{
		public string QueueName { get; set; }

		public DateTime Date { get; set; }

		public string SerializedException { get; set; }

		public string SerializedDataString { get; set; }

		public byte[] SerializedDataBinary { get; set; }
	}
}