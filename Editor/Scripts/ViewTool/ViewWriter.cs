using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;
using ZincFramework.ScriptWriter;
using ZincFramework.UI.Collections;
using ZincFramework.UI.ScriptWriter;
using ZincFramework.UI.UIElements;


namespace ZincFramework.UI.ViewWriter
{
    public static class ViewWriter
    {
        public static void WriteViewScript(VisualElement visualElement, string className, string savePath, UIConfig uIConfig)
        {
            //先写父类
            CSharpWriter cSharpWriter = CSharpWriter.RentWriter();
            File.WriteAllLines(savePath, GetClassStrings(cSharpWriter, className, visualElement, uIConfig));
            int count = string.IsNullOrEmpty(uIConfig.ClassNamespaces) ? 0 : 1;

            string childName = className.Replace("Base", string.Empty);

            //检查子类是否存在
            if (typeof(ViewBase).Assembly.GetType($"{uIConfig.ClassNamespaces}.{childName}") == null)
            {
                using (FileStream fileStream = File.Create(savePath.Replace("Base", string.Empty)))
                {
                    cSharpWriter = CSharpWriter.RentWriter();
                    cSharpWriter.WriteNamespace(uIConfig.Namespaces.ToArray());
                    cSharpWriter.BeginWriteClass(count, uIConfig.ClassNamespaces, null, childName, parents: new string[] { className + "Base" });
                    cSharpWriter.WriteLine(count + 1, $"public new class UxmlFactory : UxmlFactory<{childName}> {{ }}");
                    cSharpWriter.WriteMethod(count + 1, "Initialize", "void", CSharpWriter.Modifiers.Override, null, UIClassWriter.BaseStatement);

                    cSharpWriter.EndWriteClass(count, count > 0);
                    cSharpWriter.WriteAndReturn(fileStream);
                }
            }

            AssetDatabase.Refresh();
        }

        public static string[] GetClassStrings(CSharpWriter cSharpWriter, string className, VisualElement visualElement, UIConfig uIConfig)
        {
            int count = string.IsNullOrEmpty(uIConfig.ClassNamespaces) ? 0 : 1;
            cSharpWriter.WriteNamespace(uIConfig.Namespaces.ToArray());
            cSharpWriter.BeginWriteClass(count, uIConfig.ClassNamespaces, CSharpWriter.Modifiers.Abstract, className + "Base", parents: uIConfig.Parents);

            
            string[] statements = CreateFields(cSharpWriter, count + 1, visualElement.Query<BindableElement>().ToList());
            cSharpWriter.WriteMethod(count + 1, "Initialize", "void", CSharpWriter.Modifiers.Override, null, statements);

            cSharpWriter.EndWriteClass(count, count > 0);
            string[] readStr = cSharpWriter.ReadAllLines();

            //还回去
            cSharpWriter.Return();
            return readStr;
        }

        private static string[] CreateFields(CSharpWriter cSharpWriter, int count, IList<BindableElement> bindableElements)
        {
            string[] statements = new string[bindableElements.Count + 1];
            HashSet<string> fields = new HashSet<string>();

            for (int i = 0; i < bindableElements.Count; i++)
            {
                if (bindableElements[i] is TemplateContainer || bindableElements[i].GetFirstAncestorOfType<ScrollView>() != null)
                {
                    continue;
                }

                if (!fields.Contains(bindableElements[i].name))
                {
                    UIWriteInfo uIWriteInfo = GetUIWriteInfo(bindableElements[i]);
                    cSharpWriter.WriteAutoProperty(count, uIWriteInfo.Type, uIWriteInfo.Name, CSharpWriter.Accessors.Public, CSharpWriter.Accessors.Protected);
                    cSharpWriter.WriteLine();

                    statements[i] = UIMethodWriter.GetViewStatement(uIWriteInfo);
                    fields.Add(bindableElements[i].name);
                }
            }

            return statements;
        }

        private static UIWriteInfo GetUIWriteInfo(VisualElement visualElement)
        {
            return new UIWriteInfo(visualElement.name, visualElement.name, visualElement.GetType().Name);
        }
    }
}