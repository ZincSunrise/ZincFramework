using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using ZincFramework.Events;
using System.Collections.Generic;



namespace ZincFramework.TreeService.GraphView
{
    public abstract class NodeView : Node
    {
        public abstract BaseNode DisplayNode { get; }

        public Port InputPort { get; protected set; }

        public Port OutputPort { get; protected set; }

        public event ZincAction<BaseNode> OnSelecting;

        public NodeView(BaseNode baseNode, string loadPath) : base(loadPath)
        {
            this.viewDataKey = baseNode.guid;
            this.name = baseNode.name;
            this.title = baseNode.name;


            this.SetVecPosition(baseNode.position);
        }

        public HashSet<GraphElement> DisconnectAll()
        {
            HashSet<GraphElement> toDelete = new HashSet<GraphElement>();
            var list = ListPool<GraphElement>.Get();

            if (OutputPort?.connections != null)
            {
                foreach (Edge connection in OutputPort.connections)
                {
                    list.Add(connection);
                }
            }

            if (InputPort?.connections != null)
            {
                foreach (Edge connection in InputPort.connections)
                {
                    list.Add(connection);
                }
            }

            toDelete.UnionWith(list);
            toDelete.Remove(null);

            ListPool<GraphElement>.Release(list);
            return toDelete;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {

        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnSelecting?.Invoke(DisplayNode);
            this.RegisterCallback<MouseMoveEvent>(OnSelectedMouseMove);
        }

        public override void OnUnselected()
        {
            this.UnregisterCallback<MouseMoveEvent>(OnSelectedMouseMove);
        }

        public void OnSelectedMouseMove(MouseMoveEvent mouseMoveEvent)
        {
            DisplayNode.position = GetVecPosition();
            EditorUtility.SetDirty(DisplayNode);
        }


        public Vector2 GetVecPosition()
        {
            if (base.resolvedStyle.position == Position.Absolute)
            {
                return new Vector2(base.resolvedStyle.left, base.resolvedStyle.top);
            }

            return new Vector2(base.layout.xMin, base.layout.yMin);
        }

        public void SetVecPosition(Vector2 position)
        {
            base.style.position = Position.Absolute;
            base.style.left = position.x;
            base.style.top = position.y;
        }
    }
}