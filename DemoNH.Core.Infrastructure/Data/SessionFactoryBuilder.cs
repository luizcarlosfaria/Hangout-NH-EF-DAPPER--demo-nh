using DemoNH.Core.Infrastructure.Data.ConnectionStrings;
using Spring.Objects.Factory.Attributes;
using Spring.Threading;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace DemoNH.Core.Infrastructure.Data
{
	/// <summary>
	/// Responsável por inicializar a configuraçãio do NHibernate e disponibilizar um SessionFactory pra a aplicação
	/// </summary>
	public class SessionFactoryBuilder : DemoNH.Core.Infrastructure.Data.ISessionFactoryBuilder
	{
		#region Injeção de Dependência

		/// <summary>
		/// Identifica qual a chave da conexão
		/// </summary>
		[Required]
		public IConStrConfigDiscovery ConStrConfigDiscovery { get; set; }

		/// <summary>
		/// Identifica tipos contidos em
		/// </summary>
		[Required]
		public List<string> TypeNames { get; set; }

		/// <summary>
		/// Define a profundidade máxima para o preenchimento automático do mecanismo de persistência.
		/// </summary>
		[Required]
		public int MaxFetchDepth { get; set; }

		/// <summary>
		/// Define o nível de isolamento da sessão para  contextos não transacionais
		/// </summary>
		[Required]
		public System.Data.IsolationLevel DefaultIsolationLevel { get; private set; }

		/// <summary>
		/// Define o nível de isolamento da sessão para  contextos transacionais
		/// </summary>
		[Required]
		public System.Data.IsolationLevel TransactionIsolationLevel { get; private set; }

		/// <summary>
		/// Define a estratégia de flush para sessões não transacionais
		/// </summary>
		[Required]
		public NHibernate.FlushMode DefaultFlushMode { get; private set; }


		/// <summary>
		/// Define a estratégia de flush para sessões transacionais
		/// </summary>
		[Required]
		public NHibernate.FlushMode TransactionFlushMode { get; private set; }

		/// <summary>
		/// Define o timeout padrão para a execução de comandos
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

		#endregion Injeção de Dependência

		#region Instance State

		private volatile NHibernate.ISessionFactory sessionFactory;

		private Semaphore semaphore = new Semaphore(1);

		#endregion Instance State

		public SessionFactoryBuilder()
		{
		}

		#region Métodos Públicos

		public NHibernate.ISessionFactory GetSessionFactory()
		{
			if (this.sessionFactory == null)
			{
				semaphore.Acquire();
				if (this.sessionFactory == null)//Reteste.. outra thread pode ter feito o preenchimento do campo antes da liberação do semáfoto.
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

		#endregion Métodos Públicos

		#region Métodos Privados

		/// <summary>
		/// Principal método privado, realiza a criação do SessionFactory e este não deve ser criado novamente até que o domínio de aplicação seja finalizado.
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
					throw new ConfigurationErrorsException(string.Format("Não foi possível carregar o tipo '{0}', informado na propriedade TypeNames do SessionFactoryBuilder.", typeName));
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
					throw new ConfigurationErrorsException("A ConnectionString não possui ProviderName configurado. Os valores possívels são: 'MySql.Data.MySqlClient', 'System.Data.DB2Client' e 'System.Data.SqlClient'.");
			}
			return returnValue;
		}

		private ConnectionStringSettings GetProviderName()
		{
			ConnectionStringSettings connStrSettings = null;

			if (this.ConStrConfigDiscovery == null)
				throw new ConfigurationErrorsException(string.Format("Não foi possível identificar a ConnectionString"));

			connStrSettings = this.ConStrConfigDiscovery.GetConnectionString();

			if (connStrSettings == null)
				throw new ConfigurationErrorsException("Não foi possível identificar a ConnectionString");

			string providerName = connStrSettings.ProviderName;
			if (string.IsNullOrWhiteSpace(providerName))
				throw new ConfigurationErrorsException("A ConnectionString não possui ProviderName configurado. Os valores possívels são: 'MySql.Data.MySqlClient', 'System.Data.DB2Client' e 'System.Data.SqlClient'.");

			return connStrSettings;
		}

		#endregion Métodos Privados
	}
}