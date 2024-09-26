using System.IO;
using ZincFramework.Excel;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using ZincFramework.Serialization;
using DocumentFormat.OpenXml.Packaging;
using System;
using DocumentFormat.OpenXml.Spreadsheet;


namespace ZincFramework.TreeGraphView.TextTree
{
    public static class TreeToExcelConverter
    {
        public static void WriteExcel(TextTree[] textTrees, string directoryPath)
        {
            string tempPath = $"{Application.dataPath}/Editor/ZincFramework/TreeGraphView/TextGraphView/ExcelConverter/Temp.xlsx";

          
            for (int i = 0; i < textTrees.Length; i++)
            {
                if (textTrees[i] == null) { continue; }
                using SpreadsheetDocument temp = SpreadsheetDocument.CreateFromTemplate(tempPath);
                ExcelBook excelBook = new ExcelBook(temp.WorkbookPart);
                ExcelWriter excelWriter = new ExcelWriter(excelBook.ExcelSheets[0]);

                excelWriter.WriteSheet(textTrees[i]);
                
                using FileStream fileStream = File.Create(directoryPath + '/' + textTrees[i].name + ExcelTool.Extension);
                using SpreadsheetDocument newDoc = ExcelWriter.CreateExcelDocument(fileStream, new string[] { textTrees[i].name });

                ExcelBook newExcelBook = new ExcelBook(newDoc.WorkbookPart);
                ExcelSheet excelSheet = newExcelBook.ExcelSheets[0];

                excelBook.ExcelSheets[0].CopyTo(excelSheet);
                ExcelSytleData excelSytleData = ExcelResManager.Instance.DefaultStyleData;
                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;

                for (int k = 0; k < excelSheet.RowCount; k++)
                {
                    excelSheet.SetRowHeight(k, k == autoWriteConfig.tipsLine ? excelSytleData.height * 4 : excelSytleData.height);
                    for (int j = 0; j < excelSheet.ColCount; j++)
                    {
                        excelSheet.SetColumnWidth(j, excelSytleData.width);
                    }
                }
                newDoc.Save();
            }

            AssetDatabase.Refresh();
        }

        private static void WriteSheet(this ExcelWriter excelWriter, TextTree textTree)
        {
            AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;
            textTree.Nodes.Sort((a, b) => a.Index - b.Index);
            PropertyInfo[] propertyInfos = typeof(DialogueInfo).GetProperties();

            for (int i = 0;i < textTree.Nodes.Count; i++)
            {
                DialogueInfo dialogueInfo = textTree.Nodes[i].GetDialogueInfo();
                string[] rowText = new string[propertyInfos.Length];

                for (int j = 0; j < propertyInfos.Length; j++) 
                {
                    object obj = propertyInfos[j].GetValue(dialogueInfo);

                    if (obj is Array array)
                    {
                        if(array.Length > 0)
                        {
                            string[] array2 = new string[array.Length];
                            for (int k = 0; k < array.Length; k++)
                            {
                                array2[k] = array.GetValue(k)?.ToString();
                            }

                            rowText[j] = string.Join('/', array2);
                        }
                    }
                    else if(obj != null)
                    {
                        rowText[j] = obj.ToString();
                    }
                }

                excelWriter.WriteRow(autoWriteConfig.startLine + i, rowText);
            }
        }

        private static void CopyTo(this ExcelSheet sourcePart, ExcelSheet targetPart)
        {
            for (int i = 0; i < sourcePart.RowCount; i++) 
            {
                for (int j = 0; j < sourcePart.ColCount; j++) 
                {
                    targetPart[i, j] = sourcePart[i, j];
                }
            }
        }
    }
}