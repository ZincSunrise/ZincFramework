using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZincFramework.Events;
using ZincFramework.Loop;


namespace ZincFramework.DialogueSystem
{
    /// <summary>
    /// 普通galgame的文字显示
    /// </summary>
    public class WarpTextShower : MonoBehaviour, ITextShower
    {
        public bool IsShowingText { get; private set; }
        /// <summary>
        /// 开始时触发的事件
        /// </summary>
        public ZincAction OnTextBegin { get; set; }

        /// <summary>
        /// 进行到第几个字的时候触发的事件
        /// </summary>
        public event ZincAction<int> OnTextShowing;

        /// <summary>
        /// 结束时触发的事件
        /// </summary>
        public ZincAction OnTextEnd { get; set; }

        [SerializeField]
        private Text _textDialogue;

        [SerializeField]
        private Image _imageCharacter;

        [SerializeField]
        private Text _textName;

        [SerializeField]
        private float _defaultOffset = 0.2f;

        private int _nowIndex;

        private string _nowShowingText;

        private WaitForSecondsRealtime _dialogueOffset;

        private void Awake()
        {
            if (_textDialogue == null)
            {       
                if (!TryGetComponent<Text>(out _textDialogue))
                {
                    Debug.LogError("必须要挂载一个对话Text组件");
                    return;
                }
            }

            if (_textDialogue == null)
            {
                if (!TryGetComponent<Text>(out _textName))
                {
                    Debug.LogError("必须要挂载一个姓名Text组件");
                    return;
                }
            }

            if (_imageCharacter == null)
            {
                if (!TryGetComponent<Image>(out _imageCharacter))
                {
                    Debug.LogError("必须要挂载一个立绘Image组件");
                    return;
                }
            }

            _textDialogue.alignment = TextAnchor.UpperLeft;
            _dialogueOffset = new WaitForSecondsRealtime(_defaultOffset);
        }


        public void SetRoleSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                _imageCharacter.color = new Color(1, 1, 1, 0);
            }
            else
            {
                _imageCharacter.color = new Color(1, 1, 1, 1);
            }

            _imageCharacter.sprite = sprite;
        }

        public void ShowTextAsync(string name, string text)
        {
            _textName.text = name;
            if(_defaultOffset == 0)
            {
                SkipText();
                return;
            }

            _dialogueOffset.waitTime = _defaultOffset;
            ShowTextInternal(text);
        }

        private void ShowTextInternal(string text)
        {
            _nowShowingText = text;
            _dialogueOffset.Reset();
            ZincLoopSystem.StartCoroutine(nameof(WarpTextShower), ShowTextCoroutine());
        }


        private IEnumerator ShowTextCoroutine()
        {
            StartShowText();

            for (_nowIndex = 0; _nowIndex < _nowShowingText.Length; _nowIndex++)
            {
                _textDialogue.text = _nowShowingText[.._nowIndex];
                OnTextShowing?.Invoke(_nowIndex);
                yield return _dialogueOffset;
            }

            EndShowText();
        }


        public void CompleteImmidately()
        {
            EndShowText();
            ZincLoopSystem.StopCoroutine(nameof(WarpTextShower));

            _nowIndex = _nowShowingText.Length - 1;
        }

        public void StartShowText()
        {
            IsShowingText = true;
            OnTextBegin?.Invoke();
        }

        public void EndShowText()
        {
            _textDialogue.text = _nowShowingText;
            _nowShowingText = string.Empty;
            IsShowingText = false;
            OnTextEnd?.Invoke();
        }

        public void SkipText()
        {
            OnTextBegin?.Invoke();
            _textDialogue.text = _nowShowingText;
            OnTextEnd?.Invoke();

            IsShowingText = true;
        }
    }
}