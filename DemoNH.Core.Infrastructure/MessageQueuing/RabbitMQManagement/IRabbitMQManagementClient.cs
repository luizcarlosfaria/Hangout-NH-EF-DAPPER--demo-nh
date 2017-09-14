using System;
namespace DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement
{
	interface IRabbitMQManagementClient
	{
		System.Collections.Generic.List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetQueues.Model.Queue> GetQueues(string virtualHost = null);
		System.Collections.Generic.List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetVirtualHosts.Model.VirtualHost> GetVirtualHosts();
	}
}
