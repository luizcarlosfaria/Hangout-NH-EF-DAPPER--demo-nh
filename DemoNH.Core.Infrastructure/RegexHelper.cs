using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoNH.Core.Infrastructure
{

	/// <summary>
	/// Define uma variável extraída de um texto
	/// </summary>
	public class Variable
	{
		/// <summary>
		/// Nome da variável
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Valor
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Resultado de match
		/// </summary>
		public bool HasValue { get; internal set; }

		public Variable(string name, string value, bool hasValue)
		{
			this.Name = name;
			this.Value = value;
			this.HasValue = hasValue;
		}
	}

	public static class VariableExtensions
	{
		public static bool HasAllVariables(this IEnumerable<Variable> variables)
		{
			return variables.All(variable => variable.HasValue);
		}
	
	}

	/// <summary>
	/// Auxilia no trabalho com expressões regulares simples e complexas
	/// </summary>
	public class RegexHelper
	{

		/// <summary>
		/// Realiza a extração de variáveis a partir de uma string usando Expressão Regular
		/// </summary>
		/// <param name="pattern">Expressão Regular responsável pela identificação das variáveis e literais da sentença</param>
		/// <param name="text">Sentença para análise (nomes de diretórios e nomes de arquivos)</param>
		/// <returns></returns>
		public static List<Variable> ExtractVariables(string pattern, string text)
		{
			List<Variable> result = new List<Variable>();
			var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);
			Match match = regex.Match(text);
			foreach (string groupName in regex.GetGroupNames())
			{
				result.Add(new Variable(groupName, match.Groups[groupName].Value, match.Groups[groupName].Success));
			}
			return result;
		}

		/// <summary>
		/// Analisa as variáveis extraídas do nome do arquivo e do nome do diretório, identificando se cada variável faz match uma com a outra.
		/// </summary>
		/// <param name="deliveryCompletedVariables">Lista de varáveis do nome do arquivo</param>
		/// <param name="directoryVariables">Lista de varáveis do nome do diretório</param>
		/// <returns>Determina se o match aconteceu com sucesso ou falha.</returns>
		public static bool Match(List<string> variableNames, List<Variable> deliveryCompletedVariables, List<Variable> directoryVariables)
		{
			var query = from currentGroupName in variableNames
						join deliveryCompleteVariable in deliveryCompletedVariables on currentGroupName equals deliveryCompleteVariable.Name
						join directoryVariable in directoryVariables on currentGroupName equals directoryVariable.Name
						where
							deliveryCompleteVariable.HasValue
							&&
							directoryVariable.HasValue
						select (deliveryCompleteVariable.Value == directoryVariable.Value);
			//A query faz join em 3 listas
			// A primeira lista (VariableNames) determina 
			bool returnValue = (query.Count(success => success) == variableNames.Count);
			return returnValue;
		}

	}
}
