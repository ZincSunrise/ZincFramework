using UnityEngine;
using UnityEngine.UI;
using ZincFramework.Events;
using static UnityEngine.UI.Image;

namespace ZincFramework
{
    namespace UI
    {
        public class ProgressBar : MonoBehaviour
        {
            public float Progress { get => _imageProgress.fillAmount; set => _imageProgress.fillAmount = value; }

            [SerializeField]
            private FillMethod _fillMethod;

            [SerializeField]
            private Image _imageProgress;

            public ZincEvent<float> OnProgressChanged { get; } = new ZincEvent<float>();

            private void Awake()
            {
                _imageProgress ??= GetComponent<Image>();
                _imageProgress.type = Type.Filled;
                _imageProgress.fillMethod = _fillMethod;
            }
        }
    }
}
