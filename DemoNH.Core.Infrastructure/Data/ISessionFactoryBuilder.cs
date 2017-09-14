using System;
namespace DemoNH.Core.Infrastructure.Data
{
	public interface ISessionFactoryBuilder
	{
		NHibernate.ISessionFactory GetSessionFactory();
		
		System.Data.IsolationLevel DefaultIsolationLevel { get; }

		System.Data.IsolationLevel TransactionIsolationLevel { get; }

		NHibernate.FlushMode DefaultFlushMode { get; }

		NHibernate.FlushMode TransactionFlushMode { get; }
	}
}
