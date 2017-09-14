namespace DemoNH.Core.Infrastructure.Globalization
{
	public interface IContextualMessageSource
	{
		void ApplyResources(object value, string objectName);

		string GetMessage(string messageName);

		object GetResourceObject(string name);
	}
}