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
	public class StaticMappingConventionConfigurer : IConventionConfigurer
	{
		public string Directory { get; set; }

		public string Value { get; set; }

		public void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
		{
			nancyConventions.MapStaticContent(
			(file, directory) =>
			{
				directory[this.Directory] = this.Value;
			});
		}
	}
}
