using UnityEditor;
using UnityEngine;



namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class ObjectField<T> : EditorField<T> where T : Object
            {
                private readonly System.Type _type;
                private readonly bool _canChooseScene;

                public ObjectField(T obj, string title, bool canChooseScene) : base(obj, title)
                {
                    _type = typeof(T);
                    _canChooseScene = canChooseScene;
                }

                public override void OnGUI()
                {
                    Value = EditorGUILayout.ObjectField(Title, Value, _type, _canChooseScene) as T;
                }
            }
        }
    }
}