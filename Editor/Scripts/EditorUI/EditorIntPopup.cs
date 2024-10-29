using UnityEditor;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class EditorIntPopup : IEditorUI
            {
                public string Title { get; }

                public ZincEvent<int> OnSelectionChanged { get; } = new ZincEvent<int>();


                private int _nowIndex;


                private readonly string[] _displayedOptions;


                private readonly int[] _optionValues;

                public EditorIntPopup(string title, int value, string[] displayedOptions, int[] optionValues) 
                {
                    Title = title;
                    _nowIndex = value;
                    _displayedOptions = displayedOptions;
                    _optionValues = optionValues;
                }

                public EditorIntPopup(string title, int value, string[] displayedOptions, int[] optionValues, ZincAction<int> callback) : this(title, value, displayedOptions, optionValues)
                {
                    OnSelectionChanged.AddListener(callback);
                }

                public void OnGUI()
                {
                    int preIndex = _nowIndex;
                    _nowIndex = EditorGUILayout.IntPopup(Title, _nowIndex, _displayedOptions, _optionValues);

                    if(preIndex != _nowIndex)
                    {
                        OnSelectionChanged.Invoke(_nowIndex);
                    }
                }
            }
        }
    }
}
