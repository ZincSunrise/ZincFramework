using System.IO;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using ZincFramework.Json;
using ZincFramework.Excel;
using ZincFramework.Binary;
using ZincFramework.Binary.Serialization;
using ZincFramework.LoadServices.Addressable;
using ZincFramework.Json.Serialization;
using DocumentFormat.OpenXml.Packaging;


namespace ZincFramework
{
    namespace Localization
    {
        public static class LocalizationTool
        {
            private static string _saveDir = Path.Combine(Application.dataPath, "Application", "Localization");

            [MenuItem("GameTool/Localization/GenerateLocalFile")]
            public static void GenerateLocalFile()
            {
                ExcelUtility.CloseExcel();
                string defaultDir = EditorPrefs.GetString("defaultDir", Application.dataPath);
                string loadPath = EditorUtility.OpenFilePanel("选择一个本地化Excel文件", defaultDir, "xlsx");

                if (!string.IsNullOrEmpty(loadPath))
                {
                    if(defaultDir != loadPath)
                    {
                        EditorPrefs.SetString("defaultDir", Path.GetDirectoryName(loadPath));
                    }
         
                    if (!Directory.Exists(_saveDir))
                    {
                        Directory.CreateDirectory(_saveDir);
                    }

                    using (FileStream fileStream = File.OpenRead(loadPath))
                    {
                        SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileStream, false);
                        ExcelBook excelBook = new ExcelBook(spreadsheetDocument.WorkbookPart);
                        for (int i = 0; i < excelBook.ExcelSheets.Length; i++) 
                        {
                            ExcelSheet excelSheet = excelBook.ExcelSheets[i];
                            for (int j = 2; j < excelSheet.ColCount; j++) 
                            {
                                string cultureName = CultureInfo.GetCultureInfo(excelSheet[0, j]).Name;

                                if (!string.IsNullOrEmpty(cultureName))
                                {
                                    string savePath = Path.Combine(_saveDir, cultureName);
                                    if (!Directory.Exists(savePath))
                                    {
                                        Directory.CreateDirectory(savePath);
                                    }

                                    savePath = Path.Combine(savePath, excelSheet.TableName);
                                    CreateLocalInfo(excelSheet, savePath, cultureName, j);
                                }
                            }
                        }
                    }

                    AssetDatabase.Refresh();
                }
            }

            private static void CreateLocalInfo(ExcelSheet excelSheet, string savePath, string cultureName, int colCount)
            {
                LocalizationInfo localizationInfo = new LocalizationInfo();
                for (int i = 1; i < excelSheet.RowCount; i++) 
                {
                    string str = excelSheet[i, colCount];

                    if (!string.IsNullOrEmpty(str))
                    {
                        localizationInfo.Add(excelSheet[i, 0], str);
                    }
                }

                using(FileStream fileStream = File.Create(savePath + JsonDataManager.Extension))
                {
                    JsonSelector.Serialize<LocalizationInfo>(localizationInfo, fileStream, true, Json.JsonSerializeType.JsonSerializer);
                }

                using (FileStream fileStream = File.Create(savePath + BinaryDataManager.Extension))
                {
                    BinarySerializer.Serialize<LocalizationInfo>(localizationInfo, fileStream, LocalizationManager.Instance.LocalSerializerOption);
                }

                AssetDatabase.SaveAssets();

                string path = savePath[(Application.dataPath.Length - "Assets".Length)..] + BinaryDataManager.Extension;
                AddressablesUtility.AddObjectInGroup(cultureName, path);
                AddressablesUtility.AddLabelnGroup(cultureName, path);
            }
        }
    }
}
