using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure
{
	public interface IMatcher
	{
		bool Match(string left, string right);
	}
}
