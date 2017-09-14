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
		/// Define ou obtém um(a) IdAluno da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.IdAluno int</remarks>
		[DataMember]
		public virtual int IdAluno { get; set; }
		
		/// <summary>
		/// Define ou obtém um(a) Nome da Aluno.
		/// </summary>
		/// <remarks>Referencia Coluna Aluno.Nome varchar(50)</remarks>
		[DataMember]
		public virtual string Nome { get; set; }

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
	
}
 
