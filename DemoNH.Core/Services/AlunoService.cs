using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoNH.Core.Infrastructure.AOP.Data.NHibernate;
using DemoNH.Core.Data.Process;
using DemoNH.Core.Data.Entity;

namespace DemoNH.Core.Services
{
	public class AlunoService : IAlunoService
	{
        #region Public Properties

        public PersistenceDataProcess WriteDP { get; set; }
        public AlunoDataProcess AlunoDP { get; set; }
        
        //public TurmaDataProcess TurmaDP { get; set; }

        #endregion Public Properties

        #region Public Properties

        [NHContext(contextKey: "SQLSERVER", creationStrategy: NHContextCreationStrategy.CreateNew, transactionMode: NHContextTransactionMode.Transactioned)]
        //[NHContext(contextKey: "MYSQL", creationStrategy: NHContextCreationStrategy.CreateNew, transactionMode: NHContextTransactionMode.None)]
        public void Execute()
		{
            //Turma turma = new Turma() { Nome = "Turma do Canal" };
            //this.WriteDP.Save(turma);

            this.WriteDP.Save(new Aluno
            {
                Nome = "Luiz Carlos Faria",
                //Turma = turma
            });


            
        }

		#endregion Public Properties
	}
}