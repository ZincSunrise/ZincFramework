using System.Collections;
using System.IO;
using UnityEngine;
using ZincFramework.Serialization;
using ZincFramework.Serialization.TypeWriter;
using ZincFramework.ScriptWriter;
using System;
using System.Collections.Generic;
using ZincFramework.Binary.Excel;



namespace ZincFramework
{
    namespace Excel
    {
        public static class ExcelClassWriter
        {
            public static void CreateGenerateExcelClass(ExcelSheet excelSheet, string className)
            {
                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;

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
                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;

                string dataName = TextUtility.UpperFirstChar(className);
                path = Path.Combine(path, dataName + ".cs");

                if (File.Exists(path))
                {
                    File.WriteAllText(path, string.Empty);
                }

                using (FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int key = ExcelReader.GetContainerKey(excelSheet);

                    CSharpWriter classWriter = CSharpWriter.RentWriter();
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

                    var memberInfos = CreateFieldWriteInfo(excelSheet, out int count);
                    SerializableClassWriter.CreateSerializeClass(classWriter, infoName, excelSheet[3, 1], memberInfos, false);

                    ExcelMethodCreator.WriteAllMethod(infoName, classWriter, memberInfos);
                    classWriter.EndWriteClass(1, true);

                    classWriter.WriteAndReturn(fileStream);
                }
            }

            public static List<MemberWriteInfo> CreateFieldWriteInfo(ExcelSheet excelSheet, out int usefulCount)
            {
                int count = 0;
                while (count < excelSheet.ColCount && !string.IsNullOrEmpty(excelSheet[0, count]))
                {
                    count++;
                }

                usefulCount = count;
                List<MemberWriteInfo> memberWriteInfos = new List<MemberWriteInfo>();
                AutoWriteConfig autoWriteConfig = ExcelResManager.Instance.ExcelDefault;

                for (int i = 0; i < count; i++)
                {
                    string type = excelSheet[autoWriteConfig.typeLine, i];
                    if(type.Equals("ignore", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    string ordinal = excelSheet[autoWriteConfig.numberLine, i];
                    memberWriteInfos.Add(new MemberWriteInfo(0, excelSheet[autoWriteConfig.nameLine, i], type, ordinal, true));
                }

                return memberWriteInfos;
            }
        }
    }
}
