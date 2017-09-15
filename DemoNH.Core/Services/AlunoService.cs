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
            //var turma = new Turma() { IdTurma = 1, Nome = "Turma do Canal .NET" };
            //this.WriteDP.Save(turma);

            //var aluno = new Aluno
            //{
            //    Nome = "Usuário de exemplo 15",
            //    Idade = 15,
            //    //Turma = turma
            //    Turmas = new List<Turma>() { turma }
            //};
            //this.WriteDP.Save(aluno);

            var aluno = this.AlunoDP.GetSingleBy(it => it.Idade == 15);
            Console.WriteLine(aluno.Nome);
            foreach (var turma in aluno.Turmas)
            {
                Console.WriteLine(turma.Nome);
            }


        }

        #endregion Public Properties
    }
}