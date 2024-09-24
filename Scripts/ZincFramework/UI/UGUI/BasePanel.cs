using UnityEngine;
using UnityEngine.Events;


namespace ZincFramework
{
    namespace UI
    {
        public abstract class BasePanel : MonoBehaviour, IViewBase
        {
            public bool IsHiding { get; private set; }

            [SerializeField]
            private float _showTime = 0.2f;

            [SerializeField]
            private float _hideTime = 0.2f;

            protected CanvasGroup _canvasGroup;

            private Coroutine _nowCoroutine;

            protected virtual void Awake()
            {
                _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
                Initialize();
            }

            public abstract void Initialize();

            public virtual void ShowMe()
            {
                if(_showTime <= 0)
                {
                    _canvasGroup.alpha = 1;
                }
                else
                {
                    if (IsHiding && _nowCoroutine != null)
                    {
                        StopCoroutine(_nowCoroutine);
                    }

                    _nowCoroutine = StartCoroutine(LerpUtility.LerpValue(_canvasGroup.alpha, 1, _showTime, x => _canvasGroup.alpha = x));
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
                    if (!IsHiding && _nowCoroutine != null)
                    {
                        StopCoroutine(_nowCoroutine);
                    }
                    _nowCoroutine = StartCoroutine(LerpUtility.LerpValue(_canvasGroup.alpha, 0, _hideTime, (x) => _canvasGroup.alpha = x, hideAction));
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
}

