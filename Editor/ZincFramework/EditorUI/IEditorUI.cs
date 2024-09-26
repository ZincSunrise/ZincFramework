using UnityEngine;

namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public interface IEditorUI
            {
                public string Title { get; }

                public abstract void OnGUI();
            }
        }
    }
}