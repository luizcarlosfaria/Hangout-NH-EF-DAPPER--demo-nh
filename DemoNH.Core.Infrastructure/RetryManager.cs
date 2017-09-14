using System;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure
{
	public static class RetryManager
	{
		public static List<Exception> Try(Action actionToExecute, int tryCount, bool throwException = true, int timeToRetry = 250)
		{
			List<Exception> exceptionList = new List<Exception>();
			bool executionIsDone = false;
			for (int executionCount = 1; ((executionCount <= tryCount) && (executionIsDone == false)); executionCount++)
			{
				RetryManager.Try(
					delegate
					{
						if (actionToExecute != null)
							actionToExecute();
						executionIsDone = true;
					},
					delegate(Exception exception)
					{
						exceptionList.Add(exception);
						System.Threading.Thread.Sleep(timeToRetry);
					}
				);
			}
			if (executionIsDone == false && throwException)
			{
				throw new AggregateException("A execução não foi concluída corretamente", exceptionList.ToArray());
			}
			return exceptionList;
		}

		public static Exception Try(Action action, Action<Exception> exceptionHandler = null, Action finallyHandler = null)
		{
			System.Runtime.CompilerServices.ExtensionAttribute x = null;
			Console.WriteLine(x);

			Exception result = null;
			try
			{
				action();
			}
			catch (Exception ex)
			{
				result = ex;
				if (exceptionHandler != null)
					exceptionHandler(ex);
			}
			finally
			{
				if (finallyHandler != null)
					finallyHandler();
			}
			return result;
		}
	}
}