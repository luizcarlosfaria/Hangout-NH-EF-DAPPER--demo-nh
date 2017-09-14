using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DemoNH.Core.Infrastructure.ExcelIntegration2.Model
{
	public abstract class ExcelTable
	{
		public string Name { get; protected set; }

		public string Title { get; protected set; }

		protected List<ExcelFetcher> FetchPathList { get; set; }

		protected IEnumerable DataList { get; set; }

		protected ExcelTable(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");
			this.Name = name;
			this.FetchPathList = new List<ExcelFetcher>();
		}

		internal void CreateTable(ExcelWorksheet OXexcelWorksheet, ref int currentRow)
		{
			if (string.IsNullOrWhiteSpace(this.Title) == false)
				this.CreateTableTitle(OXexcelWorksheet, ref currentRow);
			this.CreateHeader(OXexcelWorksheet, ref currentRow);
			this.CreateBody(OXexcelWorksheet, ref currentRow);
		}

		private void CreateTableTitle(ExcelWorksheet currentSheet, ref int currentRow)
		{
			currentSheet.Cells[currentRow, 1].Value = this.Title;
			currentSheet.Cells[currentRow, 1, currentRow, this.FetchPathList.Count].Merge = true;
			currentSheet.Cells[currentRow, 1, currentRow, this.FetchPathList.Count].Style.Font.Bold = true;
			currentSheet.Cells[currentRow, 1, currentRow, this.FetchPathList.Count].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
			currentRow++;
		}

		private void CreateHeader(ExcelWorksheet currentSheet, ref int currentRow)
		{
			System.Drawing.Color colorBG = System.Drawing.ColorTranslator.FromHtml("#5B9BD5");
			System.Drawing.Color colorBorder = System.Drawing.ColorTranslator.FromHtml("#5B9BD5");
			System.Drawing.Color textColor = System.Drawing.Color.White;

			for (int colIndex = 1; colIndex <= this.FetchPathList.Count; colIndex++)
			{
				ExcelRange cell = currentSheet.Cells[currentRow, colIndex];
				cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
				cell.Style.Fill.BackgroundColor.SetColor(colorBG);
				cell.Style.Font.Size = 11;
				cell.Style.Font.Color.SetColor(textColor);

				//Border
				cell.Style.Border.Bottom.Style = cell.Style.Border.Top.Style = cell.Style.Border.Left.Style = cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;
				cell.Style.Border.Bottom.Color.SetColor(colorBorder);
				cell.Style.Border.Top.Color.SetColor(colorBorder);
				cell.Style.Border.Left.Color.SetColor(colorBorder);
				cell.Style.Border.Right.Color.SetColor(colorBorder);

				cell.Value = this.FetchPathList[colIndex - 1].ColumnName;
			}
			currentRow++;
		}

		private void CreateBody(ExcelWorksheet currentSheet, ref int currentRow)
		{
			///int rowIndex = 3;
			foreach (object item in this.DataList)
			{
				int colIndex = 1;
				foreach (ExcelFetcher fetcher in this.FetchPathList)
				{
					ExcelRange cell = currentSheet.Cells[currentRow, colIndex];
					//Obtém o valor extraído da expressão
					object value = fetcher.ExpressionDelegate.DynamicInvoke(item);
					//Atribui à célula o valor extraído
					cell.Value = value;
					this.SetCellFormat(cell, fetcher.Format, ExcelBorderStyle.Thin);
					if (fetcher.CustomFormat != null)
						fetcher.CustomFormat(cell);
					colIndex++;
				}
				currentRow++;
			}
		}

		private void SetCellFormat(ExcelRange cell, ExcelFormat format, ExcelBorderStyle border)
		{
			switch (format)
			{
				case ExcelFormat.Geral:
					break;

				case ExcelFormat.Cientifico:
					cell.Style.Numberformat.Format = "0,00E+00";
					break;

				case ExcelFormat.Fracao:
					cell.Style.Numberformat.Format = "# ?/?";
					break;

				case ExcelFormat.DataCompleta:
					cell.Style.Numberformat.Format = "[$-F800]dddd, mmmm dd, yyyy";
					break;

				case ExcelFormat.DataAbrevidada:
					cell.Style.Numberformat.Format = "dd/mm/yyyy";
					break;

				case ExcelFormat.DataHora:
					cell.Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
					break;

				case ExcelFormat.Numero:
					cell.Style.Numberformat.Format = "0,00";
					break;

				case ExcelFormat.Moeda:
					cell.Style.Numberformat.Format = "R$ #,#.00#";
					break;

				case ExcelFormat.Porcentagem:
					cell.Style.Numberformat.Format = "0,00%";
					break;
			}
			cell.Style.Border.Left.Style = border;
			cell.Style.Border.Right.Style = border;
			cell.Style.Border.Bottom.Style = border;
			cell.Style.Border.Top.Style = border;

			System.Drawing.Color colorBorder = System.Drawing.ColorTranslator.FromHtml("#5B9BD5");

			cell.Style.Border.Bottom.Color.SetColor(colorBorder);
			cell.Style.Border.Top.Color.SetColor(colorBorder);
			cell.Style.Border.Left.Color.SetColor(colorBorder);
			cell.Style.Border.Right.Color.SetColor(colorBorder);
		}
	}

	public class ExcelTable<T> : ExcelTable
	{
		protected ExcelTable(string name)
			: base(name)
		{
		}

		public ExcelTable<T> SetTitle(string title)
		{
			this.Title = title;
			return this;
		}

		public static ExcelTable<T> CreateTable(string displayName)
		{
			return new ExcelTable<T>(displayName);
		}

		public ExcelTable<T> DefineColumn(Expression<Func<T, object>> expression, string columnName, ExcelFormat format = ExcelFormat.Geral, Action<ExcelRange> formatDelegate = null)
		{
			ExcelFetcher newFetcher = new ExcelFetcher();
			newFetcher.PathExpression = expression;
			newFetcher.ColumnName = columnName;
			newFetcher.Format = format;
			newFetcher.CustomFormat = formatDelegate;
			this.FetchPathList.Add(newFetcher);
			return this;
		}

		public ExcelTable<T> SetData(IEnumerable<T> dataList)
		{
			if (dataList == null)
				throw new ArgumentNullException("dataList");
			this.DataList = dataList;
			return this;
		}
	}
}