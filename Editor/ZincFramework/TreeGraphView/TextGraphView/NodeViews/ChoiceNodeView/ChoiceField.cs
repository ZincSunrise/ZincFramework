using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Events;
using static ZincFramework.TreeGraphView.TextTree.ChoiceNode;

namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class ChoiceField : VisualElement
            {
                public new class UxmlFactory : UxmlFactory<ChoiceField> { }

                public TextField TextField { get; }

                public Label NameLabel { get; }


                public event ZincAction<string> ChangeEvent;

                public ChoiceField()
                {
                    NameLabel = new Label("Ê¾Àý");
                    TextField = new TextField();

                    Add(NameLabel);
                    Add(TextField);
                    TextField.RegisterValueChangedCallback(OnValueChange);
                }

                public void RegistValueChange(ChoiceInfo choiceInfo)
                {
                    if(ChangeEvent == null)
                    {
                        ChangeEvent += x => TextNodeUtility.SetChoiceText(choiceInfo, x);
                    }              
                }

                private void OnValueChange(ChangeEvent<string> changeEvent)
                {
                    ChangeEvent?.Invoke(changeEvent.newValue);
                }
            }
        }
    }
}
