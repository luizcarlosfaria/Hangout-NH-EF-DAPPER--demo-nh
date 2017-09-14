using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, params IEnumerable<TSource>[] lists)
		{
			if (first == null)
				throw new ArgumentNullException("first");

			IEnumerable<TSource> returnValue = first;
			foreach (IEnumerable<TSource> currentList in lists)
			{
				if (currentList == null)
					throw new ArgumentNullException("lists");

				returnValue = Enumerable.Union(returnValue, currentList);
			}
			return returnValue;
		}
	}
}