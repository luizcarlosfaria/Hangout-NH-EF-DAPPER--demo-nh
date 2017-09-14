using System.ComponentModel;

namespace DemoNH.Core.Infrastructure.TypeConversion
{
	public class ReaderQuotasConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert an object of one
		/// <see cref="T:System.Type" /> to a <see cref="T:System.IO.DirectoryInfo" />
		/// </summary>
		/// <remarks>
		/// <p>
		/// Currently only supports conversion from a
		/// <see cref="T:System.String" /> instance.
		/// </p>
		/// </remarks>
		/// <param name="context">
		/// A <see cref="T:System.ComponentModel.ITypeDescriptorContext" />
		/// that provides a format context.
		/// </param>
		/// <param name="sourceType">
		/// A <see cref="T:System.Type" /> that represents the
		/// <see cref="T:System.Type" /> you want to convert from.
		/// </param>
		/// <returns>True if the conversion is possible.</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Convert from a string value to a <see cref="T:System.IO.DirectoryInfo" /> instance.
		/// </summary>
		/// <param name="context">
		/// A <see cref="T:System.ComponentModel.ITypeDescriptorContext" />
		/// that provides a format context.
		/// </param>
		/// <param name="culture">
		/// The <see cref="T:System.Globalization.CultureInfo" /> to use
		/// as the current culture.
		/// </param>
		/// <param name="value">
		/// The value that is to be converted.
		/// </param>
		/// <returns>
		/// A <see cref="T:System.IO.DirectoryInfo" /> if successful.
		/// </returns>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			object returnValue = null;
			string strValue = value as string;
			switch (strValue)
			{
				case "Default":
					returnValue = typeof(System.Xml.XmlDictionaryReaderQuotas).GetField("defaultQuota", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);
					break;

				case "Max":
					returnValue = typeof(System.Xml.XmlDictionaryReaderQuotas).GetField("maxQuota", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).GetValue(null);
					break;

				default:
					returnValue = base.ConvertFrom(context, culture, value);
					break;
			}
			return returnValue;
		}
	}
}