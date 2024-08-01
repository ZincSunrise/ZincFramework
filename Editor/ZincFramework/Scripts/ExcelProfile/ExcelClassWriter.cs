using System.Data;
using System.IO;
using ZincFramework.Serialization;
using ZincFramework.Serialization.TypeWriter;
using ZincFramework.Writer;
using ZincFramework.Writer.Handle;



namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelClassWriter
        {
            public static void CreateGenerateExcelClass(DataTable dataTable, string className)
            {
                if (!Directory.Exists(ExcelTool.ClassPath))
                {
                    Directory.CreateDirectory(ExcelTool.ClassPath);
                }

                string dataName = TextUtility.UpperFirstString(className);
                string path = Path.Combine(ExcelTool.ClassPath, dataName + ".cs");

                File.WriteAllText(path, "");

                using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        int key = ExcelReader.GetContainerKey(dataTable);
                        string keyName = dataTable.Rows[1][key] as string;

                        CSharpWriter classWriter = new CSharpWriter(streamWriter);
                        classWriter.WriteNamespace(2, AutoWriteConfig.ExcelDefault.UsingNamespaces);

                        string infoName = className.Replace("Data", "Info");
                        string namespaces = dataTable.Rows[3][5].ToString();

                        IWriteHandle writeHandle = classWriter.BeginWriteClass(1, namespaces, null, dataName);
                        classWriter.WriteGenericField(2, TextUtility.UpperFirstString(infoName) + "s", "Dictionary", new string[] { keyName, infoName });
                        classWriter.EndWriteClass(1, false);
                        classWriter.WriteLine(3);

                        SerializableClassWriter.CreateSerializeClass(classWriter, infoName, dataTable.Rows[3][1].ToString(), CreateFieldWriteInfo(dataTable), false);
                        ExcelMethodCreator.WriteAllMethod(classWriter, infoName, dataTable);
                        classWriter.EndWriteClass(1, true);
                    }
                }
            }

            public static MemberWriteInfo[] CreateFieldWriteInfo(DataTable dataTable)
            {
                MemberWriteInfo[] memberWriteInfos = new MemberWriteInfo[dataTable.Columns.Count];
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    memberWriteInfos[i] = new MemberWriteInfo(dataTable, dataTable.Columns[i], true);
                }

                return memberWriteInfos;
            }
        }
    }
}
