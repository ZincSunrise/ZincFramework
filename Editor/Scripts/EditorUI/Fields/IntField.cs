using UnityEditor;
using ZincFramework.Events;




namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class IntField : EditorField<int>
            {

                private int _preValue;

                public IntField(int value, string title) : base(value, title)
                {
                    _preValue = value;
                }

                public override void OnGUI()
                {
                    if(Title != null)
                    {
                        Value = EditorGUILayout.IntField(Title, Value);
                    }
                    else
                    {
                        Value = EditorGUILayout.IntField(Value);
                    }           
                    
                    if(Value != _preValue)
                    {
                        OnValueChanged?.Invoke(Value);
                        _preValue = Value;
                    }
                }
            }
        }
    }
}
