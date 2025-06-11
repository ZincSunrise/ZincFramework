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
        /// 初始化找到自己组件的方法
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 隐藏自己的方法
        /// </summary>
        public virtual void HideMe()
        {
            this.style.display = DisplayStyle.None;
            this.style.visibility = Visibility.Hidden;
        }

        /// <summary>
        /// 显示自己的方法
        /// </summary>
        public virtual void ShowMe()
        {
            this.style.display = DisplayStyle.Flex;
            this.style.visibility = Visibility.Visible;
        }
    }
}
