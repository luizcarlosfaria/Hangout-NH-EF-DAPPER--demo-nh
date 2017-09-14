namespace DemoNH.Core.Infrastructure.Services
{
	public interface IService
	{
		string Name
		{
			get;
		}

		void Start();

		void Stop();
	}
}