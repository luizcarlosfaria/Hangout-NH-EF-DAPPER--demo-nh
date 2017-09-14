using System;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Services
{
	public class MessageEnvelope
	{
		public Dictionary<string, object> Arguments { get; set; }

		public object ReturnValue { get; set; }

		public Exception Exception { get; set; }
	}
}