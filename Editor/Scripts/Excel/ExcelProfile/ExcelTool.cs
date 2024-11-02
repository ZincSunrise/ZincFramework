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

            public static int StartLine => ExcelResManager.Instance.ExcelDefault.startLine;

            public const string Extension = ".xlsx";


            [MenuItem("GameTool/Excel/GenerateBinary")]
            private static void ExcelToBinary()
            {
                FileInfo[] files = ExcelReader.ReadExcelDirectory();
                ExcelUtility.CloseExcel();

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
                string loadPath = EditorUtility.OpenFilePanel("ѡ��һ��Excel", Application.dataPath, "xlsx");
                if (!string.IsNullOrEmpty(loadPath))
                {
                    string savePath = EditorUtility.SaveFolderPanel("ѡ�񱣴���ļ���", Application.dataPath, "ExcelData");

                    if (!string.IsNullOrEmpty(savePath)) 
                    {
                        ExcelUtility.CloseExcel();

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
                string loadPath = EditorUtility.SaveFilePanel("ѡ��һ��Excel", Application.dataPath, "DefaultName", "xlsx");

                if (!string.IsNullOrEmpty(loadPath))
                {
                    using (FileStream fileStream = File.Create(loadPath))
                    {
                        using var doc = ExcelWriter.CreateExcelDocument(fileStream, new string[] { "����1", "����2"});
                        doc.Save();
                    }

                    AssetDatabase.Refresh();
                }
            }
        }
    }
}