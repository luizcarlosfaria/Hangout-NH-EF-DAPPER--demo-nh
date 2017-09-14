using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoNH.Core.Infrastructure.Services;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;

namespace DemoNH.Core.Infrastructure.Web.NancyFx
{
	public class NancyFxHostService : IService
	{
		public Uri[] Addresses { get; set; }

		public HostConfiguration HostConfiguration { get; set; }

		public INancyBootstrapper Bootstrapper { get; set; }

		#region IService Members

		public string Name { get { return "NancyFxHostService"; } }

		private NancyHost _nancyHost { get; set; }

		public void Start()
		{
			this.HostConfiguration.UnhandledExceptionCallback = this.HttpExceptionHandler;
			this._nancyHost = new NancyHost(this.Bootstrapper, this.HostConfiguration, this.Addresses);
			this._nancyHost.Start();
		}

		public void HttpExceptionHandler(Exception exception)
		{
			Console.WriteLine(exception);
		}

		public void Stop()
		{
			if (this._nancyHost != null)
			{
				this._nancyHost.Dispose();
				this._nancyHost = null;
			}

		}

		#endregion
	}
}
