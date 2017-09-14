using DemoNH.Core.Infrastructure.Services;
using Quartz;

namespace DemoNH.Core.Infrastructure.Schedule
{
	public class SchedulerService : IService
	{
		public IScheduler Scheduler { get; set; }

		public bool OnStoWwaitForJobsToComplete { get; set; }

		public string Name
		{
			get { return "SchedulerService"; }
		}

		public void Start()
		{
			this.Scheduler.Start();
		}

		public void Stop()
		{
			this.Scheduler.Shutdown(this.OnStoWwaitForJobsToComplete);
		}
	}
}