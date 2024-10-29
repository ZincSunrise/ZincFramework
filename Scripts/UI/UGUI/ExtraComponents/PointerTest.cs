using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ZincFramework.UI;

namespace GameSystem
{
    namespace UI
    {
        namespace Test
        {
            public class PointerTest : MonoBehaviour, IPointerClickHandler
            {
                public RectTransform MyRect { get; private set; }

                private void Awake()
                {
                    MyRect = transform as RectTransform;
                }

                public void OnPointerClick(PointerEventData eventData)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(MyRect, eventData.position, UIManager.Instance.UICamera, out var localPoint);
                    print(localPoint);
                }
            }
        }
    }
}