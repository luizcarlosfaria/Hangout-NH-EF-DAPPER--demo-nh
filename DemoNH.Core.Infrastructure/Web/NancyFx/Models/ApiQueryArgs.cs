using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Web.NancyFx.Models
{
	public class ApiQueryArgs
	{
		public int Page { get; set; }
		public int Start { get; set; }
		public int Limit { get; set; }
		public string Sort { get; set; }
		public string Dir { get; set; }
		public string Callback { get; set; }
	}
}
