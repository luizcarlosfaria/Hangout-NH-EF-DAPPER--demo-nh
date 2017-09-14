using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace DemoNH.Core.Infrastructure.Web.NancyFx.PathProviders
{
	public class DebugRootPathProvider : IRootPathProvider
	{
		#region IRootPathProvider Members

		public string GetRootPath()
		{
			return GetDevSolutionFolder();
		}

		#endregion

		public static string GetDevSolutionFolder()
		{
			string returnValue = null;
			DirectoryInfo solutionPublishDebugOrRelease = new DirectoryInfo(System.Environment.CurrentDirectory);
			DirectoryInfo solutionPublish = solutionPublishDebugOrRelease.Parent;
			if (solutionPublish != null)
			{
				DirectoryInfo solution = solutionPublish.Parent;
				if (solution != null)
				{
					DirectoryInfo solutionDemoNHCoreServicesWebManagement =
						solution
							.GetDirectories().Single(it => it.Name == "DemoNH.Core")
							.GetDirectories().Single(it => it.Name == "Services")
							.GetDirectories().Single(it => it.Name == "WebManagement");

					returnValue = solutionDemoNHCoreServicesWebManagement.FullName;
				}
			}
			return returnValue;
		}
	}
}
