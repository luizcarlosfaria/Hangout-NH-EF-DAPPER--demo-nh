using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DemoNH.Core.Infrastructure.Extensions
{
	/// <summary>
	/// Classe contendo extension methods para objetos Expression.
	/// </summary>
	public static partial class OragonExtensions
	{
		/// <summary>
		/// Concatena duas expressões lambda que retornam bool.
		/// </summary>
		/// <returns>Retorna uma expressão lambda com o retorno sendo o AND lógico das duas expressões.</returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
		{
			var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>
				(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
		}
	}
}