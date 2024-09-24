using System;
using UnityEngine.UIElements;


namespace ZincFramework
{
    namespace TreeGraphView
    {
        namespace TextTree
        {
            public class ChoiceListController : ListViewController
            {
                protected override VisualElement MakeItem()
                {
                    if (listView.makeItem == null)
                    {
                        if (listView.bindItem != null)
                        {
                            throw new NotImplementedException("You must specify makeItem if bindItem is specified.");
                        }

                        return new TextField();
                    }

                    return listView.makeItem.Invoke();
                }

                protected override void BindItem(VisualElement element, int index)
                {
                    if (listView.bindItem == null)
                    {
                        if (listView.makeItem != null)
                        {
                            throw new NotImplementedException("You must specify bindItem if makeItem is specified.");
                        }

                        TextField label = (TextField)element;
                        label.value = listView.itemsSource[index]?.ToString() ?? "null";
                    }
                    else
                    {
                        listView.bindItem.Invoke(element, index);
                    }
                }
            }
        }
    }
}