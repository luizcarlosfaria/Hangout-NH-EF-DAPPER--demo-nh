using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DemoNH.Core.Infrastructure.Data.Process;


namespace DemoNH.Core.Data.Process
{
	public sealed class PersistenceDataProcess : PersistenceDataProcessBase<DemoNH.Core.Data.Entity.EntityBase> { }

	public partial class AlunoDataProcess : QueryDataProcess<DemoNH.Core.Data.Entity.Aluno>
	{
		

	}
	
}

