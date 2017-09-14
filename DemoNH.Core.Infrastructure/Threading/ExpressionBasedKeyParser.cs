using System.Collections;
using System.Collections.Generic;

namespace DemoNH.Core.Infrastructure.Threading
{
	public class ExpressionBasedKeyParser : IKeyParser
	{
		public string Expression { get; set; }

		public IDictionary ExpressionParameters { get; set; }

		public string GetName(string name)
		{
			string returnValue = null;
			if (string.IsNullOrWhiteSpace(name) == false)
			{
				Dictionary<string, object> currentExpressionParameters = this.getExpressionParameters();
				currentExpressionParameters.Add("Name", name);
				returnValue = (string)Spring.Expressions.ExpressionEvaluator.GetValue(null, this.Expression, currentExpressionParameters);
			}
			return returnValue;
		}

		private Dictionary<string, object> getExpressionParameters()
		{
			Dictionary<string, object> currentExpressionParameters = new Dictionary<string, object>();
			foreach (object key in this.ExpressionParameters.Keys)
			{
				var value = this.ExpressionParameters[key];

				currentExpressionParameters.Add((string)key, value);
			}
			this.AddWebParameters(currentExpressionParameters);
			return currentExpressionParameters;
		}

		private void AddWebParameters(Dictionary<string, object> currentExpressionParameters)
		{
			//currentExpressionParameters.Add("HttpContext", System.Web.HttpContext.Current);

			string HttpContextCurrentUserIdentityName = string.Empty;
			if (
					(System.Web.HttpContext.Current != null)
					&&
					(System.Web.HttpContext.Current.User != null)
					&&
					(System.Web.HttpContext.Current.User.Identity != null)
					&&
					(System.Web.HttpContext.Current.User.Identity.Name != null)
				)
				HttpContextCurrentUserIdentityName = System.Web.HttpContext.Current.User.Identity.Name;

			currentExpressionParameters.Add("HttpContextCurrentUserIdentityName", HttpContextCurrentUserIdentityName);
		}
	}
}