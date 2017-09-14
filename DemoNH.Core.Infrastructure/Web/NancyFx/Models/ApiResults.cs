using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Web.NancyFx.Models
{
	public enum ApiResultStatus
	{
		Success = 1,
		SuccessWithInfo = 2,
		Warning  = -1,
		Error = -2
	}
	
	public class ApiResultEnvelope
	{
		public string Message { get; set; }

		public string StackTrace { get; set; }

		public string[] ExceptionTypes { get; set; }
		
		public ApiResultStatus ResultStatus { get; set; }
	}

	public class ApiResultEnvelope<T> : ApiResultEnvelope
	{
		public T Result { get; set; }
	}

	public class ApiPagedResultEnvelope<T> : ApiResultEnvelope<IList<T>>
	{
		public long TotalCount { get; set; }
		public long PageNumber { get; set; }
		public long PageSize { get; set; }
	}
}
