using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;



namespace ZincFramework.Excel
{
    public class ExcelBook
    {
        public ExcelSheet[] ExcelSheets { get; }

        public ExcelBook(WorkbookPart workbookPart)
        {
            var sheets = workbookPart.Workbook.Sheets.Descendants<Sheet>();

            ExcelSheets = new ExcelSheet[sheets.Count()];
            int index = 0;

            WorkbookStylesPart stylesPart = workbookPart.WorkbookStylesPart;

            SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart?.SharedStringTable;

            Stylesheet stylesheet = stylesPart.Stylesheet;

            stylesheet.AppendChild(new CellFormat()
            {
                Alignment = new Alignment()
                {
                    WrapText = true,
                },

                ApplyAlignment = true,
            });

            foreach (var sheet in sheets)
            {
                WorksheetPart worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
                ExcelSheets[index++] = new ExcelSheet(sheet, worksheetPart.Worksheet, sharedStringTable);
                UnityEngine.Debug.Log(ExcelSheets[index - 1]);
            }
        }

        public void AddSheet(string sheetName)
        {

        }

        public void RemoveSheet(string sheetName)
        {

        }
    }
}