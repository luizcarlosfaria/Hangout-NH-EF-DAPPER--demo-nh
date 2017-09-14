using System;

namespace DemoNH.Core.Infrastructure.Serialization.Interfaces
{
	public interface ISerializer
	{
		string Serialize(object entity);

		string Serialize<T>(T entity);

		object Deserialize(string entitySerialized, Type type);

		T Deserialize<T>(string entitySerialized);
	}
}