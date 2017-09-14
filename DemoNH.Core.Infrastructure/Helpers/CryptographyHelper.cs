using System.Security.Cryptography;
using System.Text;

namespace DemoNH.Core.Infrastructure.Helpers
{
	public class CryptographyHelper
	{
		public static string Md5(string String)
		{
			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.ASCII.GetBytes(String);
				byte[] hash = md5.ComputeHash(inputBytes);

				return ByteArrayToString(hash);
			}
		}

		public static string Sha512(string phrase)
		{
			var encoder = new UTF8Encoding();
			var sha512Hasher = new SHA512Managed();

			byte[] hashedDataBytes = sha512Hasher.ComputeHash(encoder.GetBytes(phrase));

			return ByteArrayToString(hashedDataBytes);
		}

		public static string ByteArrayToString(byte[] inputArray)
		{
			var output = new StringBuilder("");

			foreach (byte t in inputArray)
			{
				output.Append(t.ToString("X2"));
			}

			return output.ToString();
		}
	}
}