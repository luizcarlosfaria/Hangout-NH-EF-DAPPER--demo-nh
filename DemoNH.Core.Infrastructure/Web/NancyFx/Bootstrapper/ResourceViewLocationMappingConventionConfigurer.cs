using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy.ViewEngines;

namespace DemoNH.Core.Infrastructure.Web.NancyFx.Bootstrapper
{
	public class ResourceViewLocationMappingConventionConfigurer : IConventionConfigurer
	{
		public Assembly Assembly { get; set; }
		public string Path { get; set; }

		public void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
		{
			ResourceViewLocationProvider.RootNamespaces.Add(this.Assembly, this.Path);
		}
	}
}
