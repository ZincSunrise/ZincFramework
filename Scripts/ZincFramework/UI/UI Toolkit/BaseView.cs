using UnityEngine.UIElements;
using ZincFramework.Events;


namespace ZincFramework
{
    namespace UI
    {
        namespace UIElements
        {
            public abstract class BaseView : IViewBase
            {
                public VisualElement RootElement { get; set; }

                public ZincEvent OnShow { get; } = new ZincEvent();
                public ZincEvent OnHide { get; } = new ZincEvent();

                public abstract void Initialize();

                public virtual void ShowMe()
                {
                    OnHide.Invoke();
                }

                public virtual void HideMe()
                {
                    OnHide.Invoke();
                }
            }
        }
    }
}
