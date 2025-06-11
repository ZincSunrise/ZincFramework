using UnityEngine.UIElements;


namespace ZincFramework.UI.UIElements
{
    public abstract class ViewBase : VisualElement, IViewBase
    {
        public virtual int Layer => 0;

        public ViewBase() : base()
        {
            this.style.flexGrow = 1;
        }

        /// <summary>
        /// ��ʼ���ҵ��Լ�����ķ���
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// �����Լ��ķ���
        /// </summary>
        public virtual void HideMe()
        {
            this.style.display = DisplayStyle.None;
            this.style.visibility = Visibility.Hidden;
        }

        /// <summary>
        /// ��ʾ�Լ��ķ���
        /// </summary>
        public virtual void ShowMe()
        {
            this.style.display = DisplayStyle.Flex;
            this.style.visibility = Visibility.Visible;
        }
    }
}
