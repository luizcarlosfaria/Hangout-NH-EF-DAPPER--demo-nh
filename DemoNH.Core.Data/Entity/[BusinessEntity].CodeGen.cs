using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DemoNH.Core.Infrastructure.UnityOfWork;

namespace DemoNH.Core.Data.UnityOfWork
{
	using DemoNH.Core.Data.Entity;
	public class PersistenceQueueItem : PersistenceQueueItem<EntityBase>
	{
		public PersistenceQueueItem(EntityBase itemToPersist, PersistenceAction action)
			: base(itemToPersist, action){}
	}
}

namespace DemoNH.Core.Data.Entity
{

	public class EntityBase: DemoNH.Core.Infrastructure.Business.Entity
	{
	
	}



	/// <summary>
	/// Classe Aluno.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Aluno: EntityBase
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Turmas da Aluno.
		/// </summary>
		[DataMember]
		public virtual IList<Turma> Turmas { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdAluno da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.IdAluno int</remarks>
		[DataMember]
		public virtual int IdAluno { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.Nome varchar(30)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Idade da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.Idade int</remarks>
		[DataMember]
		public virtual int? Idade { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Aluno))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Aluno objTyped = (Aluno)obj;
			bool returnValue = ((this.IdAluno.Equals(objTyped.IdAluno)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdAluno.GetHashCode());
		}

		#endregion		

	}
	/// <summary>
	/// Classe Turma.
	/// </summary>
	[Serializable]
	[DataContract(IsReference=true)]
	public partial class Turma: EntityBase
	{
		#region "Propriedades"

		
		/// <summary>
		/// Define ou obtém um(a) Alunos da Turma.
		/// </summary>
		[DataMember]
		public virtual IList<Aluno> Alunos { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) IdTurma da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.IdTurma int</remarks>
		[DataMember]
		public virtual int IdTurma { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Turma.
		/// </summary>
		/// <remarks>Referencia Coluna Turma.Nome varchar(30)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }

		#endregion
		

		#region Equals/GetHashCode 


		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Turma))
				return false;
			if (Object.ReferenceEquals(this, obj))
				return true;
			Turma objTyped = (Turma)obj;
			bool returnValue = ((this.IdTurma.Equals(objTyped.IdTurma)));
			return returnValue;
		}

		public override int GetHashCode()
		{
			return (this.IdTurma.GetHashCode());
		}

		#endregion		

	}
	
}
 
