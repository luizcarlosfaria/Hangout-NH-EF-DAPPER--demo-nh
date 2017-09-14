using System;

namespace DemoNH.Core.Infrastructure.Business
{
	/// <summary>
	/// Classe base para exce��es de neg�cio.
	/// </summary>
	[Serializable]
	public class BusinessException : Exception
	{
		public BusinessException()
		{
		}

		public BusinessException(string message)
			: base(message)
		{
		}

		public BusinessException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected BusinessException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}