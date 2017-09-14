using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Web.NancyFx.Bootstrapper
{
	public interface IConventionConfigurer
	{
		void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions);
	}
}
