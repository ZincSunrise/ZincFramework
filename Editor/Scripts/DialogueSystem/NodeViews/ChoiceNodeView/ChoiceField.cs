using UnityEngine.UIElements;
using ZincFramework.Events;
using ZincFramework.DialogueSystem.TextData;


namespace ZincFramework.DialogueSystem.GraphView
{
    public class ChoiceField : VisualElement
    {
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
            if (ChangeEvent == null)
            {
                ChangeEvent += x => TextNodeUtility.SetChoiceText(choiceInfo, x);
            }
        }

        private void OnValueChange(ChangeEvent<string> changeEvent)
        {
            if (changeEvent.newValue != changeEvent.previousValue)
            {
                ChangeEvent?.Invoke(changeEvent.newValue);
            }
        }
    }
}
