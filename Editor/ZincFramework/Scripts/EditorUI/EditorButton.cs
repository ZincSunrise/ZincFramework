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

                public event ZincAction OnClick;

                public EditorButton(string title)
                {
                    Title = title;
                }

                public EditorButton(string title, ZincAction onclick)
                {
                    Title = title;
                    OnClick += onclick;
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
