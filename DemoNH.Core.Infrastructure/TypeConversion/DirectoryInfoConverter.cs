using System.ComponentModel;

namespace DemoNH.Core.Infrastructure.TypeConversion
{
	/// <summary>
	/// Converter for <see cref="T:System.IO.DirectoryInfo" /> instances.
	/// </summary>
	/// <author>Luiz Carlos Faria (.NET)</author>
	public class DirectoryInfoConverter : TypeConverter
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
			if (value is string)
			{
				return new System.IO.DirectoryInfo(value as string);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}