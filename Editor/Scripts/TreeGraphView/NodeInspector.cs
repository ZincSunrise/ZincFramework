using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.LoadServices.Editor;


namespace ZincFramework.TreeService.GraphView
{
    public abstract class NodeInspector<T> : VisualElement where T : Object
    {
        public T[] AllTrees;

        public NodeInspector() : base()
        {
            this.style.flexBasis = 1;
            AllTrees = AssetDataManager.LoadAllTypeAsset<T>();
        }
    }
}