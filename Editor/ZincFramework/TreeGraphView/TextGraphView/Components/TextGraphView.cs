using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Events;



namespace ZincFramework.TreeGraphView.TextTree
{
    public class TextGraphView : TreeGraphView<TextNodeView>
    {
        public event ZincAction<BaseNode> OnSelectNode;

        public event ZincAction<Edge> OnSelectEdge;

        public TextTree NowTextTree { get; set; }

        private readonly HashSet<GraphElement> _graphElements = new HashSet<GraphElement>();

        public new class UxmlFactory : UxmlFactory<TextGraphView, GraphView.UxmlTraits> { }


        public TextGraphView(): base()
        {
            
        }

        public override void AddToSelection(ISelectable selectable)
        {
            if (selectable is Edge edge)
            {
                OnSelectEdge?.Invoke(edge);
            }

            base.AddToSelection(selectable);
        }

        public void OnUndoPerformed()
        {
            
            AssetDatabase.SaveAssets();
            PaintNodes(NowTextTree);
        }

        private GraphViewChange OngraphViewChanged(GraphViewChange graphViewChange)
        {
            if(graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    BaseTextNode parent = (edge.output.node as TextNodeView).TextNode;
                    BaseTextNode child = (edge.input.node as TextNodeView).TextNode;
                    NowTextTree.SetChild(parent, child);

                    if (parent is ChoiceNode choiceNode)
                    {
                        (this.GetElementByGuid(choiceNode.guid) as ChoiceNodeView).Refrash();
                    }
                }
            }

            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var elem in graphViewChange.elementsToRemove)
                {
                    if (elem is TextNodeView nodeView)
                    {
                        NowTextTree.RemoveNode(nodeView.TextNode); 
                    }
                    else if (elem is Edge edge)
                    {
                        if (edge.output.node is TextNodeView parent && edge.input.node is TextNodeView child)
                        {                        
                            NowTextTree.BreakChild(parent.TextNode, child.TextNode);
                            if (parent is ChoiceNodeView choiceNodeView)
                            {
                                choiceNodeView.Refrash();
                            }
                        }
                    }
                }
            }
            return graphViewChange;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if (evt.target is TextNodeView nodeView)
            {
                if (nodeView.TextNode is not RootTextNode) 
                {
                    evt.menu.AppendAction("DisconnectAll", (evt) => DisconnectAll(), (a) => DisconnectAllStatus(nodeView, a));
                    evt.menu.AppendSeparator();

                    evt.menu.AppendAction($"DeleteNode", (data) => DeleteSelection());
                }
            }
            else if(evt.target is Edge edge)
            {
                evt.menu.AppendAction($"DeleteEdge", (data) => DeleteEdge(edge));
            }
            else
            {
                Vector2 position = evt.localMousePosition; 
                var collection1 = TypeCache.GetTypesDerivedFrom<ISingleNode<BaseTextNode>>();

                foreach (var type in collection1)
                {
                    if (type != typeof(RootTextNode))
                    {
                        evt.menu.AppendAction($"[{GetTypeParentName(type)}] Create{type.Name}", (action) => CreateNodeView(position, type));       
                    }
                }

                evt.menu.AppendSeparator();
                var collection2 = TypeCache.GetTypesDerivedFrom<IMutipleNode<BaseTextNode>>();

                foreach (var type in collection2)
                {
                    evt.menu.AppendAction($"[{GetTypeParentName(type)}] Create{type.Name}", (action) => CreateNodeView(position, type));
                }

                evt.menu.AppendSeparator();
                evt.menu.AppendAction($"[EndTextNode] CreateEndTextNode", (action) => CreateNodeView(position, typeof(EndTextNode)));
            }
        }

        private DropdownMenuAction.Status DisconnectAllStatus(TextNodeView textNodeView, DropdownMenuAction a)
        {
            if (textNodeView.OutputPort != null && textNodeView.OutputPort.connected)
            {
                return DropdownMenuAction.Status.Normal;
            }

            else if (textNodeView.InputPort != null && textNodeView.InputPort.connected)
            {
                return DropdownMenuAction.Status.Normal;
            }

            return DropdownMenuAction.Status.Disabled;
        }

        public void PaintNodes(TextTree textTree)
        {
            graphViewChanged -= OngraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OngraphViewChanged;

            NowTextTree = textTree;
            if(textTree.RootTextNode == null)
            {
                EditorUtility.SetDirty(textTree);
                textTree.CreateNode(typeof(RootTextNode));
                AssetDatabase.SaveAssets();
            }

            for (int i = 0; i < textTree.Nodes.Count; i++) 
            {
                PaintNodeView(textTree.Nodes[i]);
            }

            for (int i = 0; i < textTree.Nodes.Count; i++)
            {
                if(textTree.Nodes[i] is not EndTextNode)
                {
                    ConnectNodeView(textTree.Nodes[i]);
                }      
            }
        }

        private void CreateNodeView(Vector2 position, Type type)
        {
            BaseTextNode baseTextNode = NowTextTree.CreateNode(type);
            TextNodeView textNodeView = PaintNodeView(baseTextNode);

            baseTextNode.position = position;
            textNodeView.SetVecPosition(position);
        }

        private void ConnectNodeView(BaseTextNode parent)
        {
            TextNodeView parentView = this.GetElementByGuid(parent.guid) as TextNodeView;

            if (parent is IMutipleNode<BaseTextNode> mutiMode)
            {
                foreach (var node in mutiMode.GetChildren())
                {
                    CreateEdge(parentView, node);
                }
            }
            else if(parent is ISingleNode<BaseTextNode> singleNode)
            {
                CreateEdge(parentView, singleNode.Child);
            }
        }

        private void CreateEdge(TextNodeView parentView, BaseTextNode baseTextNode)
        {
            TextNodeView childView = this.GetElementByGuid(baseTextNode?.guid) as TextNodeView;
            var edge = parentView.ConnectToView(childView);
            
            if (edge != null)
            {
                Add(edge);
            }
        }

        private TextNodeView PaintNodeView(BaseTextNode baseTextNode)
        {
            var node = TextNodeView.CreateTextNodeView(baseTextNode);
            node.OnValueChanged += SetTextTreeDirty;
            node.OnSelecting += OnSelectNode;
            AddElement(node);

            return node;
        }

        private void SetTextTreeDirty()
        {
            EditorUtility.SetDirty(NowTextTree);
        }

        private void DisconnectAll()
        {
            TextNodeView textNodeView = selection[0] as TextNodeView;
            DeleteElements(textNodeView.DisconnectAll());
        }

        private void DeleteEdge(Edge edge)
        {
            _graphElements.Add(edge);
            GetInputOutput(edge, out var parent, out var child);

            NowTextTree.BreakChild(parent.TextNode, child.TextNode);
            DeleteElements(_graphElements);
            _graphElements.Clear();
        }

        private string GetTypeParentName(Type type) => type switch
        {
            not null when typeof(ISingleNode<BaseTextNode>).IsAssignableFrom(type) => "SingleTextNode",
            not null when typeof(IMutipleNode<BaseTextNode>).IsAssignableFrom(type) => "MutipleTextNode",
            _ => throw new NotSupportedException($"{type.Name}不是任何一个基础文本节点的派生类型")
        };

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            HashSet<Edge> connection = startPort.connections as HashSet<Edge>;
            HashSet<Port> newPorts = ports.ToHashSet();

            newPorts.RemoveWhere(x => x.direction == startPort.direction);

            foreach(var edge in connection)
            {
                newPorts.Remove(edge.input);
            }

            return newPorts.ToList();
        }

        public void GetInputOutput(Edge edge, out TextNodeView parent, out TextNodeView child)
        {
            child = edge.input.node as TextNodeView;
            parent = edge.output.node as TextNodeView;
        }
    }
}