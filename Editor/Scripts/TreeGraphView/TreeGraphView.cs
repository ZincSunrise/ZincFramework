using System.IO;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using ZincFramework.LoadServices.Editor;


namespace ZincFramework.TreeService.GraphView
{
    public abstract class TreeGraphView<T> : UnityEditor.Experimental.GraphView.GraphView where T : NodeView
    {
        public TreeGraphView()
        {
            Insert(0, new GridBackground());
            style.flexGrow = 1;

            string loadPath = Path.Combine(AssetDataManager.FrameworkLoadPath, "TreeGraphView", "TreeGraphView");

            StyleSheet styleSheet = AssetDataManager.LoadAssetAtPath<StyleSheet>(loadPath);
            styleSheets.Add(styleSheet);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new SelectionDragger());
        }
    }
}
