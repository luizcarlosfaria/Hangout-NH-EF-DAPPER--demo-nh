using DemoNH.Core.Infrastructure.Serialization.Interfaces;
using System;

namespace DemoNH.Core.Infrastructure.Serialization
{
	public class JsonSerializer : ISerializer
	{
		public string Serialize(object entity)
		{
			return this.Serialize(entity);
		}

		public string Serialize<T>(T entity)
		{
			return JsonHelper.Serialize(entity);
		}

		public object Deserialize(string entitySerialized, Type type)
		{
			return JsonHelper.Deserialize(entitySerialized, type);
		}

		public T Deserialize<T>(string entitySerialized)
		{
			return JsonHelper.Deserialize<T>(entitySerialized);
		}
	}
}