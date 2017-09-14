using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DemoNH.Core.Infrastructure.Serialization
{
	public static class XmlSerializerUtils
	{
		/// <summary>
		/// Method to convert a custom Object to XML string
		/// </summary>
		/// <param name="pObject">Object that is to be serialized to XML</param>
		/// <returns>XML string</returns>
		public static string SerializeObject<T>(T pObject)
		{
			try
			{
				String XmlizedString = null;
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

				xs.Serialize(xmlTextWriter, pObject);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
				return XmlizedString;
			}
			catch (Exception e)
			{
				System.Console.WriteLine(e);
				return null;
			}
		}

		/// <summary>
		/// Method to reconstruct an Object from XML string
		/// </summary>
		/// <param name="pXmlizedString"></param>
		/// <returns></returns>
		public static T DeserializeObject<T>(string pXmlizedString)
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

			return (T)xs.Deserialize(memoryStream);
		}

		private static byte[] StringToUTF8ByteArray(string pXmlizedString)
		{
			return Encoding.UTF8.GetBytes(pXmlizedString);
		}

		private static string UTF8ByteArrayToString(byte[] p)
		{
			return Encoding.UTF8.GetString(p);
		}
	}
}