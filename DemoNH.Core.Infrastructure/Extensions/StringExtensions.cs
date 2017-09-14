using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DemoNH.Core.Infrastructure.Extensions
{
	public static partial class OragonExtensions
	{
		/// <summary>
		/// Converte um objeto String para um objeto MemberExpression.
		/// </summary>
		/// <param name="origin">Objeto string original</param>
		/// <param name="p">Parâmetro de entrada do objeto Expression.</param>
		/// <returns>Uma expressão referente ao membro </returns>
		public static Expression ToExpression(this string origin, ParameterExpression p)
		{
			string[] properties = origin.Split('.');

			Type propertyType = p.Type;
			Expression propertyAccess = p;

			foreach (var prop in properties)
			{
				var property = propertyType.GetProperty(prop);
				propertyType = property.PropertyType;
				propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
			}
			return propertyAccess;
		}


		public static string RemoverAcentos(this string origin)
		{
			return new string(origin.Normalize(NormalizationForm.FormD)
				.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) !=
				            UnicodeCategory.NonSpacingMark).ToArray());
		}


		/// <summary>
		/// true, if is valid email address
		/// from http://www.davidhayden.com/blog/dave/
		/// archive/2006/11/30/ExtensionMethodsCSharp.aspx
		/// </summary>
		/// <param name="s">email address to test</param>
		/// <returns>true, if is valid email address</returns>
		public static bool IsValidEmailAddress(this string s)
		{
			return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
		}

		
		/// <summary>
		/// Checks if url is valid. 
		/// from http://www.osix.net/modules/article/?id=586 and changed to match http://localhost
		/// 
		/// complete (not only http) url regex can be found 
		/// at http://internet.ls-la.net/folklore/url-regexpr.html
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool IsValidUrl(this string url)
		{
			string strRegex = "^(https?://)"
			                  + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
			                  + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
			                  + "|" // allows either IP or domain
			                  + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
			                  + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
			                  + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
			                  + "(:[0-9]{1,5})?" // port number- :80
			                  + "((/?)|" // a slash isn't required if there is no file name
			                  + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
			return new Regex(strRegex).IsMatch(url);
		}

	

		

		
	}
}