using UnityEditor;




namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class StringField : EditorField<string>
            {
                private string _preValue;

                public StringField(string value, string title) : base(value, title)
                {
                    _preValue = value;
                }

                public override void OnGUI()
                {
                    if (Title != null)
                    {
                        Value = EditorGUILayout.TextField(Title, Value);
                    }
                    else
                    {
                        Value = EditorGUILayout.TextField(Value);
                    }

                    if (Value != _preValue)
                    {
                        OnValueChanged?.Invoke(Value);
                        _preValue = Value;
                    }
                }
            }
        }
    }
}
