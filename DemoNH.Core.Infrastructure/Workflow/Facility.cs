namespace DemoNH.Core.Infrastructure.Workflow.Facility
{
	public class StringStateMachine : StateMachine<StringTransition, string> { }

	public class StringTransition : Transition<string> { }
}

namespace DemoNH.Core.Infrastructure.Workflow.Facility
{
	public class IntStateMachine : StateMachine<IntTransition, int> { }

	public class IntTransition : Transition<int> { }
}