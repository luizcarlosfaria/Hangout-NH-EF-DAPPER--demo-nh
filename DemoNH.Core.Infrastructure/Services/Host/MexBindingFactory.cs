using Spring.Objects.Factory;
using System;

namespace DemoNH.Core.Infrastructure.Services.Host
{
	public enum MexBindingProtocol
	{
		None,
		Http,
		Https,
		NamedPipe,
		Tcp
	}

	public class MexBindingFactory : IFactoryObject
	{
		public MexBindingProtocol Protocol { get; set; }

		public object GetObject()
		{
			System.ServiceModel.Channels.Binding returnValue = null;
			if (this.Protocol == MexBindingProtocol.None)
				returnValue = null;
			else if (this.Protocol == MexBindingProtocol.Http)
				returnValue = System.ServiceModel.Description.MetadataExchangeBindings.CreateMexHttpBinding();
			else if (this.Protocol == MexBindingProtocol.Https)
				System.ServiceModel.Description.MetadataExchangeBindings.CreateMexHttpsBinding();
			else if (this.Protocol == MexBindingProtocol.NamedPipe)
				System.ServiceModel.Description.MetadataExchangeBindings.CreateMexNamedPipeBinding();
			else if (this.Protocol == MexBindingProtocol.Tcp)
				System.ServiceModel.Description.MetadataExchangeBindings.CreateMexTcpBinding();
			else
				throw new InvalidOperationException("Tipo de Protocolo não suportado ou não configurado");
			return returnValue;
		}

		public bool IsSingleton
		{
			get { return false; }
		}

		public Type ObjectType
		{
			get { return typeof(System.ServiceModel.Channels.Binding); }
		}
	}
}