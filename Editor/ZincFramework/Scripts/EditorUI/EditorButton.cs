using UnityEngine;
using ZincFramework.Events;




namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class EditorButton : IEditorUI
            {
                public string Title { get; set; }

                public float Width { get; set; }

                public event ZincAction OnClick;

                public EditorButton(string title)
                {
                    Title = title;
                }

                public EditorButton(string title, ZincAction onclick) : this(title)
                {
                    OnClick += onclick;
                }

                public EditorButton(string title, float width, ZincAction onclick) : this(title, onclick)
                {
                    Width = width;
                }

                public void OnGUI()
                {
                    if (GUILayout.Button(Title))
                    {
                        OnClick?.Invoke();
                    }
                }
            }
        }
    }
}
