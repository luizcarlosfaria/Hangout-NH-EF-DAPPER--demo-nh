using Spring.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Threading
{
	public abstract class ReadWriteThreadStorageProxy<T>
	{
		public string ContextKey { get; set; }

		public void FreeNamedDataSlot()
		{

			Spring.Threading.LogicalThreadContext.FreeNamedDataSlot(this.ContextKey);
		}

		public T Value
		{
			get
			{
				return (T)Spring.Threading.LogicalThreadContext.GetData(this.ContextKey);
			}
			set
			{
				Spring.Threading.LogicalThreadContext.SetData(this.ContextKey, value);
			}
		}

	}
}
