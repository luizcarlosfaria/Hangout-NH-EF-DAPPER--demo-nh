using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ViewEngines.Razor;

namespace DemoNH.Core.Infrastructure.Web.NancyFx
{
	public class RazorConfiguration : IRazorConfiguration
	{
		public bool AutoIncludeModelNamespace { get { return true; } }

		public IEnumerable<string> GetAssemblyNames()
		{
			yield return "DemoNH.Core";
			yield return "DemoNH.Core.Domain";
			yield return "DemoNH.Core.Infrastructure";
		}

		public IEnumerable<string> GetDefaultNamespaces()
		{
			yield return "DemoNH.Core.Services.WebManagement";
			yield return "DemoNH.Core.Services";
			yield return "System";
			yield return "System.Collections.Generic";
			yield return "System.Linq";
			yield return "System.Text";
		}
	}
}
