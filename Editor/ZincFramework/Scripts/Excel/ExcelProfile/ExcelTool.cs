using ZincFramework.Serialization;
using System.IO;
using UnityEditor;
using UnityEngine;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;



namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelTool
        {
            public static string ExcelPath { get; } = Application.dataPath + "/ArtRes/Excel/";

            public static int StartLine => ConfigManager.Instance.ExcelDefault.startLine;

            public const string Extension = ".xlsx";


            [MenuItem("GameTool/Excel/GenerateBinary")]
            private static void ExcelToBinary()
            {
                FileInfo[] files = ExcelReader.ReadExcelDirectory();
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Extension != Extension)
                    {
                        continue;
                    }

                    using (FileStream fileStream = files[i].Open(FileMode.Open, FileAccess.Read))
                    {
                        using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileStream, false);
                        ExcelSheet[] excelSheets = ExcelReader.GetExcelBook(spreadsheetDocument).ExcelSheets;

                        foreach (ExcelSheet excelSheet in excelSheets)
                        {
                            if (!string.IsNullOrEmpty(excelSheet[3, 3]))
                            {
                                ExcelClassWriter.CreateGenerateExcelClass(excelSheet, excelSheet[3, 3]);
                            }

                            ExcelStreamWriter.CreateBinaryFile(excelSheet);
                        }
                    }
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
                        using (FileStream fileStream = File.OpenRead(loadPath))
                        {
                            using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileStream, false);
                            ExcelSheet[] excelSheets = ExcelReader.GetExcelBook(spreadsheetDocument).ExcelSheets;

                            foreach (ExcelSheet excelSheet in excelSheets)
                            {
                                if (!string.IsNullOrEmpty(excelSheet[3, 3]))
                                {
                                    ExcelClassWriter.CreateGenerateExcelClass(excelSheet, excelSheet[3, 3], savePath);
                                }
                            }

                            AssetDatabase.SaveAssets();
                        }
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
                        using var doc = ExcelWriter.CreateExcelDocument(fileStream, new string[] { "铸币1", "铸币2"});
                        ExcelBook excelBook = new ExcelBook(doc.WorkbookPart);

                        for (int i = 0; i < excelBook.ExcelSheets.Length; i++) 
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                for (int k = 0; k < 10; k++)
                                {
                                    ExcelSheet excelSheet = excelBook.ExcelSheets[i];
                                    excelSheet[j, k] = "卧槽i吗";
                                }
                            }
                        }

                        doc.Save();
                    }

                    AssetDatabase.Refresh();
                }
            }
        }
    }
}
