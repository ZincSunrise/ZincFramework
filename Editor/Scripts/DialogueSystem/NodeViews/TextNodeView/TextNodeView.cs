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
        #region ����·��
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

        private ListView _listView;

        private string _preState;

        public static TextNodeView CreateTextNodeView(BaseTextNode baseTextNode) => baseTextNode switch
        {
            ChoiceNode choiceNode => new ChoiceNodeView(choiceNode),
            ConditionNode conditionNode => new ConditionNodeView(conditionNode),
            EffectNode effectNode => new EffectNodeView(effectNode),
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
            IntialListView(baseTextNode.VisibleStates);

            this.Q<VisualElement>("node-border").AddToClassList(baseTextNode is IMutipleNode<BaseTextNode> ? "mutipleNode" : "singleNode");
            CreateInputPort();
            CreateOutPort();
        }
        
        protected TextNodeView(BaseTextNode baseTextNode) : this(baseTextNode, _loadNodePath)
        {
            
        }


        private void IntialListView(VisibleState[] visibleStates)
        {
            _visibleStates = new List<VisibleState>(visibleStates ?? Array.Empty<VisibleState>());
            _listView = new ListView(_visibleStates, 150, () => new SpriteContainer(ChangeVisiblable), (container, i) => (container as SpriteContainer).UpdateSprite(i, _visibleStates[i]));

            _listView.style.maxHeight = new StyleLength(StyleKeyword.None);
            _listView.showAddRemoveFooter = true;
            _listView.showFoldoutHeader = true;
            _listView.headerTitle = string.Empty;
            _listView.itemsAdded += x => ListChanged();
            _listView.itemsRemoved += x => ListChanged();

            this.Q("right").Add(_listView);
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
            EventNode => "�¼��ڵ�,�ڵ����\n��ᷢ���¼�",
            EffectNode => "Ч���ڵ�,�ڵ㿪\nʼʱ�ᴥ��һϵ��Ч��",
            RootTextNode => "���ڵ�,�޷�ɾ��",
            SingleTextNode singleTextNode when singleTextNode.IsEndNode => "�����ڵ�,�ڵ��ָ��\n��һ���ı�",
            SingleTextNode => "���ı��ڵ�,�ڵ㿪ʼ\nʱ���������",
            _ => "�ı��ڵ�"
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

        private void ChangeVisiblable(SpriteContainer spriteContainer)
        {         
            if(TextNode.VisibleStates.Length != _visibleStates.Count)
            {
                Array.Resize(ref TextNode.VisibleStates, _visibleStates.Count);
            }

            TextNode.VisibleStates[spriteContainer.Index] = spriteContainer.VisableState;
        }

        private void ListChanged()
        {
            TextNode.VisibleStates = _visibleStates.ToArray();
        }
    }
}
