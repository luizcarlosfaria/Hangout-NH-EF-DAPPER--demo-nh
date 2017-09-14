namespace DemoNH.Core.Infrastructure.AtomicCounter
{
	public interface INamedAtomicCounterFactory
	{
		string CounterName { get; set; }

		IAtomicCounter GetCounter(string uniqueIdentifier);
	}
}
