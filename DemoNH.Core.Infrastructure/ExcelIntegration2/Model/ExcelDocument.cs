using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoNH.Core.Infrastructure.ExcelIntegration2.Model
{
	public class ExcelDocument
	{
		public string Password { get; set; }

		public string Author { get; set; }

		public string Title { get; set; }

		private List<ExcelSheet> sheets;

		private ExcelDocument()
		{
			this.sheets = new List<ExcelSheet>();
		}

		public ExcelDocument AddSheet(ExcelSheet sheet)
		{
			if (this.sheets.Any(it => it.Name == sheet.Name))
				throw new ArgumentException("Este nome da ExcelSheet já está em uso para esse ExcelDocument");
			this.sheets.Add(sheet);
			return this;
		}

		public static ExcelDocument CreateNewDocument()
		{
			return new ExcelDocument();
		}

		protected void To(Action<ExcelPackage> actionToExecute)
		{
			using (ExcelPackage OXexcelPackage = new ExcelPackage())
			{
				this.WriteSheets(OXexcelPackage);
				this.SetFileProperties(OXexcelPackage);
				if (actionToExecute != null)
					actionToExecute(OXexcelPackage);
			}
		}

		public void ToFile(string fileName)
		{
			this.ToFile(new System.IO.FileInfo(fileName));
		}

		public void ToFile(System.IO.FileInfo file)
		{
			this.To(delegate(ExcelPackage package)
			{
				package.SaveAs(file);
			});
		}

		public Byte[] ToBytes()
		{
			Byte[] bytes = null;
			this.To(delegate(ExcelPackage package)
			{
				bytes = package.GetAsByteArray();
			});
			return bytes;
		}

		private void WriteSheets(ExcelPackage OXexcelPackage)
		{
			foreach (ExcelSheet excelSheet in this.sheets)
			{
				excelSheet.CreateSheet(OXexcelPackage);
			}
		}

		private void SetFileProperties(ExcelPackage OXexcelPackage)
		{
			if (string.IsNullOrWhiteSpace(this.Author) == false)
				OXexcelPackage.Workbook.Properties.Author = this.Author;

			if (string.IsNullOrWhiteSpace(this.Title) == false)
				OXexcelPackage.Workbook.Properties.Title = this.Title;

			if (!string.IsNullOrEmpty(this.Password))
			{
				foreach (ExcelWorksheet OXexcelWorksheet in OXexcelPackage.Workbook.Worksheets)
				{
					OXexcelWorksheet.Protection.IsProtected = true;
					OXexcelWorksheet.Protection.SetPassword(this.Password);
				}
			}
		}
	}
}