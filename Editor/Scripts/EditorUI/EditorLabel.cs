using UnityEditor;
using UnityEngine;


namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class EditorLabel : IEditorUI
            {
                public string Title { get; set; }

                public EditorLabel()
                {

                }

                public EditorLabel(string label) 
                {
                    Title = label;
                }

                public void OnGUI()
                {
                    GUILayout.Label(Title);
                }
            }
        }
    }
}