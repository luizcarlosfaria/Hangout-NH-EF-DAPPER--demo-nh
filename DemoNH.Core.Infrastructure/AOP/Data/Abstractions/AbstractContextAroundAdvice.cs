﻿using AopAlliance.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.AOP.Data.Abstractions
{
	public abstract class AbstractContextAroundAdvice<AttributeType, ContextType> : IMethodInterceptor
		where AttributeType : AbstractContextAttribute
		where ContextType : AbstractContext<AttributeType>
	{
		protected abstract string ContextStackListKey { get; }

		protected Stack<AbstractContext<AttributeType>> ContextStack
		{
			get
			{
				Stack<AbstractContext<AttributeType>> contextStack = Spring.Threading.LogicalThreadContext.GetData(this.ContextStackListKey) as Stack<AbstractContext<AttributeType>>;
				if (contextStack == null)
				{
					contextStack = new Stack<AbstractContext<AttributeType>>();
					Spring.Threading.LogicalThreadContext.SetData(ContextStackListKey, contextStack);
				}
				return contextStack;
			}
		}

		public object Invoke(IMethodInvocation invocation)
		{
			object returnValue = null;
			IEnumerable<AttributeType> contextAttributes = this.GetContextAttributes(invocation);
			if (contextAttributes.Any())
				returnValue = this.Invoke(invocation, contextAttributes);
			else
				returnValue = invocation.Proceed();
			return returnValue;
		}

		protected abstract Func<AttributeType, bool> AttributeQueryFilter { get; }

		protected abstract object Invoke(IMethodInvocation invocation, IEnumerable<AttributeType> contextAttributes);

		/// <summary>
		/// Obtém informações de persistência definidas nos métodos
		/// </summary>
		/// <param name="invocation"></param>
		/// <returns></returns>
		protected IEnumerable<AttributeType> GetContextAttributes(IMethodInvocation invocation)
		{
			//Recupera os atributos do método
			IEnumerable<AttributeType> returnValue = invocation.GetAttibutes<AttributeType>(AttributeQueryFilter);
			return returnValue;
		}
	}
}