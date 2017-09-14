using DemoNH.Core.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Apps
{

	public class Dummy { }
	class Program
	{
		private static void Main(params string[] args)
		{
			ServiceProcessEntryPoint.Run<Dummy>(args);
		}
	}
}
