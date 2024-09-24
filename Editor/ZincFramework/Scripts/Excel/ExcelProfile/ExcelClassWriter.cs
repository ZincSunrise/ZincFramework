using System.Collections;
using System.Data;
using System.IO;
using UnityEngine;
using ZincFramework.Load.Editor;
using ZincFramework.Serialization;
using ZincFramework.Serialization.Excel;
using ZincFramework.Serialization.TypeWriter;
using ZincFramework.Writer;



namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelClassWriter
        {
            public static void CreateGenerateExcelClass(ExcelSheet excelSheet, string className)
            {
                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;

                string path = Path.Combine(autoWriteConfig.savePath);
                path = Path.Combine(Application.dataPath, path);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                CreateGenerateExcelClass(excelSheet, className, path);

            }

            public static void CreateGenerateExcelClass(ExcelSheet excelSheet, string className, string path)
            {
                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;

                string dataName = TextUtility.UpperFirstChar(className);
                path = Path.Combine(path, dataName + ".cs");

                File.WriteAllText(path, "");

                using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        int key = ExcelReader.GetContainerKey(excelSheet);

                        CSharpWriter classWriter = new CSharpWriter(streamWriter);
                        classWriter.WriteNamespace(2, autoWriteConfig.usingNamespaces);

                        string infoName = className.Replace("Data", "Info");
                        string namespaces = excelSheet[3, 5];

                        string collectionName = TextUtility.UpperFirstChar(infoName) + 's';
                        if (key == -1)
                        {
                            classWriter.BeginWriteClass(1, namespaces, null, dataName, parents: new string[] { "IExcelData" });
                            classWriter.WriteLine(2, $"{nameof(IEnumerable)} {nameof(IExcelData)}.Collection => {collectionName};");

                            classWriter.WriteAutoProperty(2, $"List<{infoName}>", collectionName, CSharpWriter.Accessors.Public, null, defaultValue: "new ()");
                        }
                        else
                        {
                            classWriter.BeginWriteClass(1, namespaces, null, dataName, parents: new string[] { "IExcelData" });
                            classWriter.WriteLine(2, $"{nameof(IEnumerable)} {nameof(IExcelData)}.Collection => {collectionName};");

                            string keyName = excelSheet[1, key];
                            classWriter.WriteAutoProperty(2, $"Dictionary<{keyName}, {infoName}>", collectionName, CSharpWriter.Accessors.Public, null, defaultValue: "new ()");
                        }

                        classWriter.EndWriteClass(1, false);
                        classWriter.WriteLine(3);

                        SerializableClassWriter.CreateSerializeClass(classWriter, infoName, excelSheet[3, 1], CreateFieldWriteInfo(excelSheet, out int count), false);

                        ExcelMethodCreator.WriteAllMethod(classWriter, infoName, excelSheet, count);
                        classWriter.EndWriteClass(1, true);
                    }
                }
            }

            public static MemberWriteInfo[] CreateFieldWriteInfo(ExcelSheet excelSheet, out int usefulCount)
            {
                int count = 0;
                while (count < excelSheet.ColCount && !string.IsNullOrEmpty(excelSheet[0, count]))
                {
                    count++;
                }

                usefulCount = count;
                MemberWriteInfo[] memberWriteInfos = new MemberWriteInfo[count];
                AutoWriteConfig autoWriteConfig = ConfigManager.Instance.ExcelDefault;

                for (int i = 0; i < count; i++)
                {
                    string ordinal = excelSheet[autoWriteConfig.numberLine, i];
                    memberWriteInfos[i] = new MemberWriteInfo(excelSheet[autoWriteConfig.nameLine, i], excelSheet[autoWriteConfig.typeLine, i], ordinal, true);
                }

                return memberWriteInfos;
            }
        }
    }
}
