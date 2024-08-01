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
            public class SplitView : TwoPaneSplitView
            {
                public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }

                public SplitView() 
                {

                }
            }
        }
    }
}