using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoNH.Core.Exceptions
{
	[Serializable]
	public class DemoNHException : DemoNH.Core.Infrastructure.Business.BusinessException
	{
		public DemoNHException(): base() { }
		public DemoNHException(string message) : base(message) { }
		public DemoNHException(string message, Exception inner) : base(message, inner) { }
		protected DemoNHException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

}
