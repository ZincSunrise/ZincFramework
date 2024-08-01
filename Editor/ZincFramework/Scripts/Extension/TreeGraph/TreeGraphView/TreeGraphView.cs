using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using ZincFramework.Load.Editor;



namespace ZincFramework
{
    namespace UI
    {
        namespace TreeExtension
        {
            public class TreeGraphView : GraphView
            {
                public new class UxmlFactory : UxmlFactory<TreeGraphView, GraphView.UxmlTraits> { }

                public TreeGraphView()
                {
                    Insert(0, new GridBackground());
                    style.flexGrow = 1;
                    StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>("Editor/ZincFramework/Scripts/Extension/TreeGraph/TreeGraphView/TreeGraphView");
                    this.styleSheets.Add(styleSheet);

                    this.AddManipulator(new ContentZoomer());
                    this.AddManipulator(new ContentDragger());
                    this.AddManipulator(new RectangleSelector());
                    this.AddManipulator(new SelectionDragger());
                }

                public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
                {
                    base.BuildContextualMenu(evt);
                    evt.menu.AppendAction("AddNode", AddNode);
                }

                public void AddNode(DropdownMenuAction dropdownMenuAction)
                {
                    TreeNodeView node = new TreeNodeView();
                    AddElement(node);
                }
            }
        }
    }
}