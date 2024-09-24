using UnityEditor;
using UnityEngine;
using ZincFramework.Events;



namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public abstract class EditorField<T> : IEditorUI
            {
                public ZincEvent<T> OnValueChanged { get; } = new ZincEvent<T>();

                public T Value { get; set; }

                public string Title { get; }


                public EditorField(T value, string title)
                {
                    Value = value;
                    Title = title;
                }

                public abstract void OnGUI();
            }
        }
    }
}