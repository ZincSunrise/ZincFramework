using DocumentFormat.OpenXml.Packaging;
using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ZincFramework.Binary.Serialization;
using ZincFramework.Excel;
using ZincFramework.Serialization;



namespace ZincFramework.DialogueSystem.GraphView
{
/*    public static class TreeToExcelConverter
    {
        public static void WriteExcel(TextTree[] textTrees, string directoryPath)
        {
            string tempPath = $"{Application.dataPath}/Editor/ZincFramework/TreeGraphView/TextGraphView/ExcelConverter/Temp.xlsx";

            for (int i = 0; i < textTrees.Length; i++)
            {
                try
                {
                    if (textTrees[i] == null) { continue; }
                    using SpreadsheetDocument temp = SpreadsheetDocument.CreateFromTemplate(tempPath);
                    ExcelBook excelBook = new ExcelBook(temp.WorkbookPart);
                    ExcelWriter excelWriter = new ExcelWriter(excelBook.ExcelSheets[0]);

                    excelWriter.WriteSheet(textTrees[i]);

                    using FileStream fileStream = File.Create(directoryPath + '/' + textTrees[i].name + ExcelModel.Extension);
                    using SpreadsheetDocument newDoc = ExcelWriter.CreateExcelDocument(fileStream, new string[] { textTrees[i].name });

                    ExcelBook newExcelBook = new ExcelBook(newDoc.WorkbookPart);
                    ExcelSheet excelSheet = newExcelBook.ExcelSheets[0];

                    excelBook.ExcelSheets[0].CopyTo(excelSheet);
                    ExcelSytleData excelSytleData = ExcelModel.Instance.DefaultStyleData;
                    AutoWriteConfig autoWriteConfig = ExcelModel.Instance.ExcelDefault;

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
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    Debug.LogError("不可以在已有的Excel未保存的情况下保存文件，未保存的数据已经丢失！");
                }
            }

            AssetDatabase.Refresh();
        }

        private static void WriteSheet(this ExcelWriter excelWriter, TextTree textTree)
        {
            AutoWriteConfig autoWriteConfig = ExcelModel.Instance.ExcelDefault;
            textTree.Rearrange();

            for (int i = 0; i < textTree.Nodes.Count; i++)
            {
                DialogueInfo dialogueInfo = textTree.Nodes[i].GetDialogueInfo();
                string[] rowText = new string[_propertyInfos.Length];

                for (int j = 0; j < _propertyInfos.Length; j++)
                {
                    object obj = _propertyInfos[j].GetValue(dialogueInfo);
                    if (obj != null)
                    {
                        rowText[j] = WriterFactory.Instance.GetWriter(_propertyInfos[j].PropertyType.Name).GetExcelString(obj);
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
                    if (!string.IsNullOrEmpty(sourcePart[i, j]))
                    {
                        targetPart[i, j] = sourcePart[i, j];
                    }
                }
            }
        }
    }*/
}