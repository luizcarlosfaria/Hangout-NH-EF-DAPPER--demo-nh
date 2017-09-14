﻿using DemoNH.Core.Infrastructure.AOP.Data.Abstractions;
using System.Collections.Generic;
using MongoDBDriver = MongoDB.Driver;

namespace DemoNH.Core.Infrastructure.AOP.Data.MongoDB
{
	public class MongoDBContext : AbstractContext<MongoDBContextAttribute>
	{
		public MongoDBContext(MongoDBContextAttribute contextAttribute, Stack<AbstractContext<MongoDBContextAttribute>> contextStack)
			: base(contextAttribute, contextStack)
		{
			
		}

		protected override void Initialize()
		{
			this.Client = new MongoDBDriver.MongoClient(this.ContextAttribute.MongoDBConnectionString.ConnectionString);
			this.Server = this.Client.GetServer();
		}


		public MongoDBDriver.MongoClient Client { get; private set; }

		public MongoDBDriver.MongoServer Server { get; private set; }

		protected override void DisposeContext()
		{
			base.DisposeContext();
		}

		protected override void DisposeFields()
		{
			this.Server = null;
			this.Client = null;
			base.DisposeFields();
		}
	}
}