using UnityEngine.UIElements;
using ZincFramework.DialogueSystem.TextData;



namespace ZincFramework.DialogueSystem.GraphView
{
    public class ConditionField : VisualElement
    {
        public IntegerField WeightField { get; private set; }

        public DropdownField DropdownField { get; private set; }

        public void UpdateItem(int index, TextExpression expressionInfo)
        {
            WeightField.label = "����" + (index + 1);
            DropdownField.value = expressionInfo.Expression;
        }

        public ConditionField() 
        {
            WeightField = new IntegerField() { label = "Ȩ��"};
            DropdownField = new DropdownField();
            DropdownField.choices = new System.Collections.Generic.List<string>() 
            {
                "1","2","3","4"
            };

            Add(WeightField);
            Add(DropdownField);
        }
    }
}


