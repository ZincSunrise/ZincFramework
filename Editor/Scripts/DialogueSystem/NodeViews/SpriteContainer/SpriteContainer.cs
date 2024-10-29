using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using ZincFramework.Events;
using ZincFramework.DialogueSystem.TextData;
using ZincFramework.LoadServices.Addressable;



namespace ZincFramework.DialogueSystem.GraphView
{
    public class SpriteContainer : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<SpriteContainer> { }

        public event ZincAction<SpriteContainer> OnValueChanged;

        public Image Image { get; }

        public Toggle FocusToggle { get; }

        public Vector2Field PositionField { get; }


        private readonly ObjectField _objectField;

        public VisibleState VisableState { get; private set; }

        public int Index { get; private set; }

        public SpriteContainer()
        {
            _objectField = new ObjectField();

            _objectField.objectType = typeof(Sprite);
            _objectField.style.width = 125;

            Image = new Image();
            Image.style.width = 125;
            Image.style.height = 125;

            Image.scaleMode = ScaleMode.ScaleAndCrop;
            Image.uv = new Rect(0.25f, 0.5f, 0.6f, 0.6f);  // UV 坐标设置为只显示上半部分
            _objectField.RegisterValueChangedCallback(OnValueChange);

            this.Add(_objectField);
            this.Add(Image);

            PositionField = new Vector2Field();
            FocusToggle = new Toggle();

            PositionField.style.maxWidth = 125;

            FocusToggle.style.overflow = Overflow.Visible;
            FocusToggle.style.flexDirection = FlexDirection.RowReverse;
            FocusToggle.style.maxWidth = 125;
            FocusToggle.label = "是否聚焦";
            FocusToggle.style.translate = new Translate(25, 0);
            this.Add(FocusToggle);
            this.Add(PositionField);
        }

        public SpriteContainer(ZincAction<SpriteContainer> onValueChanged) : this()
        {
            this.OnValueChanged = onValueChanged;
        }

        public void ChangeSprite(Sprite sprite)
        {
            if (sprite != Image.sprite)
            {
                _objectField.value = sprite;
                Image.sprite = sprite;

                TextNodeUtility.GetSpriteId(sprite.name, out var visableId, out var differential);
                VisableState = new VisibleState(visableId, differential, VisableState.IsFocus, VisableState.Position);
                OnValueChanged?.Invoke(this);
            }
        }


        public async void UpdateSprite(int index, VisibleState visibleState)
        {
            Index = index;
            VisableState = visibleState;

            FocusToggle.UnregisterValueChangedCallback(OnToggleChanged);
            PositionField.UnregisterValueChangedCallback(OnPositionChanged);

            string assetName = TextNodeUtility.GetSpriteName(visibleState);

            FocusToggle.value = visibleState.IsFocus;
            if (visibleState.IsFocus)
            {
                Image.style.backgroundColor = ColorUtility.TryParseHtmlString("#E3E7D3", out var color) ? color : Color.white;
            }
            else
            {
                Image.style.backgroundColor = new Color(1, 1, 1, 0);
            }

            PositionField.value = visibleState.Position;

            if (assetName != string.Empty)
            {
                ChangeSprite(await AddressablesManager.Instance.LoadAssetAsync<Sprite>(assetName));
            }

            FocusToggle.RegisterValueChangedCallback(OnToggleChanged);
            PositionField.RegisterValueChangedCallback(OnPositionChanged);
        }

        private void OnValueChange(ChangeEvent<Object> changeEvent)
        {
            if (changeEvent.newValue != changeEvent.previousValue)
            {
                ChangeSprite(changeEvent.newValue as Sprite);
            }
        }

        private void OnToggleChanged(ChangeEvent<bool> changeEvent)
        {
            if (changeEvent.newValue != changeEvent.previousValue)
            {
                VisableState = new VisibleState(VisableState.VisableId, VisableState.Differential, changeEvent.newValue, VisableState.Position);
                OnValueChanged?.Invoke(this);
                if (changeEvent.newValue)
                {
                    Image.style.backgroundColor = ColorUtility.TryParseHtmlString("#E3E7D3", out var color) ? color : Color.white;
                }
                else
                {
                    Image.style.backgroundColor = new Color(1, 1, 1, 0);
                }
            }
        }

        private void OnPositionChanged(ChangeEvent<Vector2> changeEvent)
        {
            if (changeEvent.newValue != changeEvent.previousValue)
            {
                VisableState = new VisibleState(VisableState.VisableId, VisableState.Differential, VisableState.IsFocus, changeEvent.newValue);
                OnValueChanged?.Invoke(this);
            }
        }
    }
}