using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Spring.Objects.Factory.Attributes;

namespace DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement
{
	/// <summary>
	/// Realiza as operações de validação no 
	/// </summary>
	public partial class RabbitMQManagementClient : IRabbitMQManagementClient
	{
		[Required]
		public IRestClient RestClient { get; set; }

		public RabbitMQManagementClient() { }

		public List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetQueues.Model.Queue> GetQueues(string virtualHost = null)
		{
			if (string.IsNullOrWhiteSpace(virtualHost) || virtualHost == "/")
				virtualHost = null;

			var request = new RestSharp.RestRequest(virtualHost == null ? "queues" : "queues/" + virtualHost + "/");
			IRestResponse<List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetQueues.Model.Queue>> response = this.RestClient.Execute<List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetQueues.Model.Queue>>(request);
			var result = response.Data;
			return result;
		}

		public List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetVirtualHosts.Model.VirtualHost> GetVirtualHosts()
		{
			var request = new RestSharp.RestRequest("vhosts");
			IRestResponse<List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetVirtualHosts.Model.VirtualHost>> response = this.RestClient.Execute<List<DemoNH.Core.Infrastructure.MessageQueuing.RabbitMQManagement.GetVirtualHosts.Model.VirtualHost>>(request);
			var result = response.Data;
			return result;
		}

	}
}
