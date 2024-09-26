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
            EventNode => "�¼��ڵ�,�ڵ����\n��ᷢ���¼�",
            EffectNode => "Ч���ڵ�,�ڵ㿪\nʼʱ�ᴥ��һϵ��Ч��",
            ChoiceNode => "ѡ��ڵ�,�ڵ����������ѡ���֧",
            ConditionNode => "�������Ͻڵ�,�ڵ����������\n���е�����ѡ��Ȩ����ߵĽڵ�",
            RootTextNode => "���ڵ�,�޷�ɾ��",
            EndTextNode => "�����ڵ�,�ڵ��ָ��\n��һ���ı�",
            SingleTextNode => "���ı��ڵ�,�ڵ㿪ʼ\nʱ���������",
            _ => "�ı��ڵ�"
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
