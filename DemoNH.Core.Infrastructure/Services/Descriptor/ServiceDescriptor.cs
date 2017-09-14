using DemoNH.Core.Infrastructure.Security.Authentication;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Services.Descriptor
{
	public class ServiceDescriptor
	{
		public string Name { get; set; }

		public string FriendlyName { get; set; }

		public string Description { get; set; }

		//public string InstanceName { get; set; }
		public StartMode StartMode { get; set; }

		public AccountType IdentityType { get; set; }

		public Credential CustomIdentityCredential { get; set; }

		public List<string> Dependences { get; set; }
	}
}