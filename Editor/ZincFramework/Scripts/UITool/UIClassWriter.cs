using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using ZincFramework.Writer;


namespace ZincFramework
{
    namespace UI
    {
        public static class UIClassWriter
        {
            public readonly static string[] _baseStatement = new string[] { "base.Initialize();" };

            private readonly static List<string> _baseLines = new List<string>(100);

            public static void WriteClass(string className, string savePath, bool isWriteChild, UIConfig uIConfig)
            {
                File.WriteAllLines(savePath, _baseLines);
                int count = string.IsNullOrEmpty(uIConfig.ClassNamespaces) ? 0 : 1;

                if (isWriteChild)
                {
                    using (FileStream fileStream = File.Create(savePath.Replace("Base", string.Empty)))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8, 1024, true))
                        {
                            CSharpWriter cSharpWriter = new CSharpWriter(streamWriter);
                            cSharpWriter.WriteNamespace(uIConfig.Namespaces.ToArray());
                            cSharpWriter.BeginWriteClass(count, uIConfig.ClassNamespaces, null, className.Replace("Base", string.Empty), parents: new string[] { className + "Base" });

                            cSharpWriter.WriteMethod(count + 1, "Initialize", "void", CSharpWriter.Modifiers.Override, null, _baseStatement, access: "protected");

                            cSharpWriter.WriteMethod(count + 1, "AddEvent", "void", CSharpWriter.Modifiers.Override, null, null, access: "protected");
                            cSharpWriter.EndWriteClass(count, count > 0);
                        }
                    }
                }

                AssetDatabase.Refresh();
            }

            public static List<string> GetClassStrings(string className, IList<Selectable> allBehaviors, UIConfig uIConfig)
            {
                int count = string.IsNullOrEmpty(uIConfig.ClassNamespaces) ? 0 : 1;

                using (MemoryStream memoryStream = new MemoryStream(1024))
                {
                    using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
                    {
                        CSharpWriter cSharpWriter = new CSharpWriter(streamWriter);
                        cSharpWriter.WriteNamespace(uIConfig.Namespaces.ToArray());
                        cSharpWriter.BeginWriteClass(count, uIConfig.ClassNamespaces, CSharpWriter.Modifiers.Abstract, className + "Base", parents: uIConfig.Parents);

                        string[] statements = CreateFields(cSharpWriter, count + 1, allBehaviors);
                        cSharpWriter.WriteMethod(count + 1, "Initialize", "void", CSharpWriter.Modifiers.Override, null, statements, access: "protected");

                        cSharpWriter.EndWriteClass(count, count > 0);
                    }

                    memoryStream.Position = 0;

                    _baseLines.Clear();
                    using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        string line;
                        while((line = streamReader.ReadLine()) != null)
                        {
                            _baseLines.Add(line);
                        }
                    }

                    return _baseLines;
                }
            }

            private static string[] CreateFields(CSharpWriter cSharpWriter, int count, IList<Selectable> selectables)
            {
                string[] statements = new string[selectables.Count + 1];
                HashSet<string> fields = new HashSet<string>();

                string fieldName;
                string typeName;

                for (int i = 0; i < selectables.Count; i++)
                {
                    fieldName = '_' + selectables[i].name;
                    if (!fields.Contains(fieldName))
                    {
                        typeName = selectables[i].GetType().Name;
                        cSharpWriter.WriteAttribute(count, nameof(SerializeField));
                        cSharpWriter.WriteField(count, fieldName, typeName, "protected");

                        statements[i] = UIMethodWriter.GetMethodStatement(fieldName, typeName);
                        fields.Add(fieldName);
                    }
                }

                statements[^1] = "AddEvent();";
                return statements;
            }
        }
    }
}