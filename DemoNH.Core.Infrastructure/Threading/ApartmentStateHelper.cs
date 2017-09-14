namespace DemoNH.Core.Infrastructure.Threading
{
	public delegate void SingleMethod();

	public static class ApartmentStateHelper
	{
		public static void RunSta(SingleMethod method)
		{
			Run(method, System.Threading.ApartmentState.STA);
		}

		public static void RunMta(SingleMethod method)
		{
			Run(method, System.Threading.ApartmentState.MTA);
		}

		public static void RunUnknown(SingleMethod method)
		{
			Run(method, System.Threading.ApartmentState.Unknown);
		}

		private static void Run(SingleMethod method, System.Threading.ApartmentState apartmentState)
		{
			System.Threading.ManualResetEvent evnt = new System.Threading.ManualResetEvent(false);

			System.Threading.Thread thread = new System.Threading.Thread(delegate()
			{
				method();
				evnt.Set();
			});

			thread.SetApartmentState(apartmentState);
			thread.Start();
			evnt.WaitOne();
		}
	}
}