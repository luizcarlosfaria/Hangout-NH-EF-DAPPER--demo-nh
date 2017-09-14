using Spring.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Threading
{
	public abstract class ReadOnlyThreadStorageProxy<T>
	{
		public string ContextKey { get; set; }

		public virtual T Value
		{
			get
			{
				return (T)Spring.Threading.LogicalThreadContext.GetData(this.ContextKey);
			}
		}

	}
	
}
