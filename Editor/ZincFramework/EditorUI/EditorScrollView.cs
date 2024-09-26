using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class EditorScrollView : IEditorUI
            {
                public string Title { get;}

                private readonly List<IEditorUI> _editorUIs = new List<IEditorUI>();

                private Vector2 _scrollPos;


                public void OnGUI()
                {
                    _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                    for(int i = 0;i < _editorUIs.Count; i++)
                    {
                        _editorUIs[i].OnGUI();
                    }

                    EditorGUILayout.EndScrollView();
                }

                public void Add(IEditorUI editor)
                {
                    _editorUIs.Add(editor);
                }

                public bool Remove(IEditorUI editor)
                {
                    return _editorUIs.Remove(editor);
                }

                public bool Contains(IEditorUI editor) 
                {
                    return _editorUIs.Contains(editor);
                }
            }
        }
    }
}