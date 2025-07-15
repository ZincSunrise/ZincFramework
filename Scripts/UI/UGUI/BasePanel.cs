using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using ZincFramework.MVC.Interfaces;


namespace ZincFramework.UI
{
    public abstract class BasePanel : MonoBehaviour, IViewBase
    {
        public bool IsHiding { get; private set; }

        [SerializeField]
        private float _showTime = 0.2f;

        [SerializeField]
        private float _hideTime = 0.2f;

        protected CanvasGroup _canvasGroup;

        protected CancellationTokenSource _cancellationTokenSource;

        protected virtual void Awake()
        {
            _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
            Initialize();
        }

        public abstract void Initialize();

        public virtual void ShowMe()
        {
            if (_showTime <= 0)
            {
                _canvasGroup.alpha = 1;
            }
            else
            {
                if (IsHiding && _cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }

                _cancellationTokenSource = new CancellationTokenSource();
                LerpUtility.LerpValue(_canvasGroup.alpha, 1, _showTime, true, x => _canvasGroup.alpha = x, null, _cancellationTokenSource.Token).Forget();
            }

            IsHiding = false;
        }

        public virtual void HideMe(UnityAction hideAction)
        {
            if (_hideTime <= 0)
            {
                _canvasGroup.alpha = 0;
            }
            else
            {
                if (!IsHiding && _cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }

                _cancellationTokenSource = new CancellationTokenSource();
                LerpUtility.LerpValue(_canvasGroup.alpha, 0, _showTime, true, x => _canvasGroup.alpha = x, null, _cancellationTokenSource.Token).Forget();
            }

            HideMe();
        }

        public void HideMe()
        {
            IsHiding = true;
        }

        protected virtual void AddEvent()
        {

        }
    }
}

