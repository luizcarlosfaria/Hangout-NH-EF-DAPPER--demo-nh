using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure.Web.UX
{
	/// <summary>
	/// Enforce contracts to User Interface
	/// </summary>
	public interface IUXHandeableItem
	{

		/// <summary>
		/// Define a unique type name for User Experience Handlers
		/// </summary>
		string UXType { get; set; }
	}
}
