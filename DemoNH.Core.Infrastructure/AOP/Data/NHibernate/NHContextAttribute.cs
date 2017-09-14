using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using DemoNH.Core.Infrastructure.Data;
using System;

namespace DemoNH.Core.Infrastructure.AOP.Data.NHibernate
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class NHContextAttribute : AbstractContextAttribute
	{
		public NHContextAttribute(string contextKey, NHContextCreationStrategy creationStrategy = NHContextCreationStrategy.ReuseOrCreate, NHContextTransactionMode transactionMode = NHContextTransactionMode.None)
		{
			this.ContextKey = contextKey;
			this.TransactionMode = transactionMode;
			this.CreationStrategy = creationStrategy;
		}

		public NHContextTransactionMode TransactionMode { get; private set; }

		public NHContextCreationStrategy CreationStrategy { get; private set; }

		internal ISessionFactoryBuilder SessionFactoryBuilder { get; set; }
	}
}