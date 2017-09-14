using System.Security.Principal;

namespace DemoNH.Core.Infrastructure.Security
{
	/// <summary>
	/// Interface se propõe a resolver uma entidade de negócio com base no IIdentity informado
	/// </summary>
	public interface IUserResolver
	{
		object Resolve(IIdentity identity);
	}
}