 
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
			HasManyToMany(x => x.Turmas)
				.ParentKeyColumns.Add("[IdAluno]")
				.Table("[AlunoTurma]")
				.ChildKeyColumns.Add("[IdTurma]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("varchar(30)").Length(30);
			Map(it => it.Idade, "[Idade]").Nullable().CustomSqlType("int");
			this.CompleteMappings();
		}
		
	}
	
	public partial class TurmaMapper : ClassMap<DemoNH.Core.Data.Entity.Turma>
	{

		partial void CompleteMappings();

		public TurmaMapper()
		{
			Table("[Turma]");
			OptimisticLock.None();
			DynamicUpdate();
			Id(it => it.IdTurma, "[IdTurma]").GeneratedBy.Assigned();
			HasManyToMany(x => x.Alunos)
				.ParentKeyColumns.Add("[IdTurma]")
				.Table("[AlunoTurma]")
				.ChildKeyColumns.Add("[IdAluno]")
				.LazyLoad()
				.Fetch.Select()
				.AsBag();
			Map(it => it.Nome, "[Nome]").Not.Nullable().CustomSqlType("varchar(30)").Length(30);
			this.CompleteMappings();
		}
		
	}
	
}
 




