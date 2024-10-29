using System;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ZincFramework.DialogueSystem.TextData;



namespace ZincFramework.DialogueSystem.GraphView
{
    public enum EffectType
    {
        None = 0,
        Animation = 2,
        PlayMusic = 4,
        StopMusic = 8,
        PlaySound = 16,
        ChangeBackGround = 32,
    }

    public enum DropType
    {
        None = 0,
        User = 2,
        NoUser = 4,
    }

    public class EffectField : VisualElement
    {
        private readonly DropdownField _dropdownField;

        private readonly EnumField _enumField;

        private readonly ObjectField _objectField;

        private readonly TextField _expressionField;

        private readonly VisualElement _container;

        public EffectField()
        {
            _enumField = new EnumField(EffectType.None);
            _dropdownField = new DropdownField();
            _objectField = new ObjectField();
            _expressionField = new TextField();
            _container = new VisualElement();
            _container.style.flexDirection = FlexDirection.Row;

            _enumField.RegisterValueChangedCallback(OnValueChanged);

            Add(_enumField);
            Add(_container);
        }

        public void Update(TextExpression expressionInfo, in VisibleState[] visibleStates)
        {
            _dropdownField.choices.Clear();

            for (int i = 0; i < visibleStates.Length; i++)
            {
                ref var visibleState = ref visibleStates[i];
                _dropdownField.choices.Add(TextNodeUtility.GetCharacterNameFormId(visibleState.VisableId));
            }
        }

        private void OnValueChanged(ChangeEvent<Enum> changeEvent)
        {
            if(changeEvent.newValue != changeEvent.previousValue)
            {
                switch (changeEvent.newValue) 
                {
                    case EffectType.None:
                        SetStyle(DropType.None);
                        break;
                    case EffectType.Animation:
                        _container.Add(_dropdownField);
                        _container.Add(_objectField);
                        _dropdownField.focusable = true;
                        _objectField.objectType = typeof(AnimationClip);
                        SetStyle(DropType.User);
                        break;
                    case EffectType.PlayMusic:
                        SetStyle(DropType.NoUser);
                        Add(_objectField);
                        _objectField.objectType = typeof(AudioClip);
                        break;
                    case EffectType.StopMusic:
                        SetStyle(DropType.None);
                        break;
                    case EffectType.ChangeBackGround:
                        SetStyle(DropType.NoUser);
                        Add(_objectField);
                        _objectField.objectType = typeof(Sprite);
                        break;
                }
            }
        }

        private void CheckRemove(VisualElement visualElement)
        {
            if(visualElement.parent == this)
            {
                Remove(visualElement);
            }
            else if (visualElement.parent == _container)
            {
                _container.Remove(visualElement);
            }
        }


        private void SetStyle(DropType dropType)
        {
            switch (dropType) 
            {
                case DropType.None:
                    CheckRemove(_objectField);
                    CheckRemove(_expressionField);
                    CheckRemove(_dropdownField);
                    _objectField.style.flexBasis = _dropdownField.style.flexBasis = 1f;
                    break;
                case DropType.NoUser:
                    _container.style.flexDirection = FlexDirection.Column;
                    _objectField.style.flexBasis = _dropdownField.style.flexBasis = 1f;
                    break;
                case DropType.User:
                    _container.style.flexDirection = FlexDirection.Row;
                    _objectField.style.flexBasis = _dropdownField.style.flexBasis = 0.5f;
                    _objectField.style.flexGrow = _dropdownField.style.flexGrow = 1;
                    break;
            }
        }
    }
}