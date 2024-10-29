using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using ZincFramework.ScriptWriter;
using ZincFramework.UI.Collections;
using ZincFramework.UI.Complex;




namespace ZincFramework.UI.ScriptWriter
{
    public static class UIClassWriter
    {
        public readonly static string[] _baseStatement = new string[] { "base.Initialize();" };

        private readonly static List<string> _baseLines = new List<string>(100);

        public static void WriteClass(Selectable[] selectables, string className, string savePath, UIConfig uIConfig)
        {
            //先写父类
            CSharpWriter cSharpWriter = CSharpWriter.RentWriter();
            File.WriteAllLines(savePath, GetClassStrings(cSharpWriter, className, selectables, uIConfig));
            int count = string.IsNullOrEmpty(uIConfig.ClassNamespaces) ? 0 : 1;

            string childName = className.Replace("Base", string.Empty);

            //检查子类是否存在
            if (typeof(ComplexUI).Assembly.GetType(uIConfig.ClassNamespaces + $".{childName}") == null)
            {
                using (FileStream fileStream = File.Create(savePath.Replace("Base", string.Empty)))
                {
                    cSharpWriter = CSharpWriter.RentWriter();
                    cSharpWriter.WriteNamespace(uIConfig.Namespaces.ToArray());
                    cSharpWriter.BeginWriteClass(count, uIConfig.ClassNamespaces, null, className.Replace("Base", string.Empty), parents: new string[] { className + "Base" });

                    cSharpWriter.WriteMethod(count + 1, "Initialize", "void", CSharpWriter.Modifiers.Override, null, _baseStatement);

                    cSharpWriter.EndWriteClass(count, count > 0);
                    cSharpWriter.WriteAndReturn(fileStream);
                }
            }

            AssetDatabase.Refresh();
        }

        public static string[] GetClassStrings(CSharpWriter cSharpWriter, string className, IList<Selectable> allBehaviors, UIConfig uIConfig)
        {
            int count = string.IsNullOrEmpty(uIConfig.ClassNamespaces) ? 0 : 1;
            cSharpWriter.WriteNamespace(uIConfig.Namespaces.ToArray());
            cSharpWriter.BeginWriteClass(count, uIConfig.ClassNamespaces, CSharpWriter.Modifiers.Abstract, className + "Base", parents: uIConfig.Parents);

            string[] statements = CreateFields(cSharpWriter, className, count + 1, allBehaviors);
            cSharpWriter.WriteMethod(count + 1, "Initialize", "void", CSharpWriter.Modifiers.Override, null, statements);

            cSharpWriter.EndWriteClass(count, count > 0);
            string[] readStr = cSharpWriter.ReadAllLines();

            //还回去
            cSharpWriter.Return();
            return readStr;
        }

        private static string[] CreateFields(CSharpWriter cSharpWriter, string className, int count, IList<Selectable> selectables)
        {
            string[] statements = new string[selectables.Count + 1];
            HashSet<string> fields = new HashSet<string>();
            UICollector collector = new UICollector();

            for (int i = 0; i < selectables.Count; i++)
            {
                if (selectables[i] is Scrollbar)
                {
                    continue;
                }

                if (!fields.Contains(selectables[i].name))
                {
                    UIWriteInfo uIWriteInfo = collector.GetUIWriteInfos(className, selectables[i]);
                    cSharpWriter.WriteAutoProperty(count, uIWriteInfo.Type, uIWriteInfo.Name, CSharpWriter.Accessors.Public, CSharpWriter.Accessors.Protected);
                    cSharpWriter.WriteLine();

                    statements[i] = UIMethodWriter.GetMethodStatement(uIWriteInfo);
                    fields.Add(selectables[i].name);
                }
            }

            return statements;
        }
    }
}