using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using ZincFramework.Events;



namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class EditorCollection
            {
                public List<IEditorUI> EditorUIs { get; } = new List<IEditorUI>();

                public EditorCollection(EditorWindow editorWindow)
                {
                    FieldInfo[] fieldInfos = editorWindow.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    List<EditorScrollView> scrollViews = new List<EditorScrollView>(); 

                    foreach (FieldInfo fieldInfo in fieldInfos) 
                    {
                        object value = fieldInfo.GetValue(editorWindow);
                        if (value is IEditorUI editorUI)
                        {
                            if (value is EditorScrollView scrollView)
                            {
                                scrollViews.Add(scrollView);
                            }
                            EditorUIs.Add(editorUI);
                        }
                    }

                    for (int i = 0; i < scrollViews.Count; i++) 
                    {
                        EditorUIs.RemoveAll((x) =>
                        {
                            return scrollViews[i].Contains(x);
                        });
                    }
                }

                public EditorCollection()
                {

                }

                public void Foreach(ZincAction<IEditorUI> callback)
                {
                    for (int i = 0; i < EditorUIs.Count; i++)
                    {
                        callback.Invoke(EditorUIs[i]);
                    }
                }
            }
        }
    }
}