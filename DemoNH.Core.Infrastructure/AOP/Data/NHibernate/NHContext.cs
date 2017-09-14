using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using System.Collections.Generic;
using System.Linq;
using NH = NHibernate;

namespace DemoNH.Core.Infrastructure.AOP.Data.NHibernate
{
	public class NHContext : AbstractContext<NHContextAttribute>
	{
		public NH.ISession Session { get; private set; }

		private NH.ITransaction Transaction { get; set; }

		private NHContext ReusedContext { get; set; }

		public NH.ISessionFactory SessionFactory { get; private set; }

		public NH.IInterceptor Interceptor { get; private set; }

		public NHContext(NHContextAttribute contextAttribute, Stack<AbstractContext<NHContextAttribute>> contextStack, NH.IInterceptor interceptor)
			: base(contextAttribute, contextStack)
		{
			this.Interceptor = interceptor;
		}

		protected override void Initialize()
		{
			this.SessionFactory = this.ContextAttribute.SessionFactoryBuilder.GetSessionFactory();
			this.ReusedContext = this.GetContextToReuse();
			if (this.ReusedContext != null)
			{
				this.Session = this.ReusedContext.Session;
				this.Transaction = this.ReusedContext.Transaction;
			}
			else
			{
				this.Session = this.BuildSession(this.Interceptor);
				this.Transaction = this.CreateTransactionIfRequired();
			}
		}


		private NHContext GetContextToReuse()
		{
			NHContext contextToReuse = null;
			if (this.ContextAttribute.CreationStrategy == NHContextCreationStrategy.ReuseOrCreate)
			{
				contextToReuse = (NHContext)this.ContextStack.FirstOrDefault(it =>
					it.ContextAttribute.ContextKey == this.ContextAttribute.ContextKey
					&&
					(
						(this.ContextAttribute.TransactionMode == NHContextTransactionMode.None)
						||
						(it.ContextAttribute.TransactionMode == this.ContextAttribute.TransactionMode)
					)
				);
			}
			return contextToReuse;
		}

		protected bool IsTransactional
		{
			get
			{
				return (this.ContextAttribute.TransactionMode == NHContextTransactionMode.Transactioned);
			}
		}

		/// <summary>
		/// Constr�i uma sess�o NHibernate injetando interceptadores na sess�o de acordo com o estado definido na pr�pria configura��o do ObjectcontextAroundAdvice
		/// </summary>
		/// <param name="sessionFactory"></param>
		/// <returns></returns>
		private NH.ISession BuildSession(NH.IInterceptor interceptor)
		{
			NH.ISession session = null;
			if (interceptor != null)
				session = this.SessionFactory.OpenSession(interceptor);
			else
				session = this.SessionFactory.OpenSession();

			if (this.IsTransactional)
				session.FlushMode = this.ContextAttribute.SessionFactoryBuilder.TransactionFlushMode;
			else
				session.FlushMode = this.ContextAttribute.SessionFactoryBuilder.DefaultFlushMode;
			return session;
		}



		private NH.ITransaction CreateTransactionIfRequired()
		{
			NH.ITransaction returnValue = null;
			if (this.IsTransactional)
			{
				returnValue = this.Session.BeginTransaction(this.ContextAttribute.SessionFactoryBuilder.TransactionIsolationLevel);
			}
			return returnValue;
		}

		public bool Complete()
		{
			bool completePerformed = (this.Transaction != null && this.ReusedContext == null); //Significa que esta inst�ncia n�o pode controlar o dispose pois h� uma heran�a de contexto
			if (completePerformed)
				this.Transaction.Commit();
			return completePerformed;
		}

		#region Dispose Methods

		protected override void DisposeContext()
		{
			if (this.ReusedContext == null)//Significa que esta inst�ncia n�o pode controlar o dispose pois h� uma heran�a de contexto
			{
				//TODO: Adicionado tratamento para a transaction. Na vers�o anterior n�o havia sido codificado, no entanto tamb�m n�o h� nenhum bug conhecido a respeito da falta desse c�digo.
				if (this.Transaction != null)
					this.Transaction.Dispose();

				if (this.Session != null)
					this.Session.Dispose();
			}
			base.DisposeContext();
		}

		protected override void DisposeFields()
		{
			//Valida��o removida. N�o h� necessidade de validar se � ou n�o um contexto Reusado, essas propriedades podem ser limpas sempre.
			this.Session = null;
			this.Transaction = null;
			base.DisposeFields();
		}

		#endregion Dispose Methods
	}
}