using System.Configuration;

namespace DemoNH.Core.Infrastructure.Data.ConnectionStrings
{
	public interface IConStrConfigDiscovery
	{
		ConnectionStringSettings GetConnectionString();
	}
}