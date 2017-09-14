using System.Security.Principal;

namespace DemoNH.Core.Infrastructure.Security
{
	/// <summary>
	/// Interface se prop�e a resolver uma entidade de neg�cio com base no IIdentity informado
	/// </summary>
	public interface IUserResolver
	{
		object Resolve(IIdentity identity);
	}
}