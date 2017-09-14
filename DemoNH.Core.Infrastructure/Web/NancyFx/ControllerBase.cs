using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Web.NancyFx
{
	public abstract class ControllerBase : NancyModule
	{
		protected ControllerBase() : base() { }

		protected ControllerBase(string modulePath) : base(modulePath) { }


	}
}
