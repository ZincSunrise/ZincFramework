using System.Data;
using System.IO;
using ZincFramework.Binary;
using ZincFramework.Load;


namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelFileCreator
        {
            public static void CreateBinaryFile(DataTable dataTable)
            {
                if (!Directory.Exists(FrameworkPaths.ProfilePath))
                {
                    Directory.CreateDirectory(FrameworkPaths.ProfilePath);
                }

                string extension = ResourcesManager.Instance.Load<FrameworkData>("FrameWork/FrameWorkData").extension;

                using (FileStream fs = File.Open(FrameworkPaths.ProfilePath + '/' + $"{dataTable.TableName}{extension}", FileMode.Create, FileAccess.Write))
                {
                    DataRow dataRow;
                    DataRow typeRow = ExcelReader.GetVariableType(dataTable);
                    DataRow codeRow = ExcelReader.GetVariableCode(dataTable);
                    int keyLine = ExcelReader.GetContainerKey(dataTable);

                    string keyName = dataTable.Rows[1][keyLine] as string;
                    //先写容器中存储了多少元素
                    ByteWriter.WriteInt16((short)(dataTable.Rows.Count - ExcelTool.StartLine), fs);

                    for (int i = ExcelTool.StartLine; i < dataTable.Rows.Count; i++)
                    {
                        dataRow = dataTable.Rows[i];
                        ExcelFieldProfileWriter.WriteSingleVariable(keyName, dataRow[keyLine].ToString(), fs);

                        for (int j = 0; j < dataTable.Columns.Count; j++)
                        {
                            //先写字段名
                            ExcelFieldProfileWriter.WriteField(typeRow[j].ToString(), codeRow, dataRow, j, fs);
                        }
                    }
                    fs.Close();
                }
            }
        }
    }
}
