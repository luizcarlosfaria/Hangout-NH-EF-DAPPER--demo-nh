using Spring.Threading;

namespace DemoNH.Core.Infrastructure.Threading
{
	public class SessionStorage : IThreadStorage
	{
		private IKeyParser KeyParser { get; set; }

		private string ResolveName(string name)
		{
			string newName = this.KeyParser.GetName(name);
			return newName;
		}

		public void FreeNamedDataSlot(string name)
		{
			if (this.IsValid(name))
			{
				string newName = this.ResolveName(name);
				System.Web.HttpContext.Current.Session.Remove(newName);
			}
		}

		public object GetData(string name)
		{
			object returnValue = null;
			if (this.IsValid(name))
			{
				string newName = this.ResolveName(name);
				returnValue = System.Web.HttpContext.Current.Session[newName];
			}
			return returnValue;
		}

		public void SetData(string name, object value)
		{
			if (this.IsValid(name))
			{
				string newName = this.ResolveName(name);
				System.Web.HttpContext.Current.Session[newName] = value;
			}
		}

		private bool IsValid(string name)
		{
			return ((System.Web.HttpContext.Current != null) && (System.Web.HttpContext.Current.Session != null) && (string.IsNullOrWhiteSpace(name) == false));
		}
	}
}