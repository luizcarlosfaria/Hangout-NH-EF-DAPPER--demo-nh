using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.ExcelIntegration2.Model
{
	public class ExcelSheet
	{
		public string Name { get; protected set; }

		protected List<ExcelTable> tables;

		private ExcelSheet(string name)
		{
			this.tables = new List<ExcelTable>();

			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException("name");
			this.Name = name;
		}

		public static ExcelSheet CreateNewSheet(string name)
		{
			return new ExcelSheet(name);
		}

		public ExcelSheet SetName(string name)
		{
			this.Name = name;
			return this;
		}

		public ExcelSheet AddTable(ExcelTable table)
		{
			if (this.tables.Any(it => it.Name == table.Name))
				throw new ArgumentException("Este nome da ExcelTable já está em uso para esse ExcelSheet");
			this.tables.Add(table);
			return this;
		}

		internal void CreateSheet(ExcelPackage package)
		{
			package.Workbook.Worksheets.Add(this.Name);
			ExcelWorksheet OXexcelWorksheet = package.Workbook.Worksheets.Last();
			OXexcelWorksheet.Name = this.Name; //Setting Sheet's name
			OXexcelWorksheet.Cells.Style.Font.Size = 10; //Default font size for whole sheet
			OXexcelWorksheet.Cells.Style.Font.Name = "Calibri"; //Default Font name for whole sheet

			int currentRow = 1;
			foreach (ExcelTable excelTable in this.tables)
			{
				excelTable.CreateTable(OXexcelWorksheet, ref currentRow);
				currentRow++;
			}
		}
	}
}