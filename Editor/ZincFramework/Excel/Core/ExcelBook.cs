using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq;
using UnityEngine;



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
                
                WorkbookStylesPart stylesPart = workbookPart.WorkbookStylesPart;
                Stylesheet stylesheet = stylesPart.Stylesheet;

                stylesheet.AppendChild(new CellFormat()
                {
                    Alignment = new Alignment()
                    {
                        WrapText = true,
                    },
                    
                    ApplyAlignment = true,
                });

                uint styleIndex = (uint)stylesheet.Descendants<CellFormat>().Count() - 1;

                foreach (var sheet in sheets) 
                {
                    WorksheetPart worksheetPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
                    ExcelSheets[index++] = new ExcelSheet(sheet, worksheetPart.Worksheet, styleIndex, SharedStringPool);
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