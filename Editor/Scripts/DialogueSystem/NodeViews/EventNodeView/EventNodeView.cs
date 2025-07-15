using UnityEngine.UIElements;

namespace ZincFramework.DialogueSystem.GraphView
{
    public class EventNodeView : TextNodeView
    {
        private IntegerField _intField;

        private EventNode EventNode => TextNode as EventNode;

        public EventNodeView(BaseTextNode baseTextNode) : base(baseTextNode)
        {
            _intField = new IntegerField();
            _intField.label = "事件ID";

            _intField.RegisterValueChangedCallback<int>(OnTextValueChanged);
            this.Q("unity-content").Add(_intField);
        }

        private void OnTextValueChanged(ChangeEvent<int> eventBase)
        {
            if(EventNode.EventIds.Count > 0)
            {
                EventNode.EventIds[0] = eventBase.newValue;
            }
            else
            {
                EventNode.EventIds.Add(eventBase.newValue);
            }
        }
    }
}