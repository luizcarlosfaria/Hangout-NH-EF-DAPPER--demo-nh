using System;
using System.ServiceModel.Channels;

namespace DemoNH.Core.Infrastructure.Services.Host
{
	public class ServiceEndpointConfiguration
	{
		public Type ServiceInterface { get; set; }

		public Binding Binding { get; set; }

		public string Name { get; set; }
	}
}