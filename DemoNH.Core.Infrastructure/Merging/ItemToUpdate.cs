namespace DemoNH.Core.Infrastructure.Merging
{
	public class ItemToUpdate<T>
	{
		public T Original { get; set; }

		public T Modified { get; set; }
	}
}