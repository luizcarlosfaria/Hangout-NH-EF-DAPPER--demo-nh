using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Responses;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using Spring.Aop.Framework.DynamicProxy;
using Spring.Context;

namespace DemoNH.Core.Infrastructure.Web.NancyFx.Bootstrapper
{
	public class EmbeddedStaticMappingConventionConfigurer : IConventionConfigurer
	{
		public string Directory { get; set; }

		public Assembly Assembly { get; set; }

		public string EmbeddedResourceNamespacePrefix { get; set; }

		public void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
		{
			nancyConventions.StaticContentsConventions.Add((ctx, rootPath) =>
			{
				Response response = null;
				var path = Path.GetDirectoryName(ctx.Request.Url.Path) ?? string.Empty;
				if (path.StartsWith(this.Directory, StringComparison.OrdinalIgnoreCase))
				{
					response = new EmbeddedFileResponse(this.Assembly, this.EmbeddedResourceNamespacePrefix + path.Replace(@"\", @"."), Path.GetFileName(ctx.Request.Url.Path));
				}
				return response;
			});
		}
	}
}
