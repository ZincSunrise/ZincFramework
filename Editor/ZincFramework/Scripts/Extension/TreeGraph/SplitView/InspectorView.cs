using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace ZincFramework
{
    namespace UI
    {
        namespace TreeExtension
        {
            public class InspectorView : VisualElement
            {
                public new class UxmlFactory : UxmlFactory<InspectorView> { }

                public InspectorView() { }
            }
        }
    }
}