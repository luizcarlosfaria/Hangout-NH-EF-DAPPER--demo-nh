using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DemoNH.Core.Infrastructure;
using Spring.Objects.Factory;
using Spring.Objects.Factory.Attributes;

namespace DemoNH.Core.Infrastructure
{

	/// <summary>
	/// RegexMatcher utiliza grupos em RegEx para fazer match entre strings
	/// </summary>
	public class VariablesExtractorMatcher : IMatcher, IInitializingObject
	{
		/// <summary>
		/// Padrão Regex a ser usado na extração das variáveis do nome do arquivo delivery.completed.
		/// </summary>
		[Required]
		public string LeftPattern { get; set; }

		/// <summary>
		/// Padrão Regex a ser usado na extração das variáveis do nome dos diretórios.
		/// </summary>
		[Required]
		public string RightPattern { get; set; }


		/// <summary>
		/// Determina as variáveis que devem estar presentes nas duas sentenças
		/// </summary>
		[Required]
		public string[] VariableNames { get; set; }


		/// <summary>
		/// Realiza checks de configuração para garantir que o plugin está configurado corretamente
		/// </summary>
		private void ValidateConfiguration()
		{
			if (this.VariableNames == null || this.VariableNames.Any() == false)
				throw new System.InvalidOperationException("A pripriedade VariableNames não possui elementos.");

			string[] fileNameGroupNames = (new Regex(this.LeftPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.CultureInvariant)).GetGroupNames();
			if (this.VariableNames.All(expectedGroupName => fileNameGroupNames.Contains(expectedGroupName)) == false)
				throw new System.InvalidOperationException(string.Format("Os grupos definidos na propriedade VariableNames ({0}) não estão presentes na expressão regular da propriedade DeliveryCompletedExtractionRegexPattern ({1})", string.Join(", ", this.VariableNames), this.LeftPattern));


			string[] directoryNameGroupNames = (new Regex(this.RightPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.CultureInvariant)).GetGroupNames();
			if (this.VariableNames.All(expectedGroupName => directoryNameGroupNames.Contains(expectedGroupName)) == false)
				throw new System.InvalidOperationException(string.Format("Os grupos definidos na propriedade VariableNames ({0}) não estão presentes na expressão regular da propriedade DirectoryExtractionRegexPattern ({1})", string.Join(", ", this.VariableNames), this.RightPattern));
		}

		/// <summary>
		/// Realiza o match entre duas strings com base nas configurações de Variáveis e 
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public bool Match(string left, string right)
		{
			List<Variable> leftVariables = RegexHelper.ExtractVariables(this.LeftPattern, left);
			List<Variable> rightVariables = RegexHelper.ExtractVariables(this.RightPattern, right);
			bool matchResult = RegexHelper.Match(this.VariableNames.ToList(), leftVariables, rightVariables);
			return matchResult;
		}

		/// <summary>
		/// Executa a validação durante a inicialização do contexto spring.net
		/// </summary>
		public void AfterPropertiesSet()
		{
			this.ValidateConfiguration();
		}
	}
}
