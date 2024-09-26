using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using UnityEngine.UIElements;



namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelWriter
        {
            public ExcelSheet ExcelSheet { get; }

            public Stylesheet Stylesheet { get; }

            public ExcelWriter(ExcelSheet excelSheet)
            {
                ExcelSheet = excelSheet;
            }

            public ExcelWriter(ExcelSheet excelSheet, Stylesheet stylesheet) : this(excelSheet)
            {
                Stylesheet = stylesheet;
            }


            public static SpreadsheetDocument CreateExcelDocument(Stream stream, string[] sheetNames)
            {
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                SharedStringTablePart sharedStringTablePart = workbookPart.AddNewPart<SharedStringTablePart>();
                sharedStringTablePart.SharedStringTable = new SharedStringTable();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());


                Stylesheet stylesheet = new Stylesheet();
                workbookPart.AddNewPart<WorkbookStylesPart>().Stylesheet = stylesheet;

                for (int i = 0;i < sheetNames.Length; i++)
                {
                    Sheet sheet = new Sheet()
                    {
                        Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = (uint)(i + 1),
                        Name = sheetNames[i]
                    };
                    sheets.AppendChild(sheet);

                }

                return spreadsheetDocument;
            }

            public void WriteRow(int rowIndex, params string[] rowText)
            {
                for (int i = 0; i < rowText.Length; i++)
                {
                    if(!string.IsNullOrEmpty(rowText[i]))
                    {
                        ExcelSheet[rowIndex, i] = rowText[i];
                    }
                }
            }

            public void WriteCell(string cellReference, string cellName)
            {
                ExcelSheet[cellReference] = cellName;
            }
        }
    }
}