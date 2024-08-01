using System.Data;
using System.IO;

namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelReader
        {
            public static DataRow GetVariableName(DataTable dataTable)
            {
                return dataTable.Rows[0];
            }

            public static DataRow GetVariableType(DataTable dataTable)
            {
                return dataTable.Rows[1];
            }

            public static DataRow GetVariableCode(DataTable dataTable)
            {
                return dataTable.Rows[4];
            }

            public static int GetContainerKey(DataTable dataTable)
            {
                DataRow dataRow = dataTable.Rows[2];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    if (dataRow[i].ToString() == "key")
                    {
                        return i;
                    }
                }
                return 0;
            }

            public static FileInfo[] ReadExcel()
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(ExcelTool.ExcelPath);
                return directoryInfo.GetFiles();
            }
        }
    }
}

