﻿using System;

namespace DemoNH.Core.Infrastructure.MessageQueuing
{
	[Serializable]
	public class DeserializationException : RejectionException
	{
		public DeserializationException()
			: base()
		{
		}

		public DeserializationException(string message)
			: base(message)
		{
		}

		public DeserializationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected DeserializationException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }

		public string SerializedDataString { get; set; }

		public byte[] SerializedDataBinary { get; set; }
	}
}