using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;

namespace DemoNH.Core.Infrastructure.Helpers
{
	public class SerializationHelper
	{
		public static string Serialize(object content)
		{
			return JsonConvert.SerializeObject(content, Formatting.Indented, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
			});
		}

		public static object Deserialize(string content)
		{
			return JsonConvert.DeserializeObject(content, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.All,
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
			});
		}
	}
}