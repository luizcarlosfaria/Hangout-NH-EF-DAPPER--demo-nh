using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.Data
{
	public class PagedList<T>
	{
		public int Total { get; private set; }

		public IEnumerable<T> Data { get; private set; }

		public PagedList(IEnumerable<T> data)
		{
			this.Data = data;
			this.Total = this.Data == null ? 0 : this.Data.Count();
		}
	}
}