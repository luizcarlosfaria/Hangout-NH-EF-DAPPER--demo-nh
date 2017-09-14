using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Merging
{
	public class MergeResult<T>
	{
		public List<T> ItemsToInsert { get; set; }

		public List<ItemToUpdate<T>> ItemsToUpdate { get; set; }

		public List<T> ItemsToDelete { get; set; }

		public bool AreEquals
		{
			get { return this.ItemsToInsert.Count == 0 && this.ItemsToUpdate.Count == 0 && this.ItemsToDelete.Count == 0; }
		}
	}
}