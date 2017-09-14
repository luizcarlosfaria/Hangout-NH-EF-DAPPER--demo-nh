using System;

namespace DemoNH.Core.Infrastructure.Business
{
	/// <summary>
	/// Define uma exception n�o gerenciada tratada pelo mecanismo de exception replacement
	/// </summary>
	[Serializable]
	public class UndefinedException : BusinessException
	{
		public UndefinedException()
		{
		}

		public UndefinedException(string message)
			: base(message)
		{
		}

		public UndefinedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected UndefinedException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}