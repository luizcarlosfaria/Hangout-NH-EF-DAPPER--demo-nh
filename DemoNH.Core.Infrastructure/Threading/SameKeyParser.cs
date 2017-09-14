namespace DemoNH.Core.Infrastructure.Threading
{
	public class SameKeyParser : IKeyParser
	{
		public string GetName(string name)
		{
			return name;
		}
	}
}