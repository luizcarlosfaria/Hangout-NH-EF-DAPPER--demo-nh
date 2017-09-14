using System;

namespace DemoNH.Core.Infrastructure.MessageQueuing.Exceptions
{
	public class QueuingRetryException : BaseQueuingException
	{
		public QueuingRetryException()
			: base()
		{
		}

		public QueuingRetryException(string message)
			: base(message)
		{
		}

		public QueuingRetryException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
