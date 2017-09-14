namespace DemoNH.Core.Infrastructure.Cache.Interfaces
{
	public interface ICacheProviderPool
	{
		ICacheProvider GetClient();
	}
}