using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZincFramework.Serialization;

namespace ZincFramework.Excel
{
    public static class ExcelReader
    {
        /// <summary>
        /// 读取一整个文件夹中的文件信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<FileInfo> ReadExcelDirectory()
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(ExcelModel.ExcelPath);
            return directoryInfo.GetFiles().Where(x => x.Extension == ExcelModel.Extension);
        }

        public static ExcelBook GenerateExcelBook(SpreadsheetDocument spreadsheetDocument)
        {
            return new ExcelBook(spreadsheetDocument.WorkbookPart);
        }

        /// <summary>
        /// 读取谁是容器键
        /// </summary>
        /// <param name="excelSheet"></param>
        /// <returns></returns>
        public static int GetContainerKey(ExcelSheet excelSheet)
        {
            AutoWriteConfig config = ExcelModel.Instance.ExcelDefault;
            ExcelRow excelRow;

            if ((excelRow = excelSheet[config.keyLine]) != null)
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
    }
}


