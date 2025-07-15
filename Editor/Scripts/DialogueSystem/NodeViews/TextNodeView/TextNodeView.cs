using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using ZincFramework.Events;
using ZincFramework.TreeService;
using ZincFramework.DialogueSystem.TextData;
using ZincFramework.TreeService.GraphView;
using System;




namespace ZincFramework.DialogueSystem.GraphView
{
    public class TextNodeView : NodeView
    {
        #region 加载路径
        private static readonly string _loadPath = "Assets/Editor/ZincFramework/DialogueSystem/NodeViews/TextNodeView/TextNodeView";

        private static readonly string _loadSheetPath = _loadPath + ".uss";

        private static readonly string _loadNodePath = _loadPath + ".uxml";
        #endregion

        public event ZincAction OnValueChanged;

        public override BaseNode DisplayNode => TextNode;

        public BaseTextNode TextNode { get; }

        private readonly TextField _nameField;

        private readonly TextField _dialogueField;

        private readonly Label _dialogueTitle;

        protected List<VisibleState> _visibleStates;

        private string _preState;

        public static TextNodeView CreateTextNodeView(BaseTextNode baseTextNode) => baseTextNode switch
        {
            ChoiceNode choiceNode => new ChoiceNodeView(choiceNode),
            ConditionNode conditionNode => new ConditionNodeView(conditionNode),
            EventNode eventNode => new EventNodeView(eventNode),
            RandomNode randomNode => new RandomNodeView(randomNode),
            _ => new TextNodeView(baseTextNode)
        };

        protected TextNodeView(BaseTextNode baseTextNode, string loadPath) : base(baseTextNode, loadPath)
        {
            this.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(_loadSheetPath));
            TextNode = baseTextNode;
            baseTextNode.OnStateChanged += OnStateChanged;

            _dialogueField = this.Q<TextField>("dialogueField");
            _nameField = this.Q<TextField>("nameField");
            

            this.Q<Label>("description").text = GetDescription();
            _dialogueField.value = baseTextNode.DialogueText;
            _nameField.value = baseTextNode.StaffName;

            _dialogueField.RegisterValueChangedCallback(SetDialogue);
            _nameField.RegisterValueChangedCallback(SetName);

            this.Q<VisualElement>("node-border").AddToClassList(baseTextNode is IMutipleNode<BaseTextNode> ? "mutipleNode" : "singleNode");
            CreateInputPort();
            CreateOutPort();
        }
        
        protected TextNodeView(BaseTextNode baseTextNode) : this(baseTextNode, _loadNodePath)
        {
            
        }

        protected void SetPortColor(Port port, Direction direction)
        {
            port.portName = string.Empty;
            Color color;

            if (direction == Direction.Output)
            {
                if (ColorUtility.TryParseHtmlString(TextNode.OutputHtmlColor, out color))
                {
                    this.Q<VisualElement>("output").style.backgroundColor = color;                 
                }
                outputContainer.Add(port);
            }
            else
            {
                if (ColorUtility.TryParseHtmlString(TextNode.InputHtmlColor, out color))
                {
                    this.Q<VisualElement>("input").style.backgroundColor = color;
                }
                inputContainer.Add(port);
            }

            port.portColor = color;
        }

        protected void CreateInputPort()
        {
            if (TextNode is not RootTextNode)
            {
                InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
                SetPortColor(InputPort, Direction.Input);
            }
        }

        protected virtual void CreateOutPort()
        {
            OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            SetPortColor(OutputPort, Direction.Output);
        }

        public Edge ConnectToView(TextNodeView otherView)
        {
            if(TextNode.IsEndNode || otherView == null)
            {
                return null;
            }

            return OutputPort.ConnectTo(otherView.InputPort);
        }

        protected virtual string GetDescription() => TextNode switch
        {
            EventNode => "事件节点,节点结束\n后会发生事件",
            EffectNode => "效果节点,节点开\n始时会触发一系列效果",
            RootTextNode => "根节点,无法删除",
            RandomNode => "随机节点,随机选择下一个节点进行对话",
            SingleTextNode singleTextNode when singleTextNode.IsEndNode => "结束节点,节点会指向\n下一个文本",
            SingleTextNode => "单文本节点,节点开始\n时会出现文字",
            _ => "文本节点"
        };

        protected void OnStateChanged(BaseNode.NodeState nodeState)
        {
            if (!string.IsNullOrEmpty(_preState))
            {
                RemoveFromClassList(_preState);
            }

            _preState = nodeState.ToString();
            AddToClassList(_preState);
        }

        private void SetDialogue(ChangeEvent<string> newDialogue)
        {
            if (newDialogue.newValue != newDialogue.previousValue)
            {
                TextNodeUtility.SetNodeStatement(TextNode, newDialogue.newValue);
                OnValueChanged?.Invoke();
            }
        }

        private void SetName(ChangeEvent<string> newName)
        {
            if(newName.newValue != newName.previousValue)
            {
                TextNodeUtility.SetNodeName(TextNode, newName.newValue);
                OnValueChanged?.Invoke();
            }
        }

        private void ListChanged()
        {
            TextNode.VisibleStates = _visibleStates.ToArray();
        }
    }
}
