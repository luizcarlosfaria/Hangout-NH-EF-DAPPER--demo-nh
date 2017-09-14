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
		/// Express�o Lambda que define como obter o valor do m�todo
		/// </summary>
		public LambdaExpression PathExpression { get; set; }

		/// <summary>
		/// Define o nome da coluna para extra��o
		/// </summary>
		public string ColumnName { get; set; }

		/// <summary>
		/// Define um formato pra exibi��o do tipo de conte�do
		/// </summary>
		public ExcelFormat Format { get; set; }

		/// <summary>
		/// Aplica um formato � c�lula em quest�o
		/// </summary>
		public Action<ExcelRange> CustomFormat { get; set; }

		/// <summary>
		/// Retorna um delegate com a express�o compilada.
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