using System;
using System.Linq;
using System.Reflection;

namespace DemoNH.Core.Infrastructure.Data
{
	[AttributeUsage(AttributeTargets.All)]
	public class EnumValueAttribute : Attribute
	{
		public object Value { get; set; }

		private static Type SelfType = typeof(EnumValueAttribute);

		public EnumValueAttribute(object value)
		{
			this.Value = value;
		}

		public static TEnum ToEnum<TEnum>(object value)
			where TEnum : struct, IConvertible
		{
			if (!typeof(TEnum).IsEnum)
				throw new ArgumentException("T must be an enumerated type");

			TEnum returnValue = default(TEnum);

			foreach (FieldInfo field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
			{
				EnumValueAttribute enumValueAttr = field.GetCustomAttributes(SelfType, false).FirstOrDefault() as EnumValueAttribute;
				if ((enumValueAttr != null) && ((string)enumValueAttr.Value == (string)value))
				{
					returnValue = (TEnum)field.GetRawConstantValue();
					break;
				}
			}
			return returnValue;
		}

		public static object ToValue<TEnum>(object value)
			where TEnum : struct, IConvertible
		{
			if (!typeof(TEnum).IsEnum)
				throw new ArgumentException("T must be an enumerated type");

			object returnValue = null;

			if (value is int)
				returnValue = ToValue<TEnum>((int)value);
			else
				returnValue = ToValue<TEnum>((string)value);
			return returnValue;
		}

		private static object ToValue<TEnum>(string value)
		{
			object returnValue = null;
			foreach (FieldInfo field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
			{
				if (field.GetValue(null).Equals(value))
				{
					EnumValueAttribute enumValueAttr = field.GetCustomAttributes(SelfType, false).FirstOrDefault() as EnumValueAttribute;
					if (enumValueAttr != null)
					{
						returnValue = enumValueAttr.Value;
					}
					break;
				}
			}
			return returnValue;
		}

		private static object ToValue<TEnum>(int value)
		{
			object returnValue = null;
			Array enumValues = Enum.GetValues(typeof(TEnum));
			if ((value >= 0) && (value < enumValues.Length))
				returnValue = enumValues.GetValue(value);
			return returnValue;
		}
	}
}