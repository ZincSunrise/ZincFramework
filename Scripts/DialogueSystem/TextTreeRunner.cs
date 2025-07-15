using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Threading.Tasks;
using UnityEngine;
using ZincFramework.DialogueSystem.Events;
using ZincFramework.LoadServices;

namespace ZincFramework.DialogueSystem
{
    public class TextTreeRunner
    {
        public bool IsEnd => !_textShower.IsShowingText && _nowTextNode == null;

        /// <summary>
        /// 用于事件节点
        /// </summary>
        public event Action<int, object> OnEventEnter;

        /// <summary>
        /// 选择节点
        /// </summary>
        public event Action<int> OnChoiceSelected;

        public TextTree MainTextTree { get; private set; }


        private readonly ITextShower _textShower;


        private BaseTextNode _nowTextNode;


        private TaskCompletionSource<int> _choiceWaiter;


        private bool _isEnterChoice = false;

        public TextTreeRunner(DialogueConfig dialogueConfig)
        {
            _textShower = dialogueConfig.TextShower;
            _textShower.OnTextEnd += OnTextEnd;
        }

        public void SetTextTree(TextTree textTree)
        {
            MainTextTree = textTree;
            _nowTextNode = MainTextTree.RootTextNode;
        }

        public void ShowOneText(string name, string text)
        {
            if (!string.IsNullOrEmpty(name))
            {
                _textShower.SetRoleSprite(AssetLoadManager.LoadAsset<Sprite>(name));
            }
            else
            {
                _textShower.SetRoleSprite(null);
            }

            _textShower.ShowTextAsync(name, text);
        }

        public void ShowText()
        {
            if(_nowTextNode == null || _isEnterChoice)
            {
                return;
            }

            if (_textShower.IsShowingText)
            {
                _textShower.CompleteImmidately();
            }
            else
            {
                ShowOneText(_nowTextNode.StaffName, _nowTextNode.DialogueText);
            }
        }

        public void SetChoice(int choiceIndex)
        {
            _choiceWaiter.SetResult(choiceIndex);
        }

        public async void OnTextEnd()
        {
            if (_nowTextNode is ChoiceNode choiceNode)
            {
                _isEnterChoice = true;
                OnEventEnter?.Invoke(TextEvents.ChoiceEvent, choiceNode.ChoiceInfos);
                _choiceWaiter = new TaskCompletionSource<int>();
                choiceNode.ChoiceIndex = await _choiceWaiter.Task;
                _choiceWaiter = null;
                _isEnterChoice = false;

                _nowTextNode = _nowTextNode?.GetNextNode();
                ShowText();
            }
            else 
            {
                _nowTextNode = _nowTextNode?.GetNextNode();
            }
        }
    }
}