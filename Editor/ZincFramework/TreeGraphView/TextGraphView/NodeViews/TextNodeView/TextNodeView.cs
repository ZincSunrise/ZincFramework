using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using ZincFramework.Events;



namespace ZincFramework.TreeGraphView.TextTree
{
    public class TextNodeView : NodeView
    {
        private static readonly string _loadPath = Path.Combine("Assets/Editor/Zincframework/TreeGraphView/TextGraphView/NodeViews/TextNodeView/TextNodeView.uxml");


        public event ZincAction OnValueChanged;

        public override BaseNode BaseNode => TextNode;

        public BaseTextNode TextNode { get; }

        public TextField DialogueField { get; private set; }

        public DropdownField StaffField { get; private set; }

        public IntegerField DifferentialField { get; private set; }

        public Label DialogueTitle { get; private set; }

        public static TextNodeView CreateTextNodeView(BaseTextNode baseTextNode) => baseTextNode switch
        {
            ChoiceNode choiceNode => new ChoiceNodeView(choiceNode),
            _ => new TextNodeView(baseTextNode)
        };

        protected TextNodeView(BaseTextNode baseTextNode, string loadPath) : base(baseTextNode, loadPath)
        {
            TextNode = baseTextNode;

            DialogueField = this.Q<TextField>(nameof(DialogueField));
            StaffField = this.Q<DropdownField>(nameof(StaffField));
            DifferentialField = this.Q<IntegerField>(nameof(DifferentialField));

            this.Q<Label>("description").text = GetDescription();
            DialogueField.value = baseTextNode.DialogueText;
            StaffField.value = baseTextNode.StaffName;
            DifferentialField.value = baseTextNode.Differential;

            StaffField.RegisterValueChangedCallback(SetStaff);
            DialogueField.RegisterValueChangedCallback(SetStatement);
            DifferentialField.RegisterValueChangedCallback(SetDifferential);

            this.Q<VisualElement>("node-border").AddToClassList(baseTextNode is IMutipleNode<BaseTextNode> ? "mutipleNode" : "singleNode");

            TextNodeUtility.AddStaffItem(StaffField);
            CreateInputPort();
            CreateOutPort();
        }

        protected TextNodeView(BaseTextNode baseTextNode) : this(baseTextNode, _loadPath)
        {

        }


        protected void SetPortColor(Port port, Direction direction)
        {
            port.portName = string.Empty;
            Color color;

            if (direction == Direction.Output)
            {
                if (ColorUtility.TryParseHtmlString(BaseNode.OutputHtmlColor, out color))
                {
                    this.Q<VisualElement>("output").style.backgroundColor = color;                 
                }
                outputContainer.Add(port);
            }
            else
            {
                if (ColorUtility.TryParseHtmlString(BaseNode.InputHtmlColor, out color))
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
            if (TextNode is IMutipleNode<BaseTextNode>)
            {
                OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (TextNode is not EndTextNode)
            {
                OutputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            }

            if (OutputPort != null)
            {
                SetPortColor(OutputPort, Direction.Output);
            }
        }

        public Edge ConnectToView(TextNodeView otherView)
        {
            if(this.TextNode is EndTextNode || otherView == null)
            {
                return null;
            }

            return OutputPort.ConnectTo(otherView.InputPort);
        }

        private string GetDescription() => BaseNode switch
        {
            EventNode => "事件节点,节点结束\n后会发生事件",
            EffectNode => "效果节点,节点开\n始时会触发一系列效果",
            ChoiceNode => "选择节点,节点结束后会出现选择分支",
            ConditionNode => "条件集合节点,节点结束后会根据\n现有的条件选择权重最高的节点",
            RootTextNode => "根节点,无法删除",
            EndTextNode => "结束节点,节点会指向\n下一个文本",
            SingleTextNode => "单文本节点,节点开始\n时会出现文字",
            _ => "文本节点"
        };


        private void SetStatement(ChangeEvent<string> newStatement)
        {
            TextNodeUtility.SetNodeStatement(TextNode, newStatement.newValue);
            OnValueChanged?.Invoke();
        }

        private void SetStaff(ChangeEvent<string> newStaffId)
        {
            TextNodeUtility.SetNodeName(TextNode, newStaffId.newValue);
            OnValueChanged?.Invoke();
        }

        private void SetDifferential(ChangeEvent<int> newDifferential)
        {
            TextNodeUtility.SetNodeDifferential(TextNode, newDifferential.newValue);
            OnValueChanged?.Invoke();
        }
    }
}
