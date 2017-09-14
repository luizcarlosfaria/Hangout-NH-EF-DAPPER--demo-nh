using System.Configuration;

namespace DemoNH.Core.Infrastructure.Data.ConnectionStrings
{
	public class DefaultConStrConfigDiscovery : IConStrConfigDiscovery
	{
		private string ConnectionStringKey { get; set; }

		public System.Configuration.ConnectionStringSettings GetConnectionString()
		{
			System.Configuration.ConnectionStringSettings returnValue = ConfigurationManager.ConnectionStrings[this.ConnectionStringKey];

			if (returnValue == null)
				throw new ConfigurationErrorsException(string.Format("Não foi possível identificar a ConnectionString com a chave '{0}'", this.ConnectionStringKey));

			return returnValue;
		}
	}
}