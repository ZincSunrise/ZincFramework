using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using ZincFramework.Load.Editor;



namespace ZincFramework.TreeGraphView
{
    public abstract class TreeGraphView<T> : GraphView
    {
        public TreeGraphView()
        {
            Insert(0, new GridBackground());
            style.flexGrow = 1;

            string loadPath = Path.Combine(AssetDataManager.FrameworkLoadPath, "TreeGraphView", nameof(VisualElement) + 's', nameof(TreeGraphView));

            StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>(loadPath);
            styleSheets.Add(styleSheet);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
        }
    }
}
