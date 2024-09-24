using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Load.Editor;


namespace ZincFramework.TreeGraphView
{
    public abstract class NodeInspector<T> : VisualElement where T : Object
    {
        public T[] AllTrees;

        public NodeInspector() : base()
        {
            AllTrees = AssetDataManager.LoadAllTypeAsset<T>();
        }
    }
}