using DemoNH.Core.Infrastructure.Business;
using MongoDBDriver = MongoDB.Driver;

namespace DemoNH.Core.Infrastructure.AOP.Data.MongoDB
{
	public class MongoDBDataProcess : DemoNH.Core.Infrastructure.AOP.Data.Abstractions.AbstractDataProcess<MongoDBContext, MongoDBContextAttribute>
	{
	}

	public class MongoDBDataProcess<T> : DemoNH.Core.Infrastructure.AOP.Data.Abstractions.AbstractDataProcess<MongoDBContext, MongoDBContextAttribute>
		where T : Entity
	{
		protected string CollectionName { get; set; }

		protected string DataBaseName { get; set; }

		protected virtual MongoDBDriver.MongoDatabase GetDataBase()
		{
			return this
					.ObjectContext
					.Server
					.GetDatabase(this.DataBaseName);
		}

		protected virtual MongoDBDriver.MongoCollection<T> Collection
		{
			get
			{
				return this
					.GetDataBase()
					.GetCollection<T>(this.CollectionName);
			}
		}
	}
}