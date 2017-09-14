namespace DemoNH.Core.Infrastructure.Data.ConnectionStrings
{
	public class ConfigurableConStrConfigDiscovery : IConStrConfigDiscovery
	{
		public string ConnectionString { get; set; }

		public string Name { get; set; }

		public string ProviderName { get; set; }

		public System.Configuration.ConnectionStringSettings GetConnectionString()
		{
			return new System.Configuration.ConnectionStringSettings(this.Name, this.ConnectionString, this.ProviderName);
		}
	}
}