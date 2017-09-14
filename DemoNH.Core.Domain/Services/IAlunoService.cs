using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Services
{
	[ServiceContract]
	public interface IAlunoService
    {
		#region Public Methods

		[OperationContract]
		void Execute();

		#endregion Public Methods
	}
}