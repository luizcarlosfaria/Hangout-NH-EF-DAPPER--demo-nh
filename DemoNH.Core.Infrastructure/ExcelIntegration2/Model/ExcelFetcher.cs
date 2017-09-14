using OfficeOpenXml;
using System;
using System.Linq.Expressions;

namespace DemoNH.Core.Infrastructure.ExcelIntegration2.Model
{
	/// <summary>
	/// Define um Fetcher
	/// </summary>
	public class ExcelFetcher
	{
		private Delegate expressionDelegate;

		/// <summary>
		/// Expressão Lambda que define como obter o valor do método
		/// </summary>
		public LambdaExpression PathExpression { get; set; }

		/// <summary>
		/// Define o nome da coluna para extração
		/// </summary>
		public string ColumnName { get; set; }

		/// <summary>
		/// Define um formato pra exibição do tipo de conteúdo
		/// </summary>
		public ExcelFormat Format { get; set; }

		/// <summary>
		/// Aplica um formato à célula em questão
		/// </summary>
		public Action<ExcelRange> CustomFormat { get; set; }

		/// <summary>
		/// Retorna um delegate com a expressão compilada.
		/// </summary>
		public Delegate ExpressionDelegate
		{
			get
			{
				if (this.expressionDelegate == null)
				{
					this.expressionDelegate = this.PathExpression.Compile();
				}
				return this.expressionDelegate;
			}
		}
	}
}