using AopAlliance.Intercept;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DemoNH.Core.Infrastructure.Globalization
{
	public class GlobalizationAroundAdvice : IMethodInterceptor
	{
		public string[] PossibleArgumentNames { get; set; }

		public List<CultureInfo> SuportedCultures { get; set; }

		public object Invoke(IMethodInvocation invocation)
		{
			CultureInfo processCultureInfo = this.GetCultureInfo(invocation);
			object returnValue = null;
			Spring.Threading.LogicalThreadContext.SetData("ContextCulture", processCultureInfo);
			returnValue = invocation.Proceed();
			Spring.Threading.LogicalThreadContext.SetData("ContextCulture", null);
			Spring.Threading.LogicalThreadContext.FreeNamedDataSlot("ContextCulture");
			return returnValue;
		}

		private CultureInfo GetCultureInfo(IMethodInvocation invocation)
		{
			CultureInfo returnValue = null;
			var methodArgs = invocation.Method.GetParameters();
			var argValues = invocation.Arguments;
			for (int i = 0; i < methodArgs.Length; i++)
			{
				var methodArg = methodArgs[i];
				if (this.PossibleArgumentNames.Contains(methodArg.Name))
				{
					var argValue = argValues[i];
					if (argValue is string)
					{
						string argValueStr = (string)argValue;
						CultureInfo cultureInfo = new CultureInfo(argValueStr);
						returnValue = this.GetSupportedCultureByName(cultureInfo);
					}
					else if (argValue is int)
					{
						int argValueInt = (int)argValue;
						CultureInfo cultureInfo = new CultureInfo(argValueInt);
						returnValue = this.GetSupportedCultureByName(cultureInfo);
					}
				}
				if (returnValue != null)
					break;
			}

			if (returnValue == null)
				returnValue = this.SuportedCultures.FirstOrDefault();

			return returnValue;
		}

		private CultureInfo GetSupportedCultureByName(CultureInfo cultureInfo)
		{
			CultureInfo returnValue = null;
			returnValue = this.SuportedCultures.Where(it => it.LCID == cultureInfo.LCID).FirstOrDefault();
			if (returnValue != null)
				return returnValue;

			returnValue = this.SuportedCultures.Where(it => it.LCID == cultureInfo.Parent.LCID).FirstOrDefault();
			if (returnValue != null)
				return returnValue;

			returnValue = this.SuportedCultures.Where(it => it.Parent.LCID == cultureInfo.Parent.LCID).FirstOrDefault();
			if (returnValue != null)
				return returnValue;

			return returnValue;
		}
	}
}