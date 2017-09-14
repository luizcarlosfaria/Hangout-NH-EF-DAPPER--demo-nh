using DemoNH.Core.Infrastructure.AOP.Data.NHibernate;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DemoNH.Core.Infrastructure.Data.Process
{
	public class QueryDataProcess<T> : NHDataProcess
		where T : DemoNH.Core.Infrastructure.Business.Entity
	{
		public bool UseCacheByDefault { get; set; }

		/// <summary>
		/// Obtem um query a partir de uma expressão
		/// </summary>
		/// <param name="predicate">expressão</param>
		/// <param name="orderByExpression">order by</param>
		/// <param name="cacheable">bool que define se pode ou não usar cache</param>
		/// <returns></returns>
		protected virtual IQueryOver<T, T> GetQuery(Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> orderByExpression = null, bool? cacheable = null)
		{
			IQueryOver<T, T> queryOver = this.ObjectContext.Session.QueryOver<T>();

			if (
					(cacheable != null && cacheable.Value)
					||
					(cacheable == null && this.UseCacheByDefault)
				)
				queryOver.Cacheable().CacheMode(CacheMode.Normal);

			if (predicate != null)
				queryOver = queryOver.Where(predicate);

			if (orderByExpression != null)
				queryOver = queryOver.OrderBy(orderByExpression).Asc;

			return queryOver;
		}

		/// <summary>
		/// Obtém uma única instância de T com base no filtro informado.
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <returns>Única ocorrência de T que atenda ao filtro.</returns>
		public virtual T GetSingleBy(Expression<Func<T, bool>> predicate)
		{
			return this.GetSingleBy(predicate, this.UseCacheByDefault);
		}

		/// <summary>
		/// Obtém uma única instância de T com base no filtro informado.
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <param name="cacheable">Informa para a consulta que pode esta pode ser cacheada</param>
		/// <returns>Única ocorrência de T que atenda ao filtro.</returns>
		public virtual T GetSingleBy(Expression<Func<T, bool>> predicate, bool cacheable)
		{
			IQueryOver<T, T> queryOver = this.GetQuery(predicate: predicate, cacheable: cacheable);
			T returnValue = queryOver.SingleOrDefault();
			return returnValue;
		}

		/// <summary>
		/// Obtém uma única instância de T com base no filtro informado.
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <returns>Primeira ocorrência de T que atenda ao filtro.</returns>
		public virtual T GetFirstBy(Expression<Func<T, bool>> predicate)
		{
			return this.GetFirstBy(predicate, this.UseCacheByDefault);
		}

		/// <summary>
		/// Obtém uma única instância de T com base no filtro informado.
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <param name="cacheable">Informa para a consulta que pode esta pode ser cacheada</param>
		/// <returns>Primeira ocorrência de T que atenda ao filtro.</returns>
		public virtual T GetFirstBy(Expression<Func<T, bool>> predicate, bool cacheable)
		{
			IQueryOver<T, T> queryOver = this.GetQuery(predicate: predicate, cacheable: cacheable);
			T returnValue = queryOver.Take(1).List().FirstOrDefault();
			return returnValue;
		}

		/// <summary>
		/// Obtém uma lista de instâncias de T com base no filtro informado.
		/// </summary>
		/// <param name="predicate">Filtro a ser aplicado.</param>
		/// <param name="cacheable">Informa para a consulta que pode esta pode ser cacheada</param>
		/// <param name="orderByExpression">Identifica o orderby a ser executado na consulta</param>
		/// <returns>Lista com ocorrência de T que atendem ao filtro.</returns>
		public virtual IList<T> GetListBy(Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> orderByExpression = null, bool? cacheable = null)
		{
			IQueryOver<T, T> queryOver = this.GetQuery(predicate: predicate, orderByExpression: orderByExpression, cacheable: cacheable);
			return queryOver.List();
		}

		/// <summary>
		/// Obtém uma lista com todas as instâncias de T.
		/// </summary>
		/// <returns>Lista com ocorrência de T.</returns>
		public virtual IList<T> GetAll()
		{
			return this.GetListBy();
		}

		/// <summary>
		/// Retorna a existência de algum objeto que atenda à expressão de filtro
		/// </summary>
		/// <param name="filter">Expressão de filtro</param>
		/// <returns>Retorna um bool correspondente a existência de algum objeto que atenda à expressão</returns>
		public virtual bool Exists(Expression<Func<T, bool>> filter)
		{ 
			bool returnValue = this.Query<T>().Any(filter);
			return returnValue;
		}

		/// <summary>
		/// Retoan a quantidade de ocorrências com base na expressão
		/// </summary>
		/// <param name="filter">Expressão de filtro</param>
		/// <returns>Retorna um bool correspondente a existência de algum objeto que atenda à expressão</returns>
		public virtual long LongCount(Expression<Func<T, bool>> filter)
		{
			long returnValue = this.Query<T>().LongCount(filter);
			return returnValue;
		}

		/// <summary>
		/// Retoan a quantidade de ocorrências com base na expressão
		/// </summary>
		/// <param name="filter">Expressão de filtro</param>
		/// <returns>Retorna um bool correspondente a existência de algum objeto que atenda à expressão</returns>
		public virtual int Count(Expression<Func<T, bool>> filter)
		{
			int returnValue = this.Query<T>().Count(filter);
			return returnValue;
		}
	}
}