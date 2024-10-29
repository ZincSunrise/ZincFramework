using UnityEngine;
using UnityEngine.EventSystems;
using ZincFramework.UI.Complex;
using ZincFramework.UI.Switchable;


namespace ZincFramework.UI
{
    public class FoldOut : ComplexUI, IPointerClickHandler, ISwitchable
    {
        public Transform Content { get; private set; }

        [SerializeField]
        private UISwitchGroup _switchGroup;

        private void Awake()
        {
            Content = transform.Find(nameof(Content));
            SwitchOff();

            if (_switchGroup != null) 
            {
                _switchGroup.AddSwitch(this);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Content.gameObject.SetActive(!Content.gameObject.activeSelf);

            if (_switchGroup != null) 
            {
                _switchGroup.HideElements(this);
            }
        }

        public void SwitchOn()
        {
            Content.gameObject.SetActive(true);
        }

        public void SwitchOff()
        {
            Content.gameObject.SetActive(false);
        }
    }
}
