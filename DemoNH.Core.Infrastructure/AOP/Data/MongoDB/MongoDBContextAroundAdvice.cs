using AopAlliance.Intercept;
using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using DemoNH.Core.Infrastructure.Data.ConnectionStrings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.AOP.Data.MongoDB
{
	public class MongoDBContextAroundAdvice : AbstractContextAroundAdvice<MongoDBContextAttribute, MongoDBContext>
	{
		public List<MongoDBConnectionString> ConnectionStrings { get; set; }

		protected override string ContextStackListKey
		{
			get { return "DemoNH.Core.Infrastructure.AOP.Data.MongoDBContextAroundAdvice.Contexts"; }
		}

		protected override Func<MongoDBContextAttribute, bool> AttributeQueryFilter
		{
			get
			{
				return (it =>
				{
					it.MongoDBConnectionString = this.ConnectionStrings.Where(localConnecions => localConnecions.Key == it.ContextKey).FirstOrDefault();
					return (it.MongoDBConnectionString != null);
				});
			}
		}

		protected override object Invoke(AopAlliance.Intercept.IMethodInvocation invocation, IEnumerable<MongoDBContextAttribute> contextAttributes)
		{
			object returnValue = null;
			returnValue = this.InvokeUsingContext(invocation, new Stack<MongoDBContextAttribute>(contextAttributes));
			return returnValue;
		}

		private object InvokeUsingContext(IMethodInvocation invocation, Stack<MongoDBContextAttribute> contextAttributesStack)
		{
			//Este método é chamado recursivamente, removendo o item do Stack sempre que houver um. Até que não haja nenhum. Quando não houver nenhum item mais, ele efetivamente
			//manda executar a chamada ao método de destino.
			//Esse controle é necessário pois as os "Usings" de Contexto, Sessão e Transação precisam ser encadeados
			object returnValue = null;
			if (contextAttributesStack.Count == 0)
			{
				returnValue = invocation.Proceed();
			}
			else
			{
				//Obtendo o primeiro primeiro último RequiredPersistenceContextAttribute da stack, removendo-o.
				MongoDBContextAttribute currentContextAttribute = contextAttributesStack.Pop();
				//Criando o contexto
				using (MongoDBContext currentContext = new MongoDBContext(currentContextAttribute, this.ContextStack))
				{
					returnValue = this.InvokeUsingContext(invocation, contextAttributesStack);
				}
			}
			return returnValue;
		}
	}
}