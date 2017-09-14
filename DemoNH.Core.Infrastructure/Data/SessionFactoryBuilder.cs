using DemoNH.Core.Infrastructure.Data.ConnectionStrings;
using Spring.Objects.Factory.Attributes;
using Spring.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DemoNH.Core.Infrastructure.Data
{
	/// <summary>
	/// Respons�vel por inicializar a configura��io do NHibernate e disponibilizar um SessionFactory pra a aplica��o
	/// </summary>
	public class SessionFactoryBuilder : DemoNH.Core.Infrastructure.Data.ISessionFactoryBuilder
	{
		#region Inje��o de Depend�ncia

		/// <summary>
		/// Identifica qual a chave da conex�o
		/// </summary>
		[Required]
		public IConStrConfigDiscovery ConStrConfigDiscovery { get; set; }

		/// <summary>
		/// Identifica tipos contidos em
		/// </summary>
		[Required]
		public List<string> TypeNames { get; set; }

		/// <summary>
		/// Define a profundidade m�xima para o preenchimento autom�tico do mecanismo de persist�ncia.
		/// </summary>
		[Required]
		public int MaxFetchDepth { get; set; }

		/// <summary>
		/// Define o n�vel de isolamento da sess�o para  contextos n�o transacionais
		/// </summary>
		[Required]
		public System.Data.IsolationLevel DefaultIsolationLevel { get; private set; }

		/// <summary>
		/// Define o n�vel de isolamento da sess�o para  contextos transacionais
		/// </summary>
		[Required]
		public System.Data.IsolationLevel TransactionIsolationLevel { get; private set; }

		/// <summary>
		/// Define a estrat�gia de flush para sess�es n�o transacionais
		/// </summary>
		[Required]
		public NHibernate.FlushMode DefaultFlushMode { get; private set; }


		/// <summary>
		/// Define a estrat�gia de flush para sess�es transacionais
		/// </summary>
		[Required]
		public NHibernate.FlushMode TransactionFlushMode { get; private set; }

		/// <summary>
		/// Define o timeout padr�o para a execu��o de comandos
		/// </summary>
		[Required]
		public int CommandTimeout { get; set; }

		/// <summary>
		/// Define o default Batch Size
		/// </summary>
		[Required]
		public int BatchSize { get; set; }

		/// <summary>
		/// Define uma chave para acesso ao banco
		/// </summary>
		[Required]
		public string ObjectContextKey { get; set; }

		[Required]
		public bool EnabledDiagnostics { get; set; }

		#endregion Inje��o de Depend�ncia

		#region Instance State

		private volatile NHibernate.ISessionFactory sessionFactory;

		private Semaphore semaphore = new Semaphore(1);

		#endregion Instance State

		public SessionFactoryBuilder()
		{
		}

		#region M�todos P�blicos

		public NHibernate.ISessionFactory GetSessionFactory()
		{
			if (this.sessionFactory == null)
			{
				semaphore.Acquire();
				if (this.sessionFactory == null)//Reteste.. outra thread pode ter feito o preenchimento do campo antes da libera��o do sem�foto.
				{
					try
					{
						this.sessionFactory = this.BuildSessionFactoryInternal();
					}
					catch (Exception)
					{
						throw;
					}
					finally
					{
						semaphore.Release();
					}
				}
				else
					semaphore.Release();
			}
			return this.sessionFactory;
		}

		#endregion M�todos P�blicos

		#region M�todos Privados

		/// <summary>
		/// Principal m�todo privado, realiza a cria��o do SessionFactory e este n�o deve ser criado novamente at� que o dom�nio de aplica��o seja finalizado.
		/// </summary>
		/// <returns></returns>
		private NHibernate.ISessionFactory BuildSessionFactoryInternal()
		{
			FluentNHibernate.Cfg.Db.IPersistenceConfigurer databaseConfiguration = this.GetDataBaseConfiguration();

			FluentNHibernate.Cfg.FluentConfiguration configuration = FluentNHibernate.Cfg.Fluently
				.Configure()
				.Database(databaseConfiguration)
				.Cache(it =>
					it.UseQueryCache()
					.ProviderClass<NHibernate.Cache.HashtableCacheProvider>()
				)
				.Diagnostics(it =>
					it.Enable(this.EnabledDiagnostics)
					.OutputToConsole()
				)
				.ExposeConfiguration(it =>
					it
					.SetProperty("command_timeout", this.CommandTimeout.ToString())
					.SetProperty("adonet.batch_size", this.BatchSize.ToString())
				);

			foreach (string typeName in this.TypeNames)
			{
				Type typeInfo = Type.GetType(typeName);
				if (typeInfo == null)
					throw new ConfigurationErrorsException(string.Format("N�o foi poss�vel carregar o tipo '{0}', informado na propriedade TypeNames do SessionFactoryBuilder.", typeName));
				configuration.Mappings(it =>
				{
					it.FluentMappings.AddFromAssembly(typeInfo.Assembly);
					it.HbmMappings.AddFromAssembly(typeInfo.Assembly);
				});
			}

			NHibernate.ISessionFactory sessionFactory = configuration.BuildSessionFactory();
			return sessionFactory;
		}

		private FluentNHibernate.Cfg.Db.IPersistenceConfigurer GetDataBaseConfiguration()
		{
			FluentNHibernate.Cfg.Db.IPersistenceConfigurer returnValue = null;
			ConnectionStringSettings connectionStringSettings = this.GetProviderName();
			string connectionStringSettingsProviderName = connectionStringSettings.ProviderName;
			switch (connectionStringSettingsProviderName)
			{
				case "MySql.Data.MySqlClient":
					var configMySqlClient = FluentNHibernate.Cfg.Db.MySQLConfiguration.Standard
						.ConnectionString(connectionStringSettings.ConnectionString)
						.MaxFetchDepth(this.MaxFetchDepth)
						.IsolationLevel(this.DefaultIsolationLevel);
					if (this.EnabledDiagnostics)
						configMySqlClient = configMySqlClient.ShowSql().FormatSql();
					returnValue = configMySqlClient;
					break;

				case "System.Data.SqlClient":
					var configSqlClient = FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008
						.ConnectionString(connectionStringSettings.ConnectionString)
						.MaxFetchDepth(this.MaxFetchDepth)
						.IsolationLevel(this.DefaultIsolationLevel);
					if (this.EnabledDiagnostics)
						configSqlClient = configSqlClient.ShowSql().FormatSql();
					returnValue = configSqlClient;
					break;

				case "System.Data.DB2Client":
					var configDB2Client = FluentNHibernate.Cfg.Db.DB2Configuration.Standard
						.ConnectionString(connectionStringSettings.ConnectionString)
						.MaxFetchDepth(this.MaxFetchDepth)
						.IsolationLevel(this.DefaultIsolationLevel);
					if (this.EnabledDiagnostics)
						configDB2Client = configDB2Client.ShowSql().FormatSql();
					returnValue = configDB2Client;
					break;

				case "System.Data.SQLite":
					var configSQLiteClient = FluentNHibernate.Cfg.Db.SQLiteConfiguration.Standard
						.ConnectionString(connectionStringSettings.ConnectionString)
						.MaxFetchDepth(this.MaxFetchDepth)
						.IsolationLevel(this.DefaultIsolationLevel);
					if (this.EnabledDiagnostics)
						configSQLiteClient = configSQLiteClient.ShowSql().FormatSql();
					returnValue = configSQLiteClient;
					break;

				default:
					throw new ConfigurationErrorsException("A ConnectionString n�o possui ProviderName configurado. Os valores poss�vels s�o: 'MySql.Data.MySqlClient', 'System.Data.DB2Client' e 'System.Data.SqlClient'.");
			}
			return returnValue;
		}

		private ConnectionStringSettings GetProviderName()
		{
			ConnectionStringSettings connStrSettings = null;

			if (this.ConStrConfigDiscovery == null)
				throw new ConfigurationErrorsException(string.Format("N�o foi poss�vel identificar a ConnectionString"));

			connStrSettings = this.ConStrConfigDiscovery.GetConnectionString();

			if (connStrSettings == null)
				throw new ConfigurationErrorsException("N�o foi poss�vel identificar a ConnectionString");

			string providerName = connStrSettings.ProviderName;
			if (string.IsNullOrWhiteSpace(providerName))
				throw new ConfigurationErrorsException("A ConnectionString n�o possui ProviderName configurado. Os valores poss�vels s�o: 'MySql.Data.MySqlClient', 'System.Data.DB2Client' e 'System.Data.SqlClient'.");

			return connStrSettings;
		}

		#endregion M�todos Privados
	}
}