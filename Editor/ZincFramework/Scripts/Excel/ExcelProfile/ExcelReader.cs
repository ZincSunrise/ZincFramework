using DocumentFormat.OpenXml.Packaging;
using System.IO;
using ZincFramework.Serialization;



namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelReader
        {
            public static int GetContainerKey(ExcelSheet excelSheet)
            {
                AutoWriteConfig config = ConfigManager.Instance.ExcelDefault;
                if (!excelSheet.TryGetRow(config.keyLine, out var excelRow))
                { 
                    return -1;
                }

                for (int i = 0; i < excelSheet.ColCount; i++)
                {
                    if ("key".Equals(excelRow[i], System.StringComparison.OrdinalIgnoreCase))
                    {
                        return i;
                    }
                }

                return -1;
            }


            public static FileInfo[] ReadExcelDirectory()
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(ExcelTool.ExcelPath);
                return directoryInfo.GetFiles();
            }

            public static ExcelBook GetExcelBook(SpreadsheetDocument spreadsheetDocument)
            {
                return new ExcelBook(spreadsheetDocument.WorkbookPart);
            }
        }
    }
}

