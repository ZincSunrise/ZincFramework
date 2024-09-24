using System;
using System.Collections;
using UnityEngine.UIElements;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class ChoiceListView : ListView
            {
                public new class UxmlFactory : UxmlFactory<ChoiceListView> { }

                public ChoiceListView(IList itemsSource, float itemHeight = -1f, Func<VisualElement> makeItem = null, Action<VisualElement, int> bindItem = null) : base(itemsSource, itemHeight, makeItem, bindItem)
                {

                }

                public ChoiceListView() : base() { }

                protected override CollectionViewController CreateViewController()
                {
                    return new ChoiceListController();
                }
            }
        }
    }
}
