using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;



namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelBook
        {
            public ExcelSheet[] ExcelSheets { get; }

            private SharedStringPool SharedStringPool { get; }

            public ExcelBook(WorkbookPart workbookPart)
            {
                SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                
                var sheets = workbookPart.Workbook.Sheets.Descendants<Sheet>();

                ExcelSheets = new ExcelSheet[sheets.Count()];
                SharedStringPool = new SharedStringPool(workbookPart.SharedStringTablePart.SharedStringTable);

                int index = 0;

                foreach (var sheet in sheets) 
                {
                    WorksheetPart worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
                    ExcelSheets[index++] = new ExcelSheet(sheet, worksheetPart.Worksheet, SharedStringPool);
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
}