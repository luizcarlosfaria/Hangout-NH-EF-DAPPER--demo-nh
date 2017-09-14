using System;

namespace DemoNH.Core.Infrastructure.Merging
{
	[Serializable]
	public class MergeException : Exception
	{
		public MergeException()
		{
		}

		public MergeException(string message)
			: base(message)
		{
		}

		public MergeException(string message, Exception inner)
			: base(message, inner)
		{
		}

		protected MergeException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}