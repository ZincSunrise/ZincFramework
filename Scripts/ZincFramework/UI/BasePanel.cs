using UnityEngine;
using UnityEngine.Events;


namespace ZincFramework
{
    namespace UI
    {
        public abstract class BasePanel : MonoBehaviour
        {
            public float HideSpeed => _hideSpeed;

            [SerializeField]
            private float _hideSpeed = 10;

            private CanvasGroup _canvasGroup;
            private UnityAction _hideAction;
            private bool _isHiding = false;

            protected virtual void Awake()
            {
                _canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
            }

            protected virtual void Start()
            {
                Initialize();
            }

            protected abstract void Initialize();

            protected virtual void Update()
            {
                if (_isHiding)
                {
                    _canvasGroup.alpha -= HideSpeed * Time.unscaledDeltaTime;
                    if (_canvasGroup.alpha <= 0)
                    {
                        _canvasGroup.alpha = 0;
                        _hideAction.Invoke();
                        _hideAction = null;
                    }
                }
                else if (!_isHiding && _canvasGroup.alpha < 1)
                {
                    _canvasGroup.alpha += HideSpeed * Time.unscaledDeltaTime;
                    if (_canvasGroup.alpha > 1)
                    {
                        _canvasGroup.alpha = 1;
                    }
                }
            }

            public virtual void ShowMe()
            {
                _canvasGroup.alpha = 0;
                _isHiding = false;
            }

            public virtual void HideMe(UnityAction hideAction)
            {
                _canvasGroup.alpha = 1;
                _isHiding = true;
                _hideAction = hideAction;
            }

            public static explicit operator bool(BasePanel basePanel)
            {
                return basePanel != null;
            }

            protected virtual void AddEvent()
            {

            }
        }
    }
}

