using NHibernate;
using NHibernate.Linq;

namespace DemoNH.Core.Infrastructure.AOP.Data.NHibernate
{
	public class NHDataProcess : DemoNH.Core.Infrastructure.AOP.Data.Abstractions.AbstractDataProcess<DemoNH.Core.Infrastructure.AOP.Data.NHibernate.NHContext, DemoNH.Core.Infrastructure.AOP.Data.NHibernate.NHContextAttribute>
	{
		/// <summary>
		/// Obtém um IQueryOver pronto para realizar consultas usando lambda expressions
		/// </summary>
		/// <returns></returns>
		protected virtual System.Linq.IQueryable<T> Query<T>()
			where T : DemoNH.Core.Infrastructure.Business.Entity
		{
			System.Linq.IQueryable<T> query = this.ObjectContext.Session.Query<T>();
			return query;
		}

		/// <summary>
		/// Obtém um IQueryOver (ICriteria API) para que possa ser utilizado em consultas com lambda expressions
		/// </summary>
		/// <returns></returns>
		protected virtual IQueryOver<T, T> QueryOver<T>()
			where T : DemoNH.Core.Infrastructure.Business.Entity
		{
			IQueryOver<T, T> query = this.ObjectContext.Session.QueryOver<T>();
			return query;
		}

		/// <summary>
		/// Reanexa um objeto
		/// </summary>
		/// <param name="itemToAttach"></param>
		public virtual void Attach(DemoNH.Core.Infrastructure.Business.Entity itemToAttach)
		{
			this.ObjectContext.Session.Refresh(itemToAttach, LockMode.None);
		}

		public virtual bool IsAttached(object itemToAttach)
		{
			bool returnValue = this.ObjectContext.Session.Contains(itemToAttach);
			return returnValue;
		}

		public virtual void Flush()
		{
			this.ObjectContext.Session.Flush();
		}
		public virtual void Clear()
		{
			this.ObjectContext.Session.Clear();
		}

		public virtual void Evict<T>(T objectToEvict)
			where T : DemoNH.Core.Infrastructure.Business.Entity
		{
			this.ObjectContext.Session.Evict(objectToEvict);
		}
	}
}