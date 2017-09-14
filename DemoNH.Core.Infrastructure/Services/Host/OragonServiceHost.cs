using DemoNH.Core.Infrastructure.Services;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory;
using Spring.ServiceModel.Support;
using Spring.Util;
using System;
using System.ServiceModel;

namespace Spring.ServiceModel
{
	public class OragonServiceHost : ServiceHost, IService
	{
		public OragonServiceHost(string serviceName, params Uri[] baseAddresses)
			: this(serviceName, OragonServiceHost.GetApplicationContext(null), baseAddresses)
		{
		}

		public OragonServiceHost(string serviceName, string contextName, params Uri[] baseAddresses)
			: this(serviceName, OragonServiceHost.GetApplicationContext(contextName), baseAddresses)
		{
		}

		public OragonServiceHost(string serviceName, IObjectFactory objectFactory, params Uri[] baseAddresses)
			: this(serviceName, objectFactory, true, baseAddresses)
		{
		}

		public OragonServiceHost(string serviceName, IObjectFactory objectFactory, bool useServiceProxyTypeCache, params Uri[] baseAddresses)
			: base(OragonServiceHost.CreateServiceType(serviceName, objectFactory, useServiceProxyTypeCache), baseAddresses)
		{
		}

		private static IApplicationContext GetApplicationContext(string contextName)
		{
			if (StringUtils.IsNullOrEmpty(contextName))
			{
				return ContextRegistry.GetContext();
			}
			return ContextRegistry.GetContext(contextName);
		}

		private static System.Type CreateServiceType(string serviceName, IObjectFactory objectFactory, bool useServiceProxyTypeCache)
		{
			if (StringUtils.IsNullOrEmpty(serviceName))
			{
				throw new System.ArgumentException("The service name cannot be null or an empty string.", "serviceName");
			}
			if (objectFactory.IsTypeMatch(serviceName, typeof(System.Type)))
			{
				return objectFactory.GetObject(serviceName) as System.Type;
			}
			return new ServiceProxyTypeBuilder(serviceName, objectFactory, useServiceProxyTypeCache).BuildProxyType();
		}

		public string Name
		{
			get { return "OragonServiceHost"; }
		}

		public void Start()
		{
			this.Open();
		}

		public void Stop()
		{
			this.Close();
		}
	}
}