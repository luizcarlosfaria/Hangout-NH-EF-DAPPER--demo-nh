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
	public class ViewLocationConventionsConventionConfigurer : IConventionConfigurer
	{
		public string[] Paths { get; set; }

		public bool AddViewName { get; set; }

		public void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
		{
			nancyConventions.ViewLocationConventions.Add((viewName, model, context) =>
			{
				string[] paths = this.AddViewName? this.Paths.Union(new string[] { viewName }).ToArray() : this.Paths;
				return Path.Combine(paths).Replace(@"\", @"/");
			});
		}
	}
}
