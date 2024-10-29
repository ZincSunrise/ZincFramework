using UnityEngine.Events;


namespace ZincFramework
{
    namespace UI
    {
        public interface IPanelInfo
        {
            IViewBase ViewBase { get; }

            //这个属性仅显示是否要被移除
            bool WillDestroy { get; }
        }

        public class PanelInfo<T> : IPanelInfo where T : BasePanel
        {
            public IViewBase ViewBase => Panel;

            public T Panel { get; set; }

            public bool WillDestroy { get; set; }

            public UnityAction<T> PanelAction { get; set; }

            public PanelInfo(UnityAction<T> action)
            {
                PanelAction += action;
            }

            public PanelInfo()
            {

            }
        }
    }
}
