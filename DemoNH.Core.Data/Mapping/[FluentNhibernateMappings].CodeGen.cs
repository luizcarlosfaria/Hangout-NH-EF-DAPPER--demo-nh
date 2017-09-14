 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using DemoNH.Core.Data.Entity;
using FluentNHibernate.Mapping;

namespace DemoNH.Core.Data.Mapping
{
	
	public partial class AlunoMapper : ClassMap<DemoNH.Core.Data.Entity.Aluno>
	{

		partial void CompleteMappings();

		public AlunoMapper()
		{
			Table("[Aluno]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdAluno, "[IdAluno]").GeneratedBy.Identity();
			Map(it => it.Nome, "[Nome]").Nullable().CustomSqlType("varchar(50)").Length(50);
			this.CompleteMappings();
		}
		
	}
	
}
 




