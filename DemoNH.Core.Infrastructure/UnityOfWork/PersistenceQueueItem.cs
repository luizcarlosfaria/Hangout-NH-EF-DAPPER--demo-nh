namespace DemoNH.Core.Infrastructure.UnityOfWork
{
	public class PersistenceQueueItem<T> where T : DemoNH.Core.Infrastructure.Business.Entity
	{
		public T ItemToPersist { get; set; }

		public PersistenceAction Action { get; set; }

		public PersistenceQueueItem(T itemToPersist, PersistenceAction action)
		{
			this.ItemToPersist = itemToPersist;
			this.Action = action;
		}
	}
}