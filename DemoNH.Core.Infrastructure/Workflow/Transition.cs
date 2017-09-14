using System;

namespace DemoNH.Core.Infrastructure.Workflow
{
	public abstract class Transition
	{
	}

	public abstract class Transition<StateType> : Transition
		where StateType : IComparable
	{
		public StateType Origin { get; set; }

		public StateType Destination { get; set; }

		public StateType GetDestination()
		{
			return this.Destination;
		}

		public StateType GetOrigin()
		{
			return this.Origin;
		}
	}
}