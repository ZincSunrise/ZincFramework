using System.Collections.Generic;
using UnityEditor;
using ZincFramework.Events;



namespace ZincFramework
{
    namespace UI
    {
        namespace EditorUI
        {
            public class EditorDropdown : IEditorUI
            {
                public string Title { get; }

                public List<string> DisPlayedLabel { get; set; }

                public ZincEvent<int> OnSelectionChanaged { get; } = new ZincEvent<int>();

                public int CurrentIndex { get; set; }

                private int _preIndex = 0;

                private string[] _disPlayedLabel;


                public void AddListener(ZincAction<int> action)
                {
                    OnSelectionChanaged.AddListener(action);
                }

                public void RemoveListener(ZincAction<int> action)
                {
                    OnSelectionChanaged.RemoveListener(action);
                }

                public EditorDropdown(string title, ZincAction<int> action, params string[] disPlayedLabel)
                {
                    Title = title;
                    DisPlayedLabel = new List<string>(disPlayedLabel);
                    OnSelectionChanaged.AddListener(action);
                }

                public EditorDropdown(string title, params string[] disPlayedLabel)
                {
                    Title = title;
                    DisPlayedLabel = new List<string>(disPlayedLabel);
                }

                public void OnGUI()
                {
                    if(_disPlayedLabel == null || _disPlayedLabel.Length != DisPlayedLabel.Count)
                    {
                        _disPlayedLabel = DisPlayedLabel.ToArray();
                    }
                    CurrentIndex = EditorGUILayout.Popup(Title, CurrentIndex, _disPlayedLabel);

                    if(CurrentIndex != _preIndex)
                    {
                        OnSelectionChanaged?.Invoke(CurrentIndex);
                        _preIndex = CurrentIndex;
                    }
                }
            }
        }
    }
}
