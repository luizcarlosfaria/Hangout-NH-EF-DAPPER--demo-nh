namespace DemoNH.Core.Infrastructure.AtomicCounter
{
	public interface IAtomicCounterFactory
	{
		IAtomicCounter GetCounter(string counterName, string uniqueIdentifier = null);
	}
}
