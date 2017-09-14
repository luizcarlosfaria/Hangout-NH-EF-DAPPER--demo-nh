using Spring.Objects.Factory.Attributes;
using System;

namespace DemoNH.Core.Infrastructure.AOP.Data.Abstractions
{
	public abstract class AbstractDataProcess<ContextType, AttributeType>
		where ContextType : AbstractContext<AttributeType>
		where AttributeType : AbstractContextAttribute
	{
		[Required]
		protected virtual string ObjectContextKey { get; set; }

		protected virtual ContextType ObjectContext
		{
			get
			{
				ContextType returnValue = Spring.Threading.LogicalThreadContext.GetData(this.ObjectContextKey) as ContextType;
				if (returnValue == null)
					throw new InvalidOperationException("Não foi possível identificar o contexto para executar o data process");
				return returnValue;
			}
		}
	}
}