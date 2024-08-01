using Excel;
using ZincFramework.Serialization;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace ZincFramework
{
    namespace Excel
    {
        public class ExcelTool
        {
            public static string ExcelPath { get; } = Application.dataPath + "/ArtRes/Excel/";

            public static string ClassPath { get; } = Application.dataPath + "/Scripts/GameData/";

            public static int StartLine => AutoWriteConfig.ExcelDefault.StartLine;

            public const string Extension = ".xlsx";

            [MenuItem("GameTool/Excel/GenerateBinary")]
            private static void ExcelToBinary()
            {
                FileInfo[] files = ExcelReader.ReadExcel();
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Extension != Extension)
                    {
                        continue;
                    }
                    DataTableCollection dataTableCollection;
                    using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                        dataTableCollection = reader.AsDataSet().Tables;
                    }

                    foreach (DataTable dataTable in dataTableCollection)
                    {
                        if (dataTable.Rows[3][3] != null && dataTable.Rows[3][3].ToString() != "")
                        {
                            ExcelClassWriter.CreateGenerateExcelClass(dataTable, dataTable.Rows[3][3].ToString());
                        }
                        ExcelFileCreator.CreateBinaryFile(dataTable);
                    }
                    AssetDatabase.Refresh();
                }

            }
        }
    }
}
