using UnityEngine;
using UnityEngine.UI;
using ZincFramework.UI.Complex;
using ZincFramework.UI.Switchable;


namespace ZincFramework.UI
{
    public class DropTipsButton : ComplexUI, ISwitchable
    {
        public Button ButtonTips { get; private set; }

        public Transform TextContainer { get; private set; }

        public Text TextTips {  get; private set; }

        [SerializeField]
        public UISwitchGroup _showingGroup;

        private void Awake()
        {
            ButtonTips = this.GetComponent<Button>();
            TextContainer = this.transform.Find(nameof(TextContainer));
            TextTips = TextContainer.transform.Find(nameof(TextTips)).GetComponent<Text>();

            SwitchOff();
            ButtonTips.onClick.AddListener(SwitchTips);

            if(_showingGroup != null)
            {
                _showingGroup.AddSwitch(this);
            }
        }

        private void SwitchTips()
        {
            TextContainer.gameObject.SetActive(!TextContainer.gameObject.activeSelf);
            if(_showingGroup != null && TextContainer.gameObject.activeSelf)
            {
                _showingGroup.HideElements(this);
            }
        }

        public void SwitchOn()
        {
            TextContainer.gameObject.SetActive(true);
        }

        public void SwitchOff()
        {
            TextContainer.gameObject.SetActive(false);
        }
    }
}