using DocumentFormat.OpenXml.Packaging;
using System.IO;
using UnityEditor;
using UnityEngine;
using ZincFramework.Serialization;

namespace ZincFramework.Excel
{
    public class ExcelTool
    {
        public static int StartLine => ExcelModel.Instance.ExcelDefault.startLine;

        [MenuItem("GameTool/Excel/TestExcel")]
        public static void TextExcel()
        {
            string loadPath = EditorUtility.OpenFilePanel("选择一个Excel", Application.dataPath, "xlsx");
            if (!string.IsNullOrEmpty(loadPath))
            {
                using FileStream fileStream = File.Open(loadPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileStream, false);
                ExcelSheet[] excelSheets = ExcelReader.GenerateExcelBook(spreadsheetDocument).ExcelSheets;
            }
        }


        [MenuItem("GameTool/Excel/GenerateBinary")]
        private static void ExcelToBinary()
        {
            var files = ExcelReader.ReadExcelDirectory();

            foreach (var file in files) 
            {
                using FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                ReadExcel(fileStream);
                AssetDatabase.Refresh();

            }
        }


        [MenuItem("GameTool/Excel/ExcelToDataObject")]
        private static void ExcelToDataObject()
        {
            string loadPath = EditorUtility.OpenFilePanel("选择一个Excel", Application.dataPath, "xlsx");
            if (!string.IsNullOrEmpty(loadPath))
            {
                string savePath = EditorUtility.SaveFolderPanel("选择保存的文件夹", Application.dataPath, "ExcelData");

                if (!string.IsNullOrEmpty(savePath))
                {
                    using FileStream fileStream = File.OpenRead(loadPath);
                    ReadExcel(fileStream);
                    AssetDatabase.SaveAssets();
                }
            }

            AssetDatabase.Refresh();
        }


        [MenuItem("GameTool/Excel/CreateExcel")]
        private static void CreateExcel()
        {
            string loadPath = EditorUtility.SaveFilePanel("选择一个Excel", Application.dataPath, "DefaultName", "xlsx");

            if (!string.IsNullOrEmpty(loadPath))
            {
                using (FileStream fileStream = File.Create(loadPath))
                {
                    using var doc = ExcelWriter.CreateExcelDocument(fileStream, new string[] { "铸币1", "铸币2" });
                    doc.Save();
                }

                AssetDatabase.Refresh();
            }
        }

        private static void ReadExcel(FileStream fileStream)
        {
            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileStream, false);
            ExcelSheet[] excelSheets = ExcelReader.GenerateExcelBook(spreadsheetDocument).ExcelSheets;

            foreach (ExcelSheet excelSheet in excelSheets)
            {
                if (!string.IsNullOrEmpty(excelSheet[3, 3]))
                {
                    ExcelClassWriter.CreateGenerateExcelClass(excelSheet, excelSheet[3, 3]);
                }

                ExcelStreamWriter.CreateBinaryFile(excelSheet);
            }
        }
    }
}
