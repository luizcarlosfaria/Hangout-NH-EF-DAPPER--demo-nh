using AopAlliance.Intercept;
using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using DemoNH.Core.Infrastructure.Data;
using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using NH = NHibernate;

namespace DemoNH.Core.Infrastructure.AOP.Data.NHibernate
{
	public class NHContextAroundAdvice : AbstractContextAroundAdvice<NHContextAttribute, NHContext>
	{
		#region Dependence Injection

		[Required]
		private List<SessionFactoryBuilder> SessionFactoryBuilders { get; set; }

		private NH.IInterceptor Interceptor { get; set; }

		private bool ElevateToSystemTransactionsIfRequired { get; set; }

		#endregion Dependence Injection

		protected override string ContextStackListKey
		{
			get { return "DemoNH.Core.Infrastructure.AOP.Data.NHContextAroundAdvice.Contexts"; }
		}

		protected override object Invoke(IMethodInvocation invocation, IEnumerable<NHContextAttribute> contextAttributes)
		{
			object returnValue = null;

			if (this.ElevateToSystemTransactionsIfRequired && contextAttributes.Count(it => it.TransactionMode == NHContextTransactionMode.Transactioned) > 1)
			{
				using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
				{
					returnValue = this.InvokeUsingContext(invocation, new Stack<NHContextAttribute>(contextAttributes));
					scope.Complete();
				}
			}
			else
			{
				returnValue = this.InvokeUsingContext(invocation, new Stack<NHContextAttribute>(contextAttributes));
			}
			return returnValue;
		}

		private object InvokeUsingContext(IMethodInvocation invocation, Stack<NHContextAttribute> contextAttributesStack)
		{
			//Este m�todo � chamado recursivamente, removendo o item do Stack sempre que houver um. At� que n�o haja nenhum. Quando n�o houver nenhum item mais, ele efetivamente
			//manda executar a chamada ao m�todo de destino.
			//Esse controle � necess�rio pois as os "Usings" de Contexto, Sess�o e Transa��o precisam ser encadeados
			object returnValue = null;
			if (contextAttributesStack.Count == 0)
			{
				returnValue = invocation.Proceed();
			}
			else
			{
				NHContextAttribute currentContextAttribute = contextAttributesStack.Pop(); //Obtendo o primeiro primeiro �ltimo RequiredPersistenceContextAttribute da stack, removendo-o.
				using (NHContext currentContext = new NHContext(currentContextAttribute, this.ContextStack, this.Interceptor)) //Criando o contexto
				{
					returnValue = this.InvokeUsingContext(invocation, contextAttributesStack);
					currentContext.Complete();
				}
			}
			return returnValue;
		}

		protected override Func<NHContextAttribute, bool> AttributeQueryFilter
		{
			get
			{
				return (it =>
				{
					it.SessionFactoryBuilder = this.SessionFactoryBuilders.Where(sfb => sfb.ObjectContextKey == it.ContextKey).FirstOrDefault();
					return (it.SessionFactoryBuilder != null);
				});
			}
		}
	}
}