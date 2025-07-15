using System.Collections.Generic;
using UnityEngine;
using ZincFramework.TreeService;


namespace ZincFramework.DialogueSystem
{
    public class RandomNode : MutipleTextNode
    {
        [SerializeField]
        private List<BaseTextNode> _baseTextNodes = new List<BaseTextNode>();

        public override int ChildCount => _baseTextNodes.Count;

        public override void ClearChild()
        {
            _baseTextNodes.Clear();
        }

        public override BaseNode CloneNode()
        {
            RandomNode randomNode = ScriptableObject.Instantiate(this);
            for (int i = 0; i < _baseTextNodes.Count; i++)
            {
                var node = _baseTextNodes[i].CloneNode() as BaseTextNode;
                randomNode.SetChild(i, node);
            }

            return randomNode;
        }

        public override void DestroyNode()
        {
            for (int i = 0; i < _baseTextNodes.Count; i++)
            {
                _baseTextNodes[i].DestroyNode();
            }

            base.DestroyNode();
        }

        public override void AddChild(BaseTextNode baseTextNode)
        {
            _baseTextNodes.Add(baseTextNode);
        }

        public override void RemoveChild(BaseTextNode baseTextNode)
        {
            _baseTextNodes.Remove(baseTextNode);
        }

        public override void SetChild(int index, BaseTextNode node)
        {
            _baseTextNodes[index] = node;
        }

        public override BaseTextNode GetNextNode()
        {
            return _baseTextNodes[Random.Range(0, _baseTextNodes.Count)];
        }

#if UNITY_EDITOR

        public override string InputHtmlColor => "#A755C2";

        public override string OutputHtmlColor => "#E3D3E4";

        public override List<BaseTextNode> GetChildren()
        {
            if (ArrayListUtility.IsNullOrEmpty(_baseTextNodes))
            {
                return null;
            }

            return _baseTextNodes;
        }
#endif
    }
}