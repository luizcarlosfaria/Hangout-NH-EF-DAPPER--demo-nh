namespace DemoNH.Core.Infrastructure.AtomicCounter
{
	public class NamedAtomicCounterFactory : SimpleAtomicCounterFactory, INamedAtomicCounterFactory
	{
		public string CounterName { get; set; }

		public IAtomicCounter GetCounter(string uniqueIdentifier)
		{
			return base.GetCounter(this.CounterName, uniqueIdentifier);
		}
	}
}
