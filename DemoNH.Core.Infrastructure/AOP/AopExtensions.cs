using AopAlliance.Intercept;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.AOP
{
	internal static class AopExtensions
	{
		public static IEnumerable<T> GetAttibutes<T>(this IMethodInvocation invocation, Func<T, bool> predicate = null)
		where T : Attribute
		{
			if (predicate == null)
				predicate = (it => true);

			//Recupera os atributos do método
			IEnumerable<T> returnValue = invocation.Method.GetCustomAttributes(typeof(T), true)
			.Cast<T>()
			.Where(predicate);
			return returnValue;
		}
	}
}