using Spring.Objects.Factory;
using Spring.ServiceModel;
using System;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace DemoNH.Core.Infrastructure.Services.Host
{
	public class ServiceHostFactoryObject : IFactoryObject, IInitializingObject, IObjectFactoryAware
	{
		public List<IServiceBehavior> Behaviors { get; set; }

		public List<ServiceEndpointConfiguration> ServiceEndpoints { get; set; }

		public ServiceHostFactoryObject()
		{
			this.UseServiceProxyTypeCache = true;
		}

		protected OragonServiceHost OragonServiceHost;

		public string TargetName { get; set; }

		public Uri[] BaseAddresses { get; set; }

		public bool UseServiceProxyTypeCache { get; set; }

		public virtual IObjectFactory ObjectFactory { get; set; }

		public virtual System.Type ObjectType { get { return typeof(OragonServiceHost); } }

		public virtual bool IsSingleton { get { return false; } }

		public virtual object GetObject()
		{
			return this.OragonServiceHost;
		}

		public virtual void AfterPropertiesSet()
		{
			this.ValidateConfiguration();
			this.OragonServiceHost = new OragonServiceHost(this.TargetName, this.ObjectFactory, this.UseServiceProxyTypeCache, this.BaseAddresses);
			this.ConfigureHost();
		}

		protected virtual void ConfigureHost()
		{
			this.AddBehaviors();
			this.AddServiceEndpoints();
		}

		protected void AddBehaviors()
		{
			if (this.Behaviors != null && this.Behaviors.Count > 0)
			{
				foreach (IServiceBehavior currentServiceBehavior in this.Behaviors)
				{
					this.OragonServiceHost.Description.Behaviors.Add(currentServiceBehavior);
				}
			}
		}

		protected void AddServiceEndpoints()
		{
			if (this.ServiceEndpoints != null && this.ServiceEndpoints.Count > 0)
			{
				foreach (ServiceEndpointConfiguration currentServiceEndpoint in this.ServiceEndpoints)
				{
					this.OragonServiceHost.AddServiceEndpoint(currentServiceEndpoint.ServiceInterface, currentServiceEndpoint.Binding, currentServiceEndpoint.Name);
				}
			}
		}

		protected virtual void ValidateConfiguration()
		{
			if (this.TargetName == null)
			{
				throw new System.ArgumentException("The TargetName property is required.");
			}
		}
	}
}